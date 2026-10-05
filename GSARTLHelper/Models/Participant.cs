namespace GSARTLHelper.Models
{
    public class Participant
    {
        public Participant()
        {
            Id = Guid.NewGuid();
        }

        private Guid _Id = Guid.Empty;
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private Guid _SarGroupId = Guid.Empty;
        private ParticipantType _Type = ParticipantType.Student;

        public Guid Id { get => _Id; set => _Id = value; }
        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string FullName => $"{FirstName} {LastName}";
        public Guid SarGroupId { get => _SarGroupId; set => _SarGroupId = value; }
        public string? SarGroupName => SarGroupExtensions.GetGroupName(SarGroupId);

        public ParticipantType Type { get => _Type; set => _Type = value; }
    }

    public static class ParticipantExtensions
    {
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
    }
}
