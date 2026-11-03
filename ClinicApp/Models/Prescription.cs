using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Prescription : MedicalRecord
{
    private string _medicationName = "";
    private string _dosage = "";
    private string _instructions = "";
    
    public string MedicationName
    {
        get => _medicationName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(MedicationName));
            _medicationName = value;
        }
    }

    public string Dosage
    {
        get => _dosage;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Dosage));
            _dosage = value;
        }
    }

    public string Instructions
    {
        get => _instructions;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Instructions));
            _instructions = value;
        }
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => DateTime.Today >= StartDate && DateTime.Today <= EndDate;


    public Prescription(
    int patientId,
    int doctorId,
    DateTime date,
    string medicationName,
    string dosage,
    string instructions,
    DateTime startDate,
    DateTime endDate)
    : base(patientId, doctorId, date)
    {
        MedicationName = medicationName;
        Dosage = dosage;
        Instructions = instructions;
        StartDate = startDate;
        EndDate = endDate;
    }

    public override string GetSummary()
    {
        string status = IsActive ? "Активний" : "Неактивний";

        return $"Препарат: {MedicationName} | Дозування: {Dosage} | Інструкції: {Instructions} | Період: {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy} | Статус: {status}";
    }

    public override string GetRecordType()
    {
        return "Рецепт";
    }

    public override string ToString()
    {
        return base.ToString() + $" | {GetSummary()}";
    }
}