using System;
using System.Runtime.CompilerServices;

using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Clinic clinic = new Clinic("Медична Клініка");

            Patient p1 = new Patient(
                "Іван", "Петренко",
                new DateTime(1985, 5, 10),
                BloodType.APositive, "0501234567");

            Patient p2 = new Patient(
                "Олена", "Коваль",
                new DateTime(1993, 3, 15),
                BloodType.BNegative, "0672345678");

            Patient p3 = new Patient(
                "Максим", "Бойко",
                new DateTime(2010, 8, 20),
                BloodType.OPositive, "0933456789");

            Patient p4 = new Patient();
            Patient p5 = new Patient("Марія", "Ткач");

            clinic.Patients.Add(p1);
            clinic.Patients.Add(p2);
            clinic.Patients.Add(p3);
            clinic.Patients.Add(p4);
            clinic.Patients.Add(p5);

            Doctor d1 = new Doctor(
                "Олег", "Сидоренко",
                Speciality.Cardiologist, "001", "0441234567");

            Doctor d2 = new Doctor(
                "Наталя", "Мороз",
                Speciality.Neurologist, "002", "0442345678");

            Doctor d3 = new Doctor(
                "Андрій", "Власенко",
                Speciality.Pediatrician, "003", "0443456789");

            d1.Schedule = new WorkSchedule(8, 16);

            clinic.Doctors.Add(d1);
            clinic.Doctors.Add(d2);
            clinic.Doctors.Add(d3);

            clinic.Appointments.Book(
                1,
                1,
                new DateTime(2026, 05, 9, 10, 0, 0));

            clinic.Appointments.Book(
                2, 
                2, 
                new DateTime(2026, 05, 9, 11, 0, 0), 45);

            clinic.Appointments.Book(
                3, 
                3, 
                new DateTime(2026, 05, 10, 9, 0, 0), 20);

            clinic.DisplaySchedule(new DateTime(2026, 05, 9));
            clinic.GenerateReport(5, 3);

            Console.WriteLine("=== Тест GrowablePatientManager ===");
            Console.WriteLine("Додаємо пацієнтів одного за одним...");
            GrowablePatientManager manager = new GrowablePatientManager();

            for(int i = 1; i <= 20; i++)
            {
                Patient patient = new Patient(
                "Тест", $"Пацієнт{i}",
                new DateTime(2000, 1, 1),
                BloodType.APositive, "0500000000");
                manager.Add(patient);
                if(i <= 9 || i == 20)
                {
                    Console.WriteLine($"Додано [{i}]. Розмір: {manager.Count} / {manager.Capacity}");
                }
                else if(i == 10)
                {
                    Console.WriteLine("...");
                }
            }

            Console.WriteLine("\nТест пошук:");
            Patient? found = manager.FindById(10);
            Console.WriteLine(found != null ? $"FindById(10) => {found.FullName}"
            : "FindById(10) => не знайдено");

            Patient? missing = manager.FindById(99);
            Console.WriteLine(missing != null ? $"FindById(99) => {missing.FullName}"
            : "FindById(99) => не знайдено");
            
            Console.WriteLine("\nПорівняння:");
            Console.WriteLine("  PatientManager          100 місць (фіксовано)");
            Console.WriteLine($"  GrowablePatientManager  {manager.Capacity} місця (зросте при потребі)");

            Console.WriteLine("\n=== Тест TryFindById ===");

            if (clinic.Patients.TryFindById(1, out Patient? foundPatient))
            {
                Console.WriteLine($"Пацієнта знайдено: {foundPatient.FullName}");
            }
            else
            {
                Console.WriteLine("Пацієнта не знайдено");
            }

            if (clinic.Doctors.TryFindById(1, out Doctor? doctor))
            {
                Console.WriteLine($"Лікаря знайдено: {doctor.FullName}");
            }
            else
            {
                Console.WriteLine("Лікаря не знайдено");
            }
        }
    }
}
