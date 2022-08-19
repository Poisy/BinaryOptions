namespace Domain.Entities
{
    public class Currency
    {
        public string Name { get; set; }

        public OHLCModel OHLC { get; set; }

        public double Value { get; set; }
    }
}