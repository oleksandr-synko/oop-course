using System.IO;

using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionEventTracker
{
    private readonly Clinic _clinic;
    public int PatientsAdded { get; private set; }
    public int AppointmentsBooked { get; private set; }
    public int UrgentBooked { get; private set; }
    public int AppointmentsCancelled { get; private set; }
    public int AppointmentsCompleted { get; private set; }
    public int PaymentsReceived { get; private set; }
    public int PlansCompleted { get; private set; }

    public SessionEventTracker(Clinic clinic)
    {
        _clinic = clinic;
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        PatientsAdded++;
    }

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        AppointmentsBooked++;
    }

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        UrgentBooked++;
    } 

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCancelled++;

        if (!_clinic.WaitingRoom.IsEmpty)
        {
            Patient nextPatient = _clinic.WaitingRoom.Peek();

            Console.WriteLine(
                $"[ЧЕРГА] Слот звільнився. Наступний: {nextPatient.FullName}");
        }
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCompleted++;
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
    {
        PaymentsReceived++;
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        PlansCompleted++;
    }

    public void PrintSummary()
    {
        Console.WriteLine("\n========== ПІДСУМКИ СЕСІЇ ==========");
        Console.WriteLine($"Додано пацієнтів: {PatientsAdded}");
        Console.WriteLine($"Заплановано прийомів: {AppointmentsBooked}");
        Console.WriteLine($"Термінових прийомів: {UrgentBooked}");
        Console.WriteLine($"Скасовано прийомів: {AppointmentsCancelled}");
        Console.WriteLine($"Завершено прийомів: {AppointmentsCompleted}");
        Console.WriteLine($"Отримано оплат: {PaymentsReceived}");
        Console.WriteLine($"Завершено планів лікування: {PlansCompleted}");
        Console.WriteLine("===================================");
    }

    public void SaveSummary()
    {
        string fileName = $"session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

        string content =
            $"========== ПІДСУМКИ СЕСІЇ ==========\n" +
            $"Додано пацієнтів: {PatientsAdded}\n" +
            $"Заплановано прийомів: {AppointmentsBooked}\n" +
            $"Термінових прийомів: {UrgentBooked}\n" +
            $"Скасовано прийомів: {AppointmentsCancelled}\n" +
            $"Завершено прийомів: {AppointmentsCompleted}\n" +
            $"Отримано оплат: {PaymentsReceived}\n" +
            $"Завершено планів лікування: {PlansCompleted}\n";

        File.WriteAllText(fileName, content);
    }

    
}