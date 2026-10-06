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
        private  const int InstructorSimilarityScore = 8;
        public static bool SharesGroupWithInstructor(this Participant student, List<Participant> instructors)
        {
            return instructors.Any(instructor => student.SarGroupId == instructor.SarGroupId);
        }

        public static double GetSimilarityScore(this ScenarioAssignment scenarioAssignment, List<ScenarioAssignment> assignments, List<ScenarioAssignment> pastAssignments)
        {
            double similarityScore = 0;

            //do we have the same instructor?
            if(pastAssignments.Any(o => o.InstructorId == scenarioAssignment.InstructorId && o.StudentId == scenarioAssignment.StudentId))
            {
                similarityScore += InstructorSimilarityScore;
            }

            //do we have any of the same teammates?
            List<Guid> studentsWithThisInstructor = assignments.Where(o => o.InstructorId == scenarioAssignment.InstructorId && o.StudentId != scenarioAssignment.StudentId).Select(o=>o.StudentId).ToList();
            Guid PastInstructorId = pastAssignments.FirstOrDefault(o=>o.StudentId == scenarioAssignment.StudentId)?.InstructorId ?? Guid.Empty;
            List<Guid> studentsWithPastInstructor = pastAssignments.Where(o => o.InstructorId == PastInstructorId && o.StudentId != scenarioAssignment.StudentId).Select(o => o.StudentId).ToList();

            similarityScore += studentsWithThisInstructor.Intersect(studentsWithPastInstructor).Count();
            
            return similarityScore;
        }

        public static List<ScenarioAssignment> CreateAssignmentsSecondRound(List<Scenario> scenarios, List<Participant> participants, List<ScenarioAssignment> pastAssignments)
        {
            // Implementation for creating assignments in the second round
            
            
            double lowestSimilarityScore = 99;

            List<List<ScenarioAssignment>> AssignmentOptions = new List<List<ScenarioAssignment>>();

            int maxTries = 300;
            int tries = 0;
            while(tries < maxTries)
            {
                List<ScenarioAssignment> assignments = CreateAssignments(scenarios, participants);

                double similarityScore = assignments.Max(o => o.GetSimilarityScore(assignments, pastAssignments));
                if(similarityScore < lowestSimilarityScore)
                {
                    lowestSimilarityScore = similarityScore;
                    AssignmentOptions.Add(assignments);
                }
                tries++;
            }
                
            if(AssignmentOptions == null || AssignmentOptions.Count == 0)
            {
                throw new Exception("Unable to create assignments with a low enough similarity score.");
            }
            return AssignmentOptions.Last();
        }

        public static List< ScenarioAssignment> CreateAssignments(List<Scenario> scenarios, List<Participant> participants)
        {
            List<ScenarioAssignment> assignments = new List<ScenarioAssignment>();
            
            List<Participant> instructors = participants.Where(o => o.Type == ParticipantType.Instructor)
                .OrderBy(o => o.SarGroupName).ThenBy(o => o.FirstName).ThenBy(o => o.LastName).ToList();
            //By sorting the students to put those who share a group with an instructor to the top, the system should prioritize getting them assigned
            //and reduce the chances of assignment conflicts.
            List<Participant> students = participants.Where(o=>o.Type == ParticipantType.Student)
                .OrderBy(o=> !o.SharesGroupWithInstructor(instructors))
                .ThenBy(o=> Guid.NewGuid()).ToList();
          

            if(instructors.Count == 0)
            {
                throw new Exception("There are no instructors available for assignment."); 
            }
            if(students.Count == 0)
            {
                throw new Exception("There are no students available for assignment.");
            }

            //Ideally, each instructor will have one student assigned per scenario
            //We need to make sure instructors don't get assigned students from the same SarGroup as themas

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
