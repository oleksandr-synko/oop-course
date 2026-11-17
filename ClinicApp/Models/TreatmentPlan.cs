using ClinicApp.Enums;
using ClinicApp.Attributes;

namespace ClinicApp.Models;

public class TreatmentPlan
{
    public int Id { get; private set; }
    private static int _nextId = 0;


    [MinValue(1, "Patient ID is required.")]
    public int PatientId { get; set; }

    [Required("Diagnosis cannot be empty.")]
    [MaxLength(200, "Diagnosis must not exceed 200 characters.")]
    public string Diagnosis { get; set; } = "";

    [MinValue(1, "Duration must be at least 1 day.")]
    public int DurationDays { get; set; }

    public TreatmentStatus Status { get; private set; } = TreatmentStatus.Planned;
    public TreatmentPlan()
    {
        Id = ++_nextId;
    }

    public bool Activate()
    {
        if (Status != TreatmentStatus.Planned)
            return false;

        Status = TreatmentStatus.Active;
        return true;
    }

    public bool Complete()
    {
        if (Status != TreatmentStatus.Active)
            return false;

        Status = TreatmentStatus.Completed;
        return true;
    }

    public bool Cancel()
    {
        if (Status == TreatmentStatus.Completed ||
            Status == TreatmentStatus.Cancelled)
            return false;

        Status = TreatmentStatus.Cancelled;
        return true;
    }
}