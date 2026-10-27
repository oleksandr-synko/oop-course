using System;
using System.Runtime.CompilerServices;

namespace ClinicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Patient p1 = new Patient(
                "Іван", "Петренко",
                new DateTime(1985, 5, 10),
                "A+", "0501234567"
                );

            Patient p2 = new Patient(
                "Олена", "Коваль",
                new DateTime(1993, 3, 15),
                "B-", "0672345678"
                );

            Patient p3 = new Patient(
                "Максим", "Бойко",
                new DateTime(2010, 8, 20),
                "O+", "0933456789"
                );

            Patient p4 = new Patient();

            Patient p5 = new Patient(
                "Марія", "Ткач"
                );

            Console.WriteLine(p1);
            Console.WriteLine(p2);
            Console.WriteLine(p3);
            Console.WriteLine(p4);
            Console.WriteLine(p5);

            Console.WriteLine("\n");

            Doctor d1 = new Doctor(
                "Олег", "Сидоренко",
                "Кардіологія", "001",
                "0441234567"
            );
            Doctor d2 = new Doctor(
                "Наталя", "Мороз",
                "Неврологія", "002",
                "0442345678"
            );
            d1.WorkStartHour = 8;
            d1.WorkEndHour = 16;
            Doctor d3 = new Doctor(
              "Андрій", "Власенко",
              "Педіатрія", "003",
              "0443456789"  
            );

            Console.WriteLine(d1);
            Console.WriteLine(d2);
            Console.WriteLine(d3);

            Console.WriteLine("/n");

            PatientManager patientManager  = new PatientManager();
            patientManager.Add(p1);
            patientManager.Add(p2);
            patientManager.Add(p3);
            patientManager.Add(p4);
            patientManager.Add(p5);
            patientManager.DisplayAll();
            patientManager.DisplayStatus();


            DoctorManager doctorManager = new DoctorManager();
            doctorManager.Add(d1);
            doctorManager.Add(d2);
            doctorManager.Add(d3);
            doctorManager.DisplayAll();
            doctorManager.DisplayStats();

            Console.WriteLine("\n");

            Appointment a1 = new Appointment(
                1,
                1,
                new DateTime(2026, 05, 09, 10, 0, 0)
            );
            Appointment a2 = new Appointment(
                2,
                2,
                new DateTime(2026, 05, 09, 11, 0, 0),
                45
            );
            Appointment a3 = new Appointment(
                3,
                3,
                new DateTime(2026, 05, 10, 9, 0, 0),
                20
            );
            Console.WriteLine(a1);
            Console.WriteLine(a2);
            Console.WriteLine(a3);
            Console.WriteLine("\n");
            Console.WriteLine("Після Cancle та Comolete");
            a1.Cancle("Пацієнт не зміг прийти");
            a2.Complete();
            Console.WriteLine(a1);
            Console.WriteLine(a2);

            AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

            bool b1 = appointmentManager.Book(
            1, 1, new DateTime(2026, 11, 9, 10, 0, 0));

            bool b2 = appointmentManager.Book(
            2, 2, new DateTime(2026, 11, 9, 11, 0, 0), 45);

            bool b3 = appointmentManager.Book(
            3, 3, new DateTime(2026, 11, 10, 9, 0, 0), 20);

            Console.WriteLine($"Запис 1: {b1}");
            Console.WriteLine($"Запис 2: {b2}");
            Console.WriteLine($"Запис 3: {b3}");

            Console.WriteLine("Запис [1] створено: Іван Петренко → Олег Сидоренко о 09.05.2026 10:00");

            if (!appointmentManager.Book(
                99,
                1,
                new DateTime(2026, 5, 9, 12, 0, 0)))
            {
                Console.WriteLine(
                    "Помилка: пацієнта з ID 99 не знайдено.");
            }
            Console.WriteLine("\nМайбутні записи:");
            appointmentManager.DisplayList(
                appointmentManager.GetUpcoming());

            appointmentManager.Cancle(
                1, "Пацієнт не зміг прийти");

            Console.WriteLine("\nЗапис [1] скасовано.");

            Console.WriteLine("\nЗаписи пацієнта #2:");
            appointmentManager.DisplayList(
                appointmentManager.GetByPetient(2));
        }
    }
}
