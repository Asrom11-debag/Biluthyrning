namespace CarRenal.Pricing
{
    public abstract class AddOnDecorator
    {
        private readonly IRentalCost inner;

        protected AddOnDecorator(IRentalCost inner)
        {
            if (inner == null)
                throw new AgrumentNullExepction("inner")
            this.inner = inner;
        }

        protected abstract string AddOnName { get; }
        protected abstract decimal GetAddOnCost();
        protected abstract string GetPriceDetails();

        public decimal GetPrice()
        {
            return inner.GetPrice() + GetAddOnPrice();
        }

        public string GetDescription()
        {
            return inner.GetDescription() + $"/n + {AddOnName} : {GetPriceDetails()} = {GetAddOnPrice():0.00} kr";
        }

    }
} 