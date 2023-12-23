namespace MvcProject.DTO
{
	public class AppointmentDTO
	{
		public int Id { get; set; }
		public int DoctorId { get; set; }
		public int UserId { get; set; }
		public string strDate
		{
			get { return this.Date.ToString("yyyy-MM-dd"); }
			set { this.Date = DateOnly.ParseExact(value, "yyyy-MM-dd"); }
		}
		public DateOnly Date { get; set; }
		public int AppointmentTimeId { get; set; }
	}
}
