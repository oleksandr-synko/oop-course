using System;
using System.IO;
using System.Text;
using System.Linq;

using ClinicApp.Events;
namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    private void WriteLog(string level, string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        File.AppendAllText(_logPath, logEntry + Environment.NewLine, Encoding.UTF8);
    }

    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteLog("WARN", message);
    }

    public void LogError(string message)
    {
        WriteLog("ERROR", message);
    }

    public string[] ReadLastLines(int count)
    {
        if (!File.Exists(_logPath))
            return Array.Empty<string>();

        string[] lines = File.ReadAllLines(_logPath);

        return lines.TakeLast(count).ToArray();
    }

    public void Clear()
    {
        File.WriteAllText(_logPath, string.Empty);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        LogInfo($"Додано нового пацієнта: ID {e.PatientId}, {e.FullName}");
    }

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogInfo(
            $"Створено запис №{e.AppointmentId}. " +
            $"Пацієнт: №{e.PatientId}, лікар: №{e.DoctorId}, " +
            $"дата: {e.ScheduledAt:dd.MM.yyyy HH:mm}"
        );
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        LogInfo(
            $"Скасовано запис №{e.AppointmentId}. " +
            $"Пацієнт: №{e.PatientId}, лікар: №{e.DoctorId}"
        );
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        LogInfo(
            $"Завершено прийом №{e.AppointmentId}. " +
            $"Пацієнт: №{e.PatientId}, лікар: №{e.DoctorId}"
        );
    }

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        string message =
            $"ТЕРМІНОВИЙ ЗАПИС №{e.AppointmentId}. " +
            $"Пацієнт: №{e.PatientId}, лікар: №{e.DoctorId}";

        LogWarning(message);

        string directory = "alerts";
        Directory.CreateDirectory(directory);

        string filePath = Path.Combine(
            directory,
            $"urgent_{DateTime.Now:yyyy-MM-dd}.txt"
        );

        File.AppendAllText(
            filePath,
            $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}"
        );
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
    {
        LogInfo($"Оплачено прийом №{e.AppointmentId}. Сума: {e.Amount:F2}");
    }

    public void OnTreatmentPlanCreated(object? sender, TreatmentPlanEventArgs e)
    {
        LogInfo($"Створено план лікування №{e.PlanId} для пацієнта №{e.PatientId}. Діагноз: {e.Diagnosis}");
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        LogInfo($"Завершено план лікування №{e.PlanId} для пацієнта №{e.PatientId}. Діагноз: {e.Diagnosis}");
    }
}