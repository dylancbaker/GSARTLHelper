namespace GSARTLHelper.Models
{
    public static class ScenarioExtensions
    {


        public static List<Scenario> GetAllScenarios()
        {
            List<Scenario> scenarios = new List<Scenario> {
                new Scenario { Id = new Guid("e0d2eec1-78b3-46ba-bb8c-e2510e447936"), Number = 1, Name = "Missing Senior with Dementia", Type = ScenarioType.Search, Preferred = false },
new Scenario { Id = new Guid("36925766-67aa-4815-9e26-696d5e5eeca7"), Number = 2, Name = "Missing male with Autism", Type = ScenarioType.Search, Preferred = true },
new Scenario { Id = new Guid("86a7c4a9-d4a3-411f-85a0-8b9e0f53436c"), Number = 3, Name = "Sound Search", Type = ScenarioType.Search, Preferred = true },
new Scenario { Id = new Guid("ca5870e2-3faa-41ec-bc24-3ac693586e11"), Number = 4, Name = "Clue area search", Type = ScenarioType.Search, Preferred = true },
new Scenario { Id = new Guid("47e20581-ce76-4e27-87ea-0de90d0c5bae"), Number = 5, Name = "Missing partier", Type = ScenarioType.Search, Preferred = true },
new Scenario { Id = new Guid("c914f2e0-d817-4933-abb6-ed8e3714ddcf"), Number = 6, Name = "Missing child near creek lake", Type = ScenarioType.Search, Preferred = true },
new Scenario { Id = new Guid("0bf140cb-9693-490a-bdb7-e9f4cd0ee905"), Number = 7, Name = "SPOT activation on Hill Mountain", Type = ScenarioType.Search, Preferred = true },

            new Scenario { Id = new Guid("7DF7AB61-EDD4-4F47-B2A6-BCC995710D1C"), Number = 1, Name = "ATV Crash", Type = ScenarioType.Rescue, Preferred = true },
 new Scenario { Id = new Guid("5F6DBE1D-3D0C-42F6-B7C6-84D7F8153357"), Number = 2, Name = "Overturned Canoe" , Type = ScenarioType.Rescue, Preferred = true },
 new Scenario { Id = new Guid("42F32EEC-28AA-4393-A427-0BE686ABA6C2"), Number = 3, Name = "Aircraft Crash", Type = ScenarioType.Rescue, Preferred = false },
 new Scenario { Id = new Guid("BACCCCC7-6AD9-481A-9BF7-4368477DFF80"), Number = 4, Name = "Lost Walker", Type = ScenarioType.Rescue, Preferred = true },
 new Scenario { Id = new Guid("A5BF162A-8E3E-4605-8808-EB1DE2B06606"), Number = 5, Name = "Mountain Biker", Type = ScenarioType.Rescue, Preferred = true },
 new Scenario { Id = new Guid("B1138CB2-F1E2-4F29-B26C-5DE0FED80F3B"), Number = 6, Name = "Injured Rock Climber", Type = ScenarioType.Rescue, Preferred = true },


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
