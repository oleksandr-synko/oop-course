using System;
using System.IO;
using System.Text;

using ClinicApp;
using ClinicApp.Models;
using ClinicApp.Interfaces;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    private string GetOutputDirectory()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string dir = Path.Combine(_baseDir, date);

        Directory.CreateDirectory(dir);

        return dir;
    }

    public string ExportPatients()
    {
        string dir = GetOutputDirectory();
        string path = Path.Combine(dir, "patients.txt");

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Пацієнти — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (Patient patient in _clinic.Patients.GetAll())
                {
                    writer.WriteLine($"ID: {patient.Id}");
                    writer.WriteLine($"ПІБ: {patient.FullName}");
                    writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy}");
                    writer.WriteLine($"Група крові: {patient.BloodType}");
                    writer.WriteLine($"Телефон: {patient.Phone}");
                    writer.WriteLine("----------------------------------------");
                }

            return path;
        }
    }

    public string ExportAppointments()
    {
        string dir = GetOutputDirectory();
        string path = Path.Combine(dir, "appointments.txt");

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Записи на прийом — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (Appointment appointment in _clinic.Appointments.GetAll())
                {
                    writer.WriteLine($"ID запису: {appointment.Id}");
                    writer.WriteLine($"ID пацієнта: {appointment.PatientId}");
                    writer.WriteLine($"ID лікаря: {appointment.DoctorId}");
                    writer.WriteLine($"Дата прийому: {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
                    writer.WriteLine($"Тривалість: {appointment.DurationMinutes} хв");
                    writer.WriteLine($"Статус: {appointment.Status}");
                    writer.WriteLine($"Примітки: {appointment.Notes}");
                    writer.WriteLine("----------------------------------------");
                }
        }

        return path;
    }

    public string ExportBilling()
    {
        string dir = GetOutputDirectory();
        string path = Path.Combine(dir, "billing.txt");

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Фінансовий звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Загальна сума боргу: {_clinic.Billing.GetTotalDebt():F2} грн");
            writer.WriteLine("----------------------------------------");

            foreach (IPayable item in _clinic.Billing.GetAllUnpaid())
                {
                    if (item is Appointment appointment)
                        {
                            writer.WriteLine($"ID запису: {appointment.Id}");
                            writer.WriteLine($"ID пацієнта: {appointment.PatientId}");
                            writer.WriteLine($"ID лікаря: {appointment.DoctorId}");
                            writer.WriteLine($"Дата прийому: {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
                            writer.WriteLine($"Сума до оплати: {item.GetCost():F2} грн");
                            writer.WriteLine("----------------------------------------");
                        }
                }
        }

        return path;
    }

    public string ExportTreatmentPlans()
    {
        string dir = GetOutputDirectory();
        string path = Path.Combine(dir, "treatment_plans.txt");

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("=== Плани лікування — звіт ===");
                writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine("----------------------------------------");

                foreach (TreatmentPlan plan in _clinic.TreatmentPlans.GetAll())
                    {
                        writer.WriteLine($"ID плану: {plan.Id}");
                        writer.WriteLine($"ID пацієнта: {plan.PatientId}");
                        writer.WriteLine($"Діагноз: {plan.Diagnosis}");
                        writer.WriteLine($"Тривалість лікування: {plan.DurationDays} днів");
                        writer.WriteLine($"Статус: {plan.Status}");
                        writer.WriteLine("----------------------------------------");
                    }
            }

            return path;
    }

    public string[] ExportAll()
    {
        string[] paths =
            {
                ExportPatients(),
                ExportAppointments(),
                ExportBilling(),
                ExportTreatmentPlans()
            };
            
        return paths;
    }
}