# GitHub Actions Deployment Setup Guide

This guide will help you set up GitHub Actions to automatically build and deploy your GSARTLHelper application when you create a release.

## Prerequisites

Before you start, make sure you have:
- A GitHub repository with admin access
- SSH access to your Ubuntu server (`gsartl.greathat.ca`)
- `deploy` user with write access to the deployment directory and passwordless sudo for the service commands below
- systemd service named `gsartl` configured on your server
- ASP.NET Core 10.0 runtime installed on the server, with an executable `/usr/bin/dotnet` (see Step 4)

## Step 1: Generate SSH Key for GitHub Actions

Run this on your local machine:

```bash
ssh-keygen -t ed25519 -f github_actions_deploy -C "github-actions-deploy" -N ""
```

This creates two files:
- `github_actions_deploy` (private key - keep secret)
- `github_actions_deploy.pub` (public key - add to server)

## Step 2: Add Public Key to Ubuntu Server

1. Copy the public key to your server:

```bash
ssh-copy-id -i github_actions_deploy.pub deploy@gsartl.greathat.ca
# Or manually add the content of github_actions_deploy.pub to ~/.ssh/authorized_keys
```

2. Verify the connection works:

```bash
ssh -i github_actions_deploy deploy@gsartl.greathat.ca "echo 'SSH connection successful'"
```

## Step 3: Configure GitHub Secrets

Add the following secrets to your GitHub repository:

1. Go to **Settings > Secrets and variables > Actions**

2. Create these secrets:

   - **DEPLOY_HOST**: Your server's public IP address or a DNS-only SSH hostname (not proxied through Cloudflare)
   - **DEPLOY_USER**: `deploy`
   - **DEPLOY_SSH_KEY**: (paste the entire contents of `github_actions_deploy` - the private key)
   - **DEPLOY_PATH**: `/var/www/vhosts/greathat.ca/gsartl.greathat.ca`
   - **SERVICE_NAME**: `gsartl`

### How to add a secret:

1. Click "New repository secret"
2. Name: `DEPLOY_HOST`, Value: your server's public IP address or DNS-only SSH hostname
3. Repeat for other secrets
4. Click "Add secret"

## Step 4: Prepare Your Ubuntu Server

### Create/Update the systemd Service

Create or update `/etc/systemd/system/gsartl.service`:

```ini
[Unit]
Description=GSARTLHelper Application
After=network.target

[Service]
Type=simple
User=deploy
WorkingDirectory=/var/www/vhosts/greathat.ca/gsartl.greathat.ca
ExecStart=/usr/bin/dotnet /var/www/vhosts/greathat.ca/gsartl.greathat.ca/GSARTLHelper.dll
Restart=on-failure
RestartSec=10
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
```

After installing the runtime below, enable the service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable gsartl
sudo systemctl start gsartl
```

### Configure Sudoers for Deployment

Add this line to allow the deploy user to restart the service without a password prompt:

```bash
sudo visudo
```

Add at the end:

```
deploy ALL=(ALL) NOPASSWD: /bin/systemctl restart gsartl, /bin/systemctl start gsartl, /bin/systemctl stop gsartl, /bin/systemctl status gsartl
```

### Create Deployment Directory

```bash
sudo mkdir -p /var/www/vhosts/greathat.ca/gsartl.greathat.ca
sudo chown deploy:deploy /var/www/vhosts/greathat.ca/gsartl.greathat.ca
sudo chmod 755 /var/www/vhosts/greathat.ca/gsartl.greathat.ca
```

If the directory already contains an application installed by root, make its existing files writable by the deploy user as well:

```bash
sudo chown -R deploy:deploy /var/www/vhosts/greathat.ca/gsartl.greathat.ca
```

The workflow copies, extracts, and sets file permissions as `deploy`, without sudo. Backups are stored in the deploy user's home directory, so the parent of the deployment directory does not need to be writable.

Archive extraction uses `--no-overwrite-dir` so existing directory metadata is not overwritten with archive metadata. This avoids `tar: .: Cannot utime` and `Cannot change mode` failures when the deployment account can write to a directory but does not own it. Application files must still be writable by the deployment account; newly created files and directories are extracted normally.

### Install ASP.NET Core Runtime (if not already installed)

The release is framework-dependent and targets `net10.0`. The server needs both `Microsoft.NETCore.App` and `Microsoft.AspNetCore.App` 10.0; installing .NET on the Actions runner does not install it on the server.

Run these commands as a server administrator. Check the exact executable used by the systemd service, not just the `dotnet` found on your interactive shell's PATH:

```bash
# Check the service's runtime
/usr/bin/dotnet --list-runtimes

