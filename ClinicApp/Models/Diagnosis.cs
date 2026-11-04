namespace ClinicApp.Models;

public class Diagnosis : MedicalRecord
{
    private string _diagnosisCode = "";
    public string DiagnosisCode
    {
        get => _diagnosisCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Код діагнозу не може бути порожнім.");

            _diagnosisCode = value;
        }
    }
    public string DiagnosisName { get; set; } = "";
    public string Treatment { get; set; } = "";
    public DateTime? FollowUpDate { get; set; }
    public bool IsChronic { get; set; }

    public Diagnosis(
    int patientId,
    int doctorId,
    DateTime date,
    string diagnosisCode,
    string diagnosisName,
    string treatment,
    bool isChronic = false,
    DateTime? followUpDate = null)
    : base(patientId, doctorId, date)
    {
        DiagnosisCode = diagnosisCode;
        IsChronic = isChronic;
        DiagnosisName = diagnosisName;
        Treatment = treatment;
        FollowUpDate = followUpDate;
    }

    public override string GetSummary()
    {
        string type = IsChronic ? "Хронічний" : "Гострий";
        return $"Діагноз: {DiagnosisCode} — {DiagnosisName} ({type}) | Лікування: {Treatment}";
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