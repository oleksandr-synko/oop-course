using System;
using System.Runtime.CompilerServices;

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
                "A+", "0501234567");

            Patient p2 = new Patient(
                "Олена", "Коваль",
                new DateTime(1993, 3, 15),
                "B-", "0672345678");

            Patient p3 = new Patient(
                "Максим", "Бойко",
                new DateTime(2010, 8, 20),
                "O+", "0933456789");

            Patient p4 = new Patient();
            Patient p5 = new Patient("Марія", "Ткач");

            clinic.Patients.Add(p1);
            clinic.Patients.Add(p2);
            clinic.Patients.Add(p3);
            clinic.Patients.Add(p4);
            clinic.Patients.Add(p5);

            Doctor d1 = new Doctor(
                "Олег", "Сидоренко",
                "Кардіологія", "001", "0441234567");

            Doctor d2 = new Doctor(
                "Наталя", "Мороз",
                "Неврологія", "002", "0442345678");

            Doctor d3 = new Doctor(
                "Андрій", "Власенко",
                "Педіатрія", "003", "0443456789");

            d1.WorkStartHour = 8;
            d1.WorkEndHour = 16;

            clinic.Doctors.Add(d1);
            clinic.Doctors.Add(d2);
            clinic.Doctors.Add(d3);

            clinic.Appointments.Book(
                1, 1, new DateTime(2026, 11, 9, 10, 0, 0));

            clinic.Appointments.Book(
                2, 2, new DateTime(2026, 11, 9, 11, 0, 0), 45);

            clinic.Appointments.Book(
                3, 3, new DateTime(2026, 11, 10, 9, 0, 0), 20);

            clinic.DisplaySchedule(new DateTime(2026, 11, 9));
            clinic.GenerateReport(5, 3);
        }
    }
}
