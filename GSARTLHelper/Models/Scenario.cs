namespace GSARTLHelper.Models
{
    public class Scenario
    {
        private Guid _Id;
        private int _Number;
        private string _Name = string.Empty;
        private ScenarioType? _Type;
        private bool _Preferred;

        public Guid Id { get => _Id; set => _Id = value; }
        public string Name { get => _Name; set => _Name = value; }
        public ScenarioType? Type { get => _Type; set => _Type = value; }
        public string? ScenarioTypeName
        {
            get
            {
                switch (Type)
                {
                    case ScenarioType.Rescue:

                        return "Rescue";
                    case ScenarioType.Search:
                        return "Search";
                    case ScenarioType.Cumulative:
                        return "Cumulative";
                    default: return null;

                }
            }
        }

        public int Number { get => _Number; set => _Number = value; }
        public bool Preferred { get => _Preferred; set => _Preferred = value; }
    }

}
