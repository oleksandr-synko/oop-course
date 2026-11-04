using System;

using ClinicApp.Interfaces;
using ClinicApp.Utils;
using ClinicApp.Enums;
namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private bool _isPaid = false;
    public bool IsPaid => _isPaid;
    //множник знижки зберігається у Appointment тому вона працює для всіх
    public decimal DiscountMultiplier { get; set; } = 1m;
    public void MarkPaid()
    {
        if(!IsCancelled)
        {
            _isPaid = true;
        }
    }
    public virtual decimal GetCost()
    {
        return DurationMinutes * 10m * DiscountMultiplier;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    public int GetPriority()
    {
        return 3;
    }

    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }
    private int _durationMinutes;
    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));

            _durationMinutes = value;
        }
    }

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;
    public string CancellationReason =>
        IsCancelled ? Notes : "";

    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }


    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
    }

    public bool IsUpcoming
    {
        get
        {
            if( ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled)
            {
                return true;
            }
            return false;
        }
    }

    public bool Cancel(string reason = "")
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Cancelled;
            Notes = reason;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Complete()
    {
        if(Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Completed;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override string ToString()
    {
        DateTime endTime = ScheduledAt.AddMinutes(DurationMinutes);

        string result = $"[{Id}] {GetDescription()} | " +
                        $"Пацієнт #{PatientId} → Лікар #{DoctorId} | " +
                        $"{ScheduledAt:dd.MM.yyyy HH:mm}–{endTime:HH:mm} | {Status}";

        if (Notes != "")
        {
            result += $" | {Notes}";
        }

        result += $" | {GetCost():F2} грн";

        return result;
    }
}