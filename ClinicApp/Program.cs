
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

            PatientManager manager = new PatientManager();
            manager.Add(p1);
            manager.Add(p2);
            manager.Add(p3);
            manager.Add(p4);
            manager.Add(p5);
            manager.DisplayAll();
            manager.DisplayStatus();
        }
    }
}
