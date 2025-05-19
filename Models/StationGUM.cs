namespace Dashboard.Models
{
    public class StationGUM
    {
        public int Id { get; set; }

        public int StationId { get; set; }
        public Station Station { get; set; }

        public double Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
