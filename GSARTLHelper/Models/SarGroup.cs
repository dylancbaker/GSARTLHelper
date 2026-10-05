namespace GSARTLHelper.Models
{
    public class SarGroup
    {
        private Guid _Id = Guid.Empty;
        private string _Name = string.Empty;

        public Guid Id { get => _Id; set => _Id = value; }
        public string Name { get => _Name; set => _Name = value; }
    }
}
