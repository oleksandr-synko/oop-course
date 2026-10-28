using System;

namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; private set; }
    public string Notes { get; private set; }


    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = "Scheduled";
        Notes = "";
    }

    public bool IsUpcoming
    {
        get
        {
            if( ScheduledAt > DateTime.Now && Status == "Scheduled")
            {
                return true;
            }
            return false;
        }
    }

    public bool Cancle(string reason)
    {
        if( Status == "Scheduled")
        {
            Status = "Cancelled";
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
        if ( Status == "Scheduled")
        {
            Status = "Complete";
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
       
       string result = $"[{Id}] Пацієнт #{PatientId} → Лікар #{DoctorId} | " +
                    $"{ScheduledAt:dd.MM.yyyy HH:mm}–{endTime:HH:mm} | {Status}";

        if (Notes != "")
        {
            result += $" | {Notes}";
        }

        return result;
    }
}