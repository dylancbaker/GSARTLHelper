namespace GSARTLHelper.Models
{
    public class ScenarioAssignment
    {
        private Guid _StudentId;
        private Guid _InstructorId;
        private Guid _ScenarioId;
        private int _AreaNumber;

        public Guid StudentId { get => _StudentId; set => _StudentId = value; }
        public Guid InstructorId { get => _InstructorId; set => _InstructorId = value; }
        public Guid ScenarioId { get => _ScenarioId; set => _ScenarioId = value; }
        public int AreaNumber { get => _AreaNumber; set => _AreaNumber = value; }
    }

    public static class ScenarioAssignmentExtensions
    {
        public static List< ScenarioAssignment> CreateAssignments(List<Scenario> scenarios, List<Participant> participants)
        {
            List<ScenarioAssignment> assignments = new List<ScenarioAssignment>();
            List<Participant> students = participants.Where(o=>o.Type == ParticipantType.Student)
                .OrderBy(o=>o.SarGroupName).ThenBy(o=>o.FirstName).ThenBy(o=>o.LastName).ToList();
            List<Participant> instructors = participants.Where(o => o.Type == ParticipantType.Instructor)
                .OrderBy(o => o.SarGroupName).ThenBy(o => o.FirstName).ThenBy(o => o.LastName).ToList();

            //Ideally, each instructor will have one student assigned per scenario
            //We need to make sure instructors don't get assigned students from the same SarGroup as them

            double studentsPerInstructor = students.Count / instructors.Count;
            if(studentsPerInstructor > scenarios.Count)
            {
                throw new Exception("There are too many students per instructor for the number of scenarios available.");
            }
            
            Guid[instructors.Count, studentsPerInstructor]


            return assignments;
        }
    }
}
