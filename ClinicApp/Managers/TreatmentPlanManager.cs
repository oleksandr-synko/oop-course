using ClinicApp.Models;
using ClinicApp.Utils;
using ClinicApp.Enums;
using ClinicApp.Events;

namespace ClinicApp.Managers;

public class TreatmentPlanManager
{
    private readonly List<TreatmentPlan> _plans = new();
    public event EventHandler<TreatmentPlanEventArgs>? TreatmentPlanCreated;
    public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

    public bool Add(TreatmentPlan plan)
    {
        ValidationResult result = ModelValidator.Validate(plan);

        if (!result.IsValid)
        {
            result.Print();
            return false;
        }

        _plans.Add(plan);

        TreatmentPlanCreated?.Invoke(this, new TreatmentPlanEventArgs(
            plan.Id,
            plan.PatientId,
            plan.Diagnosis
        ));
        return true;
    }

    public TreatmentPlan? GetById(int id)
    {
        foreach (TreatmentPlan plan in _plans)
        {
            if (plan.Id == id)
                return plan;
        }

        return null;
    }

    public bool Complete(int planId)
    {
        TreatmentPlan? plan = GetById(planId);

        if (plan == null)
            return false;

        if (!plan.Complete())
            return false;

        PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(
            plan.Id,
            plan.PatientId,
            plan.Diagnosis
        ));

        return true;
    }

    public TreatmentPlan[] GetByPatient(int patientId)
    {
        List<TreatmentPlan> result = new();

        foreach (TreatmentPlan plan in _plans)
        {
            if (plan.PatientId == patientId)
                result.Add(plan);
        }

        return result.ToArray();
    }

    public TreatmentPlan[] GetByStatus(TreatmentStatus status)
    {
        List<TreatmentPlan> result = new();

        foreach (TreatmentPlan plan in _plans)
        {
            if (plan.Status == status)
                result.Add(plan);
        }

        return result.ToArray();
    }

    public TreatmentPlan[] GetAll()
    {
        return _plans.ToArray();
    }
}