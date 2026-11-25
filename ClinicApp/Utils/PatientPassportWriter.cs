using System.Text;
using System.IO;

using ClinicApp.Events;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Managers;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _directory;

    public PatientPassportWriter(
        Clinic clinic,
        string directory = "patients")
    {
        _clinic = clinic;
        _directory = directory;

        Directory.CreateDirectory(_directory);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);

        if (patient == null)
            return;
        
        StringBuilder content = new StringBuilder();

        content.AppendLine($"=== Паспорт пацієнта #{patient.Id} ===");
        content.AppendLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        content.AppendLine();
        content.AppendLine($"Ім'я: {patient.FullName}");
        content.AppendLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({patient.Age} рік)");
        content.AppendLine($"Група крові: {patient.BloodType}   Телефон: {patient.Phone}");

        content.AppendLine();
        content.AppendLine("-- Діагнози --");
        foreach (Diagnosis diagnosis in _clinic.MedicalRecords.GetDiagnoses(patientId))
        {
            content.AppendLine($"  {diagnosis.DiagnosisCode} — {diagnosis.DiagnosisName}");

            if (diagnosis.IsChronic)
                content.AppendLine("    Тип: Хронічний");
            else
                content.AppendLine("    Тип: Гострий");

            content.AppendLine($"    Лікування: {diagnosis.Treatment}");
        }

        content.AppendLine();
        content.AppendLine("-- Результати аналізів --");
        foreach (LabResult result in _clinic.MedicalRecords.GetLabResults(patientId))
        {
            content.AppendLine($"  Аналіз: {result.TestName}");
            content.AppendLine($"    Результат: {result.Value} {result.Unit}");
            content.AppendLine($"    Референтний діапазон: {result.ReferenceRange}");
            content.AppendLine($"    Статус: {(result.IsNormal ? "Норма" : "Відхилення")}");
        }

        content.AppendLine();
        content.AppendLine("-- Рецепти --");

        foreach (Prescription prescription in _clinic.MedicalRecords.GetPrescriptions(patientId))
        {
            content.AppendLine($"  Препарат: {prescription.MedicationName}");
            content.AppendLine($"    Дозування: {prescription.Dosage}");
            content.AppendLine($"    Інструкції: {prescription.Instructions}");
            content.AppendLine($"    Період: {prescription.StartDate:dd.MM.yyyy} - {prescription.EndDate:dd.MM.yyyy}");
            content.AppendLine($"    Статус: {(prescription.IsActive ? "Активний" : "Неактивний")}");
        }

        content.AppendLine();
        content.AppendLine("-- Історія прийомів --");
        foreach (Appointment appointment in _clinic.Appointments.GetByPetient(patientId))
        {
            if (appointment.Status != AppointmentStatus.Completed)
                continue;

            content.AppendLine($"  Прийом: {appointment.GetDescription()}");
            content.AppendLine($"    Дата: {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
            content.AppendLine($"    Тривалість: {appointment.DurationMinutes} хв");
            content.AppendLine($"    Лікар: #{appointment.DoctorId}");
        }

        content.AppendLine();
        content.AppendLine("-- Плани лікування --");

        foreach (TreatmentPlan plan in _clinic.TreatmentPlans.GetByPatient(patientId))
        {
            content.AppendLine($"  План #{plan.Id}");
            content.AppendLine($"    Діагноз: {plan.Diagnosis}");
            content.AppendLine($"    Тривалість: {plan.DurationDays} днів");
            content.AppendLine($"    Статус: {plan.Status}");
        }

        string filePath = Path.Combine(_directory, $"patient_{patientId}.txt");

        File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);
    }

}