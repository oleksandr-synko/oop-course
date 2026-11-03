namespace ClinicApp.Models;

public class Diagnosis : MedicalRecord
{
    public string DiagnosisName { get; set; } = "";
    public string Treatment { get; set; } = "";
    public DateTime? FollowUpDate { get; set; }

    public Diagnosis(
    int patientId,
    int doctorId,
    DateTime date,
    string diagnosisName,
    string treatment,
    DateTime? followUpDate = null)
    : base(patientId, doctorId, date)
    {
        DiagnosisName = diagnosisName;
        Treatment = treatment;
        FollowUpDate = followUpDate;
    }

    public override string GetSummary()
    {
        return $"Діагноз: {DiagnosisName} | Лікування: {Treatment}";
    }

    public override string GetRecordType()
    {
        return "Діагноз";
    }

    public override string ToString()
    {
        return base.ToString() + $" | {GetSummary()}";
    }
}