#!/bin/bash

# GSARTLHelper Deployment Script
# This script handles the deployment of the GSARTLHelper application

set -e

DEPLOY_PATH="${1:-/var/www/vhosts/greathat.ca/gsartl.greathat.ca}"
SERVICE_NAME="${2:-gsartl}"
ARTIFACT_FILE="${3:-gsartl-build.tar.gz}"

echo "================================"
echo "GSARTLHelper Deployment Script"
echo "================================"
echo "Deploy Path: $DEPLOY_PATH"
echo "Service Name: $SERVICE_NAME"
echo "Artifact: $ARTIFACT_FILE"
echo ""

# Verify we're running with sudo if needed
if [ ! -w "$DEPLOY_PATH" ] && [ "$EUID" -ne 0 ]; then
  echo "Error: Need write permissions to $DEPLOY_PATH"
  echo "Please run with sudo or ensure proper permissions"
  exit 1
fi

# Create backup
echo "Creating backup of current deployment..."
BACKUP_DIR="${DEPLOY_PATH}.backup-$(date +%s)"
if [ -d "$DEPLOY_PATH" ]; then
  sudo cp -r "$DEPLOY_PATH" "$BACKUP_DIR"
  echo "✓ Backup created at: $BACKUP_DIR"
else
  echo "⚠ No existing deployment to backup"
fi

# Create deployment directory
echo "Preparing deployment directory..."
sudo mkdir -p "$DEPLOY_PATH"

# Extract artifact
if [ ! -f "$ARTIFACT_FILE" ]; then
  echo "Error: Artifact file not found: $ARTIFACT_FILE"
  exit 1
fi

echo "Extracting application files..."
sudo tar -xzf "$ARTIFACT_FILE" -C "$DEPLOY_PATH"
echo "✓ Files extracted"

# Set permissions
echo "Setting permissions..."
sudo chown -R deploy:deploy "$DEPLOY_PATH" || sudo chown -R $SUDO_USER:$SUDO_USER "$DEPLOY_PATH"
sudo chmod +x "$DEPLOY_PATH/GSARTLHelper" 2>/dev/null || true
echo "✓ Permissions set"

# Stop service
echo "Stopping $SERVICE_NAME service..."
sudo systemctl stop "$SERVICE_NAME" || true
sleep 2

# Start service
echo "Starting $SERVICE_NAME service..."
sudo systemctl start "$SERVICE_NAME"
sleep 2

# Verify service
echo "Verifying deployment..."
if sudo systemctl is-active --quiet "$SERVICE_NAME"; then
  echo "✓ Service is running"
  sudo systemctl status "$SERVICE_NAME"
  echo ""
  echo "================================"
  echo "✓ Deployment successful!"
  echo "================================"
else
  echo "✗ Service failed to start"
  echo "Attempting rollback..."
  if [ -d "$BACKUP_DIR" ]; then
    echo "Rolling back to previous version..."
    sudo rm -rf "$DEPLOY_PATH"
    sudo cp -r "$BACKUP_DIR" "$DEPLOY_PATH"
    sudo systemctl start "$SERVICE_NAME"
    echo "Rollback complete"
  fi
  exit 1
fi
