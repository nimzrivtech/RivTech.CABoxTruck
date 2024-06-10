namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public class AmountInfo
    {
        public AmountInfo(object id, AmountKind kind, string description)
        {
            Id = id;
            Kind = kind;
            Description = description;
        }

        public object Id { get; }
        public AmountKind Kind { get;  }
        public string Description { get;  }

        public override string ToString()
        {
            return $"Kind: {Kind},\tDescription: {Description}";
        }
    }
}
