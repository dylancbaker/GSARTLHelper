namespace GSARTLHelper.Models
{
    public class ParticipantUploadRow
    {
        //this matches the rows in the table JIBC sends out for participants, so we can use it to parse the CSV file
        public string Number { get; set; } = string.Empty;
        public string ParsedSarGroupName { get; set; } = string.Empty;
        public Guid SarGroupId { get; set; } = Guid.Empty;
        public string? SarGroupName => SarGroupExtensions.GetGroupName(SarGroupId);

        public string Name { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
