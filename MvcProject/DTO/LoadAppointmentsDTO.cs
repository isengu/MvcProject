namespace MvcProject.DTO
{
    public class LoadAppointmentsDTO
    {
        public int PoliclinicId { get; set; }
        public string strStartDate { 
            get { return this.StartDate.ToString("yyyy-MM-dd"); } 
            set { this.StartDate = DateOnly.ParseExact(value, "yyyy-MM-dd"); }
        }
        public DateOnly StartDate { get; set; }
        public string strEndDate
        {
            get { return this.EndDate.ToString("yyyy-MM-dd"); }
            set { this.EndDate = DateOnly.ParseExact(value, "yyyy-MM-dd"); }
        }
        public DateOnly EndDate { get; set; }
    }
}
