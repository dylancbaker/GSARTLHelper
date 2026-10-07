using System.Xml.Linq;

namespace GSARTLHelper.Models
{
    public static class ParticipantExtensions
    {
        public static Participant CreateStudentFromUpload(ParticipantUploadRow row)
        {
            var names = row.Name.Split(' ');
            var firstName = names.Length > 1 ? names[1].Trim() : string.Empty;
            var lastName = names.Length > 0 ? names[0].Trim() : string.Empty;
            var sarGroup = SarGroupExtensions.GetGroup(row.SarGroupId);
            if(sarGroup == null)
            {
                throw new Exception($"SarGroup with Id {row.SarGroupId} not found.");
            }
            return new Participant
            {
                FirstName = firstName,
                LastName = lastName,
                Type = ParticipantType.Student,
                SarGroupId = sarGroup.Id
            };
        }
        
        public static List<Participant> GetRandomParticipants(int qty, ParticipantType type)
        {
            var list = new List<Participant>();
            for (int x = 0; x < qty; x++)
            {
                list.Add(new Participant { FirstName = RandomGenerators.FirstName, LastName = RandomGenerators.LastName, Type = type, SarGroupId = RandomGenerators.GetRandomSarGroup().Id });
            }
            return list;
        }

        public static List<Participant> GetRandomStudents(int qty)        {            return GetRandomParticipants(qty, ParticipantType.Student);        }
        public static List<Participant> GetRandomInstructors(int qty) { return GetRandomParticipants(qty, ParticipantType.Instructor); }

        internal static List<ParticipantUploadRow> ParseStudentUploadFile(byte[] smallFileByteArray)
        {
            List<ParticipantUploadRow> rows = new List<ParticipantUploadRow>();

            //take the CSV data, convert it to a string, and split it into lines
            string csvData = System.Text.Encoding.UTF8.GetString(smallFileByteArray);
            string[] lines = csvData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //skip the header row and parse the remaining lines into ParticipantUploadRow objects
            for (int i = 2; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(',');
                if (columns.Length >= 7)
                {
                    if (!string.IsNullOrEmpty(columns[2].Trim()))
                    {
                        rows.Add(new ParticipantUploadRow
                        {
                            Number = columns[0].Trim(),
                            ParsedSarGroupName = columns[1].Trim(),
                            SarGroupId = SarGroupExtensions.GetBestGuessFromPartialName(columns[1].Trim())?.Id ?? Guid.Empty,
                            Name = columns[2].Trim(),
                            StudentId = columns[3].Trim(),
                            Email = columns[4].Trim(),
                            Phone = columns[5].Trim(),
                            Status = columns.Length > 6 ? columns[6].Trim() : string.Empty
                        });
                    }
                }
            }

            return rows;
        }
    }
}
