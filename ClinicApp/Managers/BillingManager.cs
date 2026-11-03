namespace ClinicApp.Managers;

using ClinicApp.Interfaces;
using ClinicApp.Models;

public class BillingManager
{
    private readonly AppointmentManager _appointmentManager;
    public BillingManager(AppointmentManager appointmentManager)
    {
        _appointmentManager = appointmentManager;
    }

    private IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int count = 0;

        foreach (Appointment appointment in appointments)
        {
            if (!appointment.IsPaid && !appointment.IsCancelled)
            {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int index = 0;

        foreach (Appointment appointment in appointments)
        {
            if (!appointment.IsPaid && !appointment.IsCancelled)
            {
                result[index] = appointment;
                index++;
            }
        }

        return result;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointmentManager.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        Appointment[] appointments = _appointmentManager.GetByPetient(patientId);

        return FilterUnpaid(appointments);
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);

        decimal total = 0;

        foreach (IPayable item in unpaid)
        {
            total += item.GetCost();
        }

        return total;
    }

    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;

        foreach (IPayable item in unpaid)
        {
            total += item.GetCost();
        }

        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] appointments = _appointmentManager.GetAll();
        foreach (Appointment appointment in appointments)
        {
            if (appointment.Id == appointmentId)
            {
                if (!appointment.IsPaid && !appointment.IsCancelled)
                {
                    appointment.MarkPaid();
                    return appointment.IsPaid;
                }

                return false;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        int index = 1;
        foreach (IPayable item in items)
        {
            if (item is Appointment appointment)
            {
                Console.WriteLine(
                    $"Запис №{appointment.Id} | " +
                    $"Пацієнт: {appointment.PatientId} | " +
                    $"Лікар: {appointment.DoctorId} | " +
                    $"Дата: {appointment.ScheduledAt:dd.MM.yyyy HH:mm} | " +
                    $"Сума: {item.GetCost().ToString("F2")}");
            }
            else
            {
                Console.WriteLine($"Запис №{index} | Сума: {item.GetCost().ToString("F2")}");
            }
            index++;
        }
    }
}