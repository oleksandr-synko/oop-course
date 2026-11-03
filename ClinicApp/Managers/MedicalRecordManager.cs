using System.Linq;

using ClinicApp.Models;
namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private MedicalRecord[] _records = new MedicalRecord[10];
    private int _count = 0;
    public int Count => _count;
    public int Capacity => _records.Length;

    public void Add(MedicalRecord record)
    {
        if (_count >= _records.Length)
        {
            Array.Resize(ref _records, _records.Length * 2);
        }

        _records[_count] = record;
        _count++;
    }

    public MedicalRecord[] GetAll()
    {
        MedicalRecord[] result = new MedicalRecord[_count];
        Array.Copy(_records, result, _count);
        return result;
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }

        return null;
    }

    public MedicalRecord[] GetByPatientId(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .ToArray();
    }

    public MedicalRecord[] GetByDoctorId(int doctorId)
    {
        return GetAll()
            .Where(r => r.DoctorId == doctorId)
            .ToArray();
    }

    public MedicalRecord[] GetByDateRange(DateTime start, DateTime end)
    {
        return GetAll()
            .Where(r => r.Date >= start && r.Date <= end)
            .ToArray();
    }

    public Diagnosis[] GetDiagnoses()
    {
        return GetAll()
            .OfType<Diagnosis>()
            .ToArray();
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .OfType<Diagnosis>()
            .ToArray();
    }

    public LabResult[] GetLabResults(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .OfType<LabResult>()
            .ToArray();
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .OfType<Prescription>()
            .ToArray();
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .OfType<Diagnosis>()
            .Where(d => d.IsChronic)
            .ToArray();
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        return GetAll()
            .Where(r => r.PatientId == patientId)
            .OfType<Prescription>()
            .Where(p => p.IsActive)
            .ToArray();
    }

    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] records = GetAll()
            .Where(r => r.PatientId == patientId)
            .ToArray();

        if (records.Length == 0)
        {
            Console.WriteLine($"Пацієнт #{patientId} не має медичних записів.");
            return;
        }
        int diagnosisCount = records.OfType<Diagnosis>().Count();
        int labResultCount = records.OfType<LabResult>().Count();
        int prescriptionCount = records.OfType<Prescription>().Count();

        Console.WriteLine($"\n=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine(
            $"Всього записів: {records.Length} (діагнозів: {diagnosisCount}, аналізів: {labResultCount}, рецептів: {prescriptionCount})");
        foreach (MedicalRecord record in records)
        {
            Console.WriteLine(record);
        }
    }

    public LabResult[] GetLabResults()
    {
        return GetAll()
            .OfType<LabResult>()
            .ToArray();
    }

    public Prescription[] GetPrescriptions()
    {
        return GetAll()
            .OfType<Prescription>()
            .ToArray();
    }

    public int GetTotalCount()
    {
        return GetAll().Length;
    }
}