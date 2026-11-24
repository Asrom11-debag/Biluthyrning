using CarRental.Domain;                                      // Bilar och perioder
using CarRental.Pricing;                                    // Prismodeller och tillägg
using Xunit;                                                // Testverktyget

namespace CarRental.Tests                                   // Alla tester ligger här
{
    /// <summary>Testar prismodellerna och tilläggen.</summary>
    public class PricingTests
    {
        private static readonly DateTime Today = DateTime.Today;           // Dagens datum
        private readonly Vehicle polo = new CompactCar("ABC123", "VW Polo"); // Kostar 395 kr/dag
        private readonly RentalPeriod threeDays = new RentalPeriod(Today.AddDays(1), Today.AddDays(4));  // Kort hyra
        private readonly RentalPeriod tenDays = new RentalPeriod(Today.AddDays(1), Today.AddDays(11));   // Lång hyra

        [Fact]
        public void StandardPricing_IsDailyRateTimesDays()  // 3 dagar à 395 = 1185
        {
            Assert.Equal(1185m, new StandardPricing().CalculateBasePrice(polo, threeDays));
        }

        [Fact]
        public void LongTermPricing_ShortRental_GivesFullPrice() // Under en vecka, ingen rabatt
        {
            Assert.Equal(1185m, new LongTermPricing().CalculateBasePrice(polo, threeDays)); // Vanligt pris
        }

        [Fact]
        public void LongTermPricing_SevenDaysOrMore_Gives15PercentOff() // 10 dagar ger 15 % rabatt
        {
            Assert.Equal(3357.50m, new LongTermPricing().CalculateBasePrice(polo, tenDays));
        }

        [Fact]
        public void CorporatePricing_Gives10PercentOff()    // Företag får alltid 10 %
        {
            Assert.Equal(1066.50m, new CorporatePricing().CalculateBasePrice(polo, threeDays));
        }

        [Fact]
        public void AllStrategies_AreSubstitutable()        // Alla prismodeller ska funka likadant
        {
            IPricingStrategy[] strategies = { new StandardPricing(), new LongTermPricing(), new CorporatePricing() }; // Alla tre
            foreach (IPricingStrategy s in strategies)      // Testa var och en
                Assert.True(s.CalculateBasePrice(polo, threeDays) >= 0); // Inget fel, inget minuspris
        }

        [Fact]
        public void Decorators_AddUpCorrectly()             // Tilläggen läggs på priset
        {
            IRentalCost cost = new BaseRentalCost(polo, threeDays, new StandardPricing()); // 1185 kr
            cost = new ChildSeatAddOn(cost, threeDays.Days); // plus 150
            cost = new GpsAddOn(cost, threeDays.Days);      // plus 120
            cost = new ExtraDriverAddOn(cost);              // plus 200
            Assert.Equal(1655m, cost.GetPrice());           // Ska bli 1655 kr
        }

        [Fact]
        public void PriceCalculator_CombinesStrategyAndDecorators() // Prismodell och tillägg ihop
        {
            List<AddOnType> addOns = new List<AddOnType> { AddOnType.ChildSeat, AddOnType.ExtraDriver }; // Barnstol och extra förare
            IRentalCost cost = new PriceCalculator().BuildCost(polo, tenDays, new LongTermPricing(), addOns); // Räkna ut allt
            Assert.Equal(3357.50m + 500m + 200m, cost.GetPrice()); // Rabatterat pris plus tilläggen
        }

        [Fact]
        public void Description_ContainsOneLinePerAddOn()   // Kvittot får en rad per del
        {
            IRentalCost cost = new GpsAddOn(new BaseRentalCost(polo, threeDays, new StandardPricing()), 3); // Hyra plus GPS
            string[] lines = cost.GetDescription().Split('\n'); // Dela upp kvittot i rader
            Assert.Equal(2, lines.Length);                  // En rad för hyran, en för GPS
        }
    }
}