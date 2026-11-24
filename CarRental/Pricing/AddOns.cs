namespace CarRental.Pricing
{
    public class ChildSeatAddOn : AddOnDecorator
    {
        private const decimal PriceRedy = 50m;
        private readonly int days;

        public ChildSeatAddOn(IRentalCost inner, int days) : base(inner)
        {
            this.days = days;
        }
        protected override string AddOnName { get { return "GPS"; } }
        protected override decimal GetAddOnPrice() { return PricePerDay * days; }
        protected override string GetPriceDetails() { return $"{days} dagar x {PricePerDay} kr"; }
    }

    public class GspAddOn : AddOnDecorator
    {
        private const decimal PricePerDay = 40m;
        private readonly int days;

        public GspAddOn(IRentalCost inner, int days) : base(inner)
        {
            this.days = days;
        }

        protected override string AddOnName { get { return "GPS"; } }
        protected override decimal GetAddOnPrice() { return PricePerDay * days; }
        protected override string GetPriceDetails() { return $"{days} dagar x {PricePerDay} kr"; }
    }

    public class ExtraDriverAddOn : AddOnDecorator
    {
        private const decimal FixedPrice = 200m;
        public ExtraDriverAddOn(IRentalCost inner) : base(inner)
        {
        }

        protected override string AddOnName { get { return "Extra förare"; } }
        protected override decimal GetAddOnPrice() { return FixedPrice; }
        protected override string GetPriceDetails() { return "engångsavgift"; }
    }
}