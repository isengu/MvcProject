namespace MvcProject.DTO
{
	public class AppointmentDateDTO
	{
		public string Major;
		public string Policlinic;
		public DateOnly Date { get; set; }
		public List<AppointmentDoctorDTO>? Doctors { get; set; }
	}

	public class AppointmentDoctorDTO
	{
		public int Id { get; set; }
		public string Name { get; set;}
		public List<AppointmentTimeDTO> Times { get; set; }
	}

	public class AppointmentTimeDTO
	{
		public int Id { get; set; }
		public string Time { get; set; }
	}
}
