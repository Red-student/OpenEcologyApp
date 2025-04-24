namespace OpenEcologyApp.Data
{
    public class EcologyData
    {
        public int Id { get; set; }
        public string Region { get; set; } = string.Empty;
        public double CO2Level { get; set; }
        public DateTime Date { get; set; }
    }
}
