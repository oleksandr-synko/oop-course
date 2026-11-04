using System;
using System.Globalization;
using System.Threading;
using System.Linq;

using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Clinic clinic = new Clinic("Медична Клініка");
            try
            {
                Patient invalidPatient = new Patient("", "Тест");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка створення пацієнта: {ex.Message}");
            }
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

            clinic.MedicalRecords.Add(new Diagnosis(
                1,
                1,
                DateTime.Today,
                "I10", "Есенціальна гіпертензія", "Контроль артеріального тиску",
                true
            ));

            clinic.MedicalRecords.Add(new Diagnosis(
                1,
                1,
                DateTime.Today,
                "J06.9", "Гостра інфекція верхніх дихальних шляхів", "Симптоматичне лікування",
                false
            ));

            clinic.MedicalRecords.Add(new LabResult(
                1,
                1,
                DateTime.Today,
                "Гемоглобін", 145, "г/л", "120–160",
                true
            ));

            clinic.MedicalRecords.Add(new LabResult(
                1,
                1,
                DateTime.Today,
                "Холестерин загальний", 6.5, "ммоль/л", "3.0–5.2",
                false
            ));

            clinic.MedicalRecords.Add(new Prescription(
                1,
                1,
                DateTime.Today,
                "Лізиноприл", "10 мг", "Приймати 1 раз на день",
                DateTime.Today.AddDays(-5), DateTime.Today.AddDays(25)
            ));

            clinic.MedicalRecords.Add(new Prescription(
                2,
                2,
                DateTime.Today.AddDays(-30),
                "Амоксицилін", "500 мг", "Приймати 3 рази на день",
                DateTime.Today.AddDays(-30), DateTime.Today.AddDays(-20)
            ));

            clinic.MedicalRecords.Add(new Diagnosis(
                2,
                2,
                DateTime.Today,
                "K29.7", "Гастрит неуточнений", "Дієта та спостереження",
                false
            ));

            clinic.MedicalRecords.Add(new LabResult(
                3,
                3,
                DateTime.Today,
                "Глюкоза", 5.1, "ммоль/л", "3.9–5.5",
                true
            ));

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

            clinic.Appointments.Book(
                1, 1,
                new DateTime(2026, 11, 5, 10, 0, 0));

            clinic.Appointments.Book(
                1, 1,
                new DateTime(2026, 11, 6, 10, 0, 0));

            Appointment[] appointments = clinic.Appointments.GetAll();

            Console.WriteLine($"Скасований запис #{appointments[1].Id} оплачено: {appointments[1].IsPaid}");

            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine($"Запис #{appointment.Id} | Вартість: {appointment.GetCost()} | Оплачено: {appointment.IsPaid}");
            }

            appointments[0].MarkPaid();
            Console.WriteLine($"Після оплати запису #{appointments[0].Id}: {appointments[0].IsPaid}");

            clinic.Appointments.Cancle(appointments[1].Id, "Тест скасування");
            appointments[1].MarkPaid();
            Console.WriteLine($"Скасований запис #{appointments[1].Id} оплачено: {appointments[1].IsPaid}");

            Appointment[] patientAppointments = appointments
                .Where(a => a.PatientId == 1)
                .Cast<Appointment>()
                .ToArray();

            int cancelledCount = AppointmentManager.CancelAll(
                patientAppointments,
                "Масове скасування записів");

            Console.WriteLine($"Скасовано записів пацієнта №1: {cancelledCount}");

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

            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════╗");
                Console.WriteLine("║                 МЕДИЧНА КЛІНІКА              ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1. Пацієнти        — реєстрація, пошук      ║");
                Console.WriteLine("║  2. Лікарі          — персонал, розклад      ║");
                Console.WriteLine("║  3. Записи          — прийом, скасування     ║");
                Console.WriteLine("║  4. Медична картка  — діагнози, рецепти      ║");
                Console.WriteLine("║  5. Рахунки         — оплата, борги          ║");
                Console.WriteLine("║  6. Звіт            — загальна статистика    ║");
                Console.WriteLine("║  0. Вийти                                    ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "4":
                        MedicalRecordMenu(clinic);
                        break;
                    case "5":
                        BillingMenu(clinic);
                        break;

                    case "6":
                        clinic.GenerateReport(
                            clinic.Patients.Count,
                            clinic.Doctors.GetAll().Length);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний пункт меню.");
                        break;
                }
            }
        }
        
        static void MedicalRecordMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n=== МЕДИЧНА КАРТКА ===");
                Console.WriteLine("1. Медична картка пацієнта");
                Console.WriteLine("2. Усі записи пацієнта");
                Console.WriteLine("3. Додати діагноз");
                Console.WriteLine("4. Додати результат аналізу");
                Console.WriteLine("5. Додати рецепт");
                Console.WriteLine("6. Записи лікаря");
                Console.WriteLine("7. Назад");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                    Console.Write("Введіть ID пацієнта: ");

                    if (int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        if (clinic.Patients.FindById(patientId) == null)
                        {
                            Console.WriteLine("Пацієнта з таким ID не знайдено.");
                        }
                        else
                        {
                            clinic.MedicalRecords.DisplayPatientSummary(patientId);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                    break;

                    case "2":
                    Console.Write("Введіть ID пацієнта: ");

                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Patients.FindById(id) == null)
                        {
                            Console.WriteLine("Пацієнта з таким ID не знайдено.");
                        }
                        else
                        {
                            MedicalRecord[] records =
                                clinic.MedicalRecords.GetByPatientId(id);

                            if (records.Length == 0)
                            {
                                Console.WriteLine("Медичних записів не знайдено.");
                            }
                            else
                            {
                                foreach (MedicalRecord record in records)
                                {
                                    Console.WriteLine(record);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                    break;

                    case "3":
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int diagnosisPatientId) ||
                        clinic.Patients.FindById(diagnosisPatientId) == null)
                    {
                        Console.WriteLine("Некоректний ID пацієнта.");
                        break;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int diagnosisDoctorId) ||
                        clinic.Doctors.FindById(diagnosisDoctorId) == null)
                    {
                        Console.WriteLine("Некоректний ID лікаря.");
                        break;
                    }

                    Console.Write("Код діагнозу: ");
                    string code = Console.ReadLine() ?? "";

                    Console.Write("Назва діагнозу: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Лікування: ");
                    string treatment = Console.ReadLine() ?? "";

                    Console.Write("Діагноз хронічний? (так/ні): ");
                    bool isChronic = (Console.ReadLine() ?? "").Trim().ToLower() == "так";

                    try
                    {
                        clinic.MedicalRecords.Add(new Diagnosis(
                            diagnosisPatientId,
                            diagnosisDoctorId,
                            DateTime.Today,
                            code,
                            name,
                            treatment,
                            isChronic
                        ));

                        Console.WriteLine("Діагноз успішно додано.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;

                    case "4":
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int labPatientId) ||
                        clinic.Patients.FindById(labPatientId) == null)
                    {
                        Console.WriteLine("Некоректний ID пацієнта.");
                        break;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int labDoctorId) ||
                        clinic.Doctors.FindById(labDoctorId) == null)
                    {
                        Console.WriteLine("Некоректний ID лікаря.");
                        break;
                    }

                    Console.Write("Назва аналізу: ");
                    string testName = Console.ReadLine() ?? "";

                    Console.Write("Результат: ");
                    if (!double.TryParse(Console.ReadLine(), out double value))
                    {
                        Console.WriteLine("Некоректне числове значення.");
                        break;
                    }

                    Console.Write("Одиниця вимірювання: ");
                    string unit = Console.ReadLine() ?? "";

                    Console.Write("Референтний діапазон: ");
                    string referenceRange = Console.ReadLine() ?? "";

                    Console.Write("Результат у нормі? (так/ні): ");
                    bool isNormal = (Console.ReadLine() ?? "").Trim().ToLower() == "так";

                    try
                    {
                        clinic.MedicalRecords.Add(new LabResult(
                            labPatientId,
                            labDoctorId,
                            DateTime.Today,
                            testName,
                            value,
                            unit,
                            referenceRange,
                            isNormal
                        ));

                        Console.WriteLine("Результат аналізу успішно додано.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;
                    
                    case "5":
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int prescriptionPatientId) ||
                        clinic.Patients.FindById(prescriptionPatientId) == null)
                    {
                        Console.WriteLine("Некоректний ID пацієнта.");
                        break;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int prescriptionDoctorId) ||
                        clinic.Doctors.FindById(prescriptionDoctorId) == null)
                    {
                        Console.WriteLine("Некоректний ID лікаря.");
                        break;
                    }

                    Console.Write("Назва препарату: ");
                    string medicationName = Console.ReadLine() ?? "";

                    Console.Write("Дозування: ");
                    string dosage = Console.ReadLine() ?? "";

                    Console.Write("Інструкції: ");
                    string instructions = Console.ReadLine() ?? "";

                    Console.Write("Дата початку (дд.мм.рррр): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Некоректна дата початку.");
                        break;
                    }

                    Console.Write("Дата завершення (дд.мм.рррр): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        Console.WriteLine("Некоректна дата завершення.");
                        break;
                    }

                    try
                    {
                        clinic.MedicalRecords.Add(new Prescription(
                            prescriptionPatientId,
                            prescriptionDoctorId,
                            DateTime.Today,
                            medicationName,
                            dosage,
                            instructions,
                            startDate,
                            endDate
                        ));

                        Console.WriteLine("Рецепт успішно додано.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;

                    case "6":
                    Console.Write("Введіть ID лікаря: ");

                    if (!int.TryParse(Console.ReadLine(), out int doctorId) ||
                        clinic.Doctors.FindById(doctorId) == null)
                    {
                        Console.WriteLine("Лікаря з таким ID не знайдено.");
                        break;
                    }

                    MedicalRecord[] doctorRecords =
                        clinic.MedicalRecords.GetByDoctorId(doctorId);

                    if (doctorRecords.Length == 0)
                    {
                        Console.WriteLine("У цього лікаря немає медичних записів.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== МЕДИЧНІ ЗАПИСИ ЛІКАРЯ ===");

                        foreach (MedicalRecord record in doctorRecords)
                        {
                            Console.WriteLine(record);
                        }
                    }
                    break;

                    case "7":
                        return;
                    default:
                        Console.WriteLine("Цей пункт поки не реалізовано.");
                        break;
                }                
            }
        }
        static void BillingMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n— Рахунки —");
                Console.WriteLine("1. Борги пацієнта");
                Console.WriteLine("2. Всі неоплачені записи");
                Console.WriteLine("3. Оплатити запис");
                Console.WriteLine("4. Загальний борг клініки");
                Console.WriteLine("0. Назад");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                    Console.Write("ID пацієнта: ");

                    if (!int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        Console.WriteLine("Некоректний ID.");
                        break;
                    }

                    IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);

                    if (unpaid.Length == 0)
                    {
                        Console.WriteLine("Немає неоплачених записів.");
                    }
                    else
                    {
                        Console.WriteLine($"Неоплачені записи пацієнта №{patientId}:");
                        clinic.Billing.DisplayUnpaid(unpaid);
                    }

                    Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
                    break;


                    case "2":
                    IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();

                    if (allUnpaid.Length == 0)
                    {
                        Console.WriteLine("Немає неоплачених записів.");
                    }
                    else
                    {
                        Console.WriteLine("Усі неоплачені записи:");
                        clinic.Billing.DisplayUnpaid(allUnpaid);
                    }
                    break;

                    case "3":
                    Console.Write("ID запису для оплати: ");

                    if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                    {
                        Console.WriteLine("Некоректний ID.");
                        break;
                    }

                    if (clinic.Billing.PayAppointment(appointmentId))
                    {
                        Console.WriteLine($"Запис [{appointmentId}] оплачено.");
                    }
                    else
                    {
                        Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                    }
                    break;

                    case "4":
                    decimal totalDebt = clinic.Billing.GetTotalDebt();
                    Console.WriteLine($"Загальний борг по клініці: {totalDebt:F2} грн");
                    break;

                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний пункт меню.");
                        break;
                }
            }
        }
    }
}