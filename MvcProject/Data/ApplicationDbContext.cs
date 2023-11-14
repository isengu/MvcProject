using Microsoft.EntityFrameworkCore;
using MvcProject.Models;

namespace MvcProject.Data;

public class ApplicationDbContext : DbContext
{
	public virtual DbSet<Appointment> Appointments { get; set; } = null!;
	public virtual DbSet<AppointmentTime> AppointmentTimes { get; set; } = null!;
	public virtual DbSet<Doctor> Doctors { get; set; } = null!;
	public virtual DbSet<Major> Majors { get; set; } = null!;
	public virtual DbSet<Policlinic> Policlinics { get; set; } = null!;
	public virtual DbSet<User> Users { get; set; } = null!;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Appointment>(entity =>
		{
			entity.ToTable("appointment", "hospital");

			entity.Property(e => e.Id).HasColumnName("id");

			entity.Property(e => e.AppointmentTimeId).HasColumnName("appointment_time_id");

			entity.Property(e => e.Date).HasColumnName("date");

			entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.AppointmentTime)
				.WithMany(p => p.Appointments)
				.HasForeignKey(d => d.AppointmentTimeId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("appointment_time_fk");

			entity.HasOne(d => d.Doctor)
				.WithMany(p => p.Appointments)
				.HasForeignKey(d => d.DoctorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("appointment_fk");

			entity.HasOne(d => d.User)
				.WithMany(p => p.Appointments)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("appointment_fk_2");
		});

		modelBuilder.Entity<AppointmentTime>(entity =>
		{
			entity.ToTable("appointment_time", "hospital");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.HasDefaultValueSql("nextval('hospital.work_day_time_id_seq'::regclass)");

			entity.Property(e => e.EndTime).HasColumnName("end_time");

			entity.Property(e => e.StartTime).HasColumnName("start_time");
		});

		modelBuilder.Entity<Doctor>(entity =>
		{
			entity.ToTable("doctor", "hospital");

			entity.Property(e => e.Id).HasColumnName("id");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.Property(e => e.Phone)
				.HasColumnType("character varying")
				.HasColumnName("phone");

			entity.Property(e => e.PoliclinicId).HasColumnName("policlinic_id");

			entity.HasOne(d => d.Policlinic)
				.WithMany(p => p.Doctors)
				.HasForeignKey(d => d.PoliclinicId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("doctor_fk");
		});

		modelBuilder.Entity<Major>(entity =>
		{
			entity.ToTable("major", "hospital");

			entity.HasIndex(e => e.Name, "major_un")
				.IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");
		});

		modelBuilder.Entity<Policlinic>(entity =>
		{
			entity.ToTable("policlinic", "hospital");

			entity.HasIndex(e => e.Name, "policlinic_un")
				.IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");

			entity.Property(e => e.MajorId).HasColumnName("major_id");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.HasOne(d => d.Major)
				.WithMany(p => p.Policlinics)
				.HasForeignKey(d => d.MajorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("policlinic_fk");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user", "hospital");

			entity.HasIndex(e => e.Email, "user_un")
				.IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");

			entity.Property(e => e.Email)
				.HasColumnType("character varying")
				.HasColumnName("email");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.Property(e => e.Password)
				.HasColumnType("character varying")
				.HasColumnName("password");

			entity.Property(e => e.Type)
				.HasColumnName("type")
				.HasComment("0 -> admin, 1 -> patient");
		});
	}

}
