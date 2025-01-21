namespace BusApiProyect.Data.DTO
{
    public class BusScheduleDTO
    {
        public int Id { get; set; }
        public int BusForScheduleId { get; set; }
        public int RouteScheduledId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
