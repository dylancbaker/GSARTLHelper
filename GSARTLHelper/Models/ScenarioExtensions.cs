namespace GSARTLHelper.Models
{
    public static class ScenarioExtensions
    {


        public static List<Scenario> GetAllScenarios()
        {
            List<Scenario> scenarios = new List<Scenario> {
            new Scenario { Id = new Guid("7DF7AB61-EDD4-4F47-B2A6-BCC995710D1C"), Number = 1, Name = "ATV Crash", Type = ScenarioType.Rescue },
 new Scenario { Id = new Guid("5F6DBE1D-3D0C-42F6-B7C6-84D7F8153357"), Number = 2, Name = "Overturned Canoe" , Type = ScenarioType.Rescue},
 new Scenario { Id = new Guid("42F32EEC-28AA-4393-A427-0BE686ABA6C2"), Number = 3, Name = "Aircraft Crash", Type = ScenarioType.Rescue },
 new Scenario { Id = new Guid("BACCCCC7-6AD9-481A-9BF7-4368477DFF80"), Number = 4, Name = "Lost Walker", Type = ScenarioType.Rescue },
 new Scenario { Id = new Guid("A5BF162A-8E3E-4605-8808-EB1DE2B06606"), Number = 5, Name = "Mountain Biker", Type = ScenarioType.Rescue },
 new Scenario { Id = new Guid("B1138CB2-F1E2-4F29-B26C-5DE0FED80F3B"), Number = 6, Name = "Injured Rock Climber", Type = ScenarioType.Rescue },

 };
            return scenarios;
        }

        public static List<Scenario> GetScenariosByType(ScenarioType type, bool PreferredOnly)
        {
            if (PreferredOnly) { return GetAllScenarios().Where(o => o.Type == type && o.Preferred).ToList(); }
            return GetAllScenarios().Where(o => o.Type == type).ToList();
        }
    }

}