# If the ASP.NET Core 10.0 runtime is missing, install it:
wget https://dot.net/v1/dotnet-install.sh
sudo bash ./dotnet-install.sh --runtime aspnetcore --channel 10.0 --install-dir /usr/local/dotnet

# If /usr/bin/dotnet does not exist, expose the installed host at the service's path:
sudo ln -s /usr/local/dotnet/dotnet /usr/bin/dotnet

# Verify both 10.0 runtimes are visible to the service account
sudo -u deploy /usr/bin/dotnet --list-runtimes
```

Do not replace an existing `/usr/bin/dotnet` from a package-managed installation. If it exists but lacks the required runtimes, install the ASP.NET Core 10.0 runtime using that installation's package manager, or set the service's `ExecStart` to `/usr/local/dotnet/dotnet` and the application's absolute DLL path. Installing only the base .NET runtime is insufficient for this web application.

## Step 5: Configure Your Application

Ensure your application settings are correct for production:

- Update `appsettings.json` with production settings
- Set environment variables in the systemd service if needed
- Configure reverse proxy (nginx/Apache) if needed

Example nginx configuration:

```nginx
server {
    server_name gsartl.greathat.ca;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

## Step 6: Test the Workflow

1. Make a commit and push it
2. Create a release on GitHub
3. Watch the Actions tab for the workflow execution
4. Check the deployment logs

## Troubleshooting

### Workflow Fails on SSH Connection

- Verify SSH key is correct
- Check that `DEPLOY_HOST`, `DEPLOY_USER` secrets are correct
- For `dial tcp ...:22: i/o timeout`, ensure `DEPLOY_HOST` points directly to the server, not a Cloudflare proxy IP. Standard Cloudflare proxying does not forward SSH on port 22; use the server's public IP or set the SSH hostname's DNS record to **DNS only** (grey cloud).
- Ensure the server firewall allows inbound SSH on port 22 from the GitHub Actions runner.
- Test SSH manually against the same host configured in `DEPLOY_HOST`: `ssh -i private_key deploy@<DEPLOY_HOST>`

### Service Fails to Start

- Check service logs: `sudo journalctl -u gsartl -n 50`
- Verify deployment directory has correct permissions
- Ensure the ASP.NET Core 10.0 runtime is installed on the server as described in Step 4.

If status reports `203/EXEC` and `Failed at step EXEC spawning /usr/bin/dotnet: No such file or directory`, the build and extraction may have succeeded, but systemd cannot execute its configured runtime host. A successful `systemctl restart` does not guarantee that the application stays running.

Have a server administrator install the runtime and provide `/usr/bin/dotnet` as described above, or correct the service's `ExecStart` to the absolute path of an existing host with both required 10.0 runtimes. Inspect the actual unit with `sudo systemctl cat gsartl`; the running server configuration may differ from the example in this guide. After fixing it:

```bash
sudo systemctl daemon-reload
sudo systemctl restart gsartl
sudo systemctl status gsartl
```

Confirm the service remains `active (running)` and check its journal before redeploying. The workflow cannot install system runtimes or edit service units with its service-only sudo permissions. Do not grant it unrestricted sudo; retrying alone cannot fix a missing executable.

### Deployment Fails with `mkdir: Permission denied`

The build can succeed while deployment fails because `DEPLOY_USER` cannot create the configured `DEPLOY_PATH` under a root-owned parent such as `/var/www/vhosts/greathat.ca`. The workflow cannot provision that parent with its service-only sudo permissions.

Before retrying, have a server administrator run the **Create Deployment Directory** commands above on the same server configured in `DEPLOY_HOST`. Use the actual `DEPLOY_PATH` and `DEPLOY_USER` secret values; if application files already exist, apply the recursive ownership command to that application directory only. Do not change ownership of the shared `/var/www/vhosts` tree or grant unrestricted passwordless sudo.

Verify access as the deployment account (replace `deploy` and the path if your secrets differ):

```bash
sudo -u deploy sh -c 'test -d "$1" && test -w "$1" && test -x "$1"' sh /var/www/vhosts/greathat.ca/gsartl.greathat.ca
```

This command must exit successfully. The workflow checks directory access before backing up or downloading files. If access is still denied, check execute/search permissions on each parent directory as well. After repairing server access, retry the workflow; retrying alone does not fix filesystem permissions.

### Artifact Download Fails

- The workflow packages the published application as `gsartl-build.tar.gz` and attaches it to the release before deployment. The Actions build artifact alone is not a release asset.
- Ensure the workflow has `contents: write` permission to upload the release asset.
- The server downloads the asset without GitHub credentials, so the repository and release must be publicly accessible.
- Verify the artifact path in the workflow matches your build output
- Check that the release tag matches the workflow trigger

### Workflow Fails on Sudo Password Prompt

- Complete the deployment directory ownership setup above; file operations must not require sudo.
- Configure the exact service commands in sudoers, matching `SERVICE_NAME`.
- Test as the deploy user: `sudo -n systemctl restart gsartl` and `sudo -n systemctl status gsartl`.
- The workflow uses `sudo -n` to fail immediately if service permissions are missing. Do not grant unrestricted passwordless sudo or add a sudo password to the workflow.

### Redeploy an Existing Release After a Workflow Fix

Re-running a failed release run uses the original workflow revision, not a subsequently merged fix. For example, the failed `v1.0.2` deployment used sudo for file operations and stopped because SSH could not provide a password prompt.

After merging the corrected workflow into the default branch and completing the server ownership and sudoers setup above:

1. Open **Actions > Build and Deploy on Release > Run workflow**.
2. Select the default branch so the run uses the corrected workflow.
3. Enter the existing release tag (for example, `v1.0.2`) in **release_tag**.
4. Run the workflow and check the deployment and service verification logs.

The workflow checks out the selected release tag, rebuilds its application, replaces that release's `gsartl-build.tar.gz` asset, and deploys it using the corrected deployment steps. Newly published releases still deploy automatically.

### Backup/Rollback

Backups are automatically created in:
```
~/gsartl-backup-<random suffix>/application
```

To manually restore:
```bash
sudo rm -rf /var/www/vhosts/greathat.ca/gsartl.greathat.ca
sudo cp -r ~/gsartl-backup-<random suffix>/application /var/www/vhosts/greathat.ca/gsartl.greathat.ca
sudo systemctl restart gsartl
```

## Security Best Practices

1. **Rotate SSH Keys Regularly**: Replace the GitHub Actions SSH key periodically
2. **Limit SSH Key Permissions**: The deploy user should only have access to deployment directories
3. **Use Secrets Manager**: Store sensitive data in GitHub Secrets, never in workflows
4. **Monitor Deployments**: Check GitHub Actions logs and server logs for suspicious activity
5. **Backup Before Deploy**: The workflow automatically creates backups before deployment

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [appleboy/ssh-action](https://github.com/appleboy/ssh-action)
- [.NET Deployment Guide](https://learn.microsoft.com/en-us/dotnet/core/deploying/)
