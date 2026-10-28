
using System;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }

    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;

        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");
        Appointments.DisplayList(Appointments.GetByDate(date));
    }

    public void GenerateReport(int patientCount, int doctorCount)
    {
        Appointment[] upcoming = Appointments.GetUpcoming();

        Console.WriteLine();
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine($"║  Звіт — {Name}");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Пацієнтів:              {patientCount}");
        Console.WriteLine($"║  Лікарів:                {doctorCount}");
        Console.WriteLine($"║  Майбутніх записів:      {upcoming.Length}");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.WriteLine("║  Навантаження лікарів (майбутні записи):");

        for (int id = 1; id <= doctorCount; id++)
        {
            Doctor? doctor = Doctors.FindById(id);

            if (doctor == null)
                continue;

            int count = 0;

            foreach (Appointment appointment in upcoming)
            {
                if (appointment.DoctorId == doctor.Id)
                    count++;
            }

            Console.WriteLine(
                $"║    {doctor.FullName} ({doctor.Speciality}): {count} записів");
        }

        Console.WriteLine("╚════════════════════════════════════════════════════╝");
    }
}
