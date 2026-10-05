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
                .OrderBy(o=> Guid.NewGuid()).ToList();
            List<Participant> instructors = participants.Where(o => o.Type == ParticipantType.Instructor)
                .OrderBy(o => o.SarGroupName).ThenBy(o => o.FirstName).ThenBy(o => o.LastName).ToList();

            if(instructors.Count == 0)
            {
                throw new Exception("There are no instructors available for assignment.");
            }
            if(students.Count == 0)
            {
                throw new Exception("There are no students available for assignment.");
            }

            //Ideally, each instructor will have one student assigned per scenario
            //We need to make sure instructors don't get assigned students from the same SarGroup as them

            double studentsPerInstructor = students.Count / instructors.Count;
            if(studentsPerInstructor > scenarios.Count)
            {
                throw new Exception("There are too many students per instructor for the number of scenarios available.");
            }

            bool[] studentAssigned = new bool[students.Count];

            foreach (Scenario scenario in scenarios.OrderBy(o=>!o.Preferred).ThenBy(o => o.Number))
            {
                foreach (Participant instructor in instructors)
                {
                    //get the next student that is not assigned and is not in the same SarGroup as the instructor
                    Participant? student = students.FirstOrDefault(o => !studentAssigned[students.IndexOf(o)] && o.SarGroupId != instructor.SarGroupId);
                    if(student != null)
                    {
                        assignments.Add(new ScenarioAssignment
                        {
                            StudentId = student.Id,
                            InstructorId = instructor.Id,
                            ScenarioId = scenario.Id,
                            AreaNumber = instructors.IndexOf(instructor) + 1
                        });
                        studentAssigned[students.IndexOf(student)] = true;
                    }
                }
            }

            if(studentAssigned.Any(o => !o))
            {
                throw new Exception("Not all students were assigned to a scenario. This is likely due to there being a conflict between student and instructor SAR groups.");
            }

            return assignments;
        }
    }
}
