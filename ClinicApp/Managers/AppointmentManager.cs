using System;

using ClinicApp.Interfaces;
using ClinicApp.Models;
namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 50;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count;
    private PatientManager _patients;
    private DoctorManager _doctors;

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Patient? patient = _patients.FindById(patientId);
        Doctor? doctor = _doctors.FindById(doctorId);
        if(patient == null || doctor == null)
        {
            return false;
        }

        Appointment appointment = new RegularAppointment(
        patientId,
        doctorId,
        scheduledAt,
        durationMinutes
        );

        return AddAppointment(appointment);
    }

    public bool BookUrgent(
    int patientId,
    int doctorId,
    DateTime scheduledAt,
    string urgencyNote = "",
    int durationMinutes = 30)
    {
        Patient? patient = _patients.FindById(patientId);
        Doctor? doctor = _doctors.FindById(doctorId);

        if (patient == null || doctor == null)
        {
            return false;
        }

        Appointment appointment = new UrgentAppointment(
            patientId,
            doctorId,
            scheduledAt,
            urgencyNote,
            durationMinutes
        );

        return AddAppointment(appointment);
    }

    public bool BookSpecialist(
    int patientId,
    int doctorId,
    DateTime scheduledAt,
    string topic = "",
    int durationMinutes = 45)
    {
        Patient? patient = _patients.FindById(patientId);
        Doctor? doctor = _doctors.FindById(doctorId);

        if (patient == null || doctor == null)
        {
            return false;
        }

        Appointment appointment = new SpecialistAppointment(
            patientId,
            doctorId,
            scheduledAt,
            topic,
            durationMinutes
        );

        return AddAppointment(appointment);
    }

    private bool AddAppointment(Appointment appointment)
    {
        if (_count >= MaxAppointments)
        {
            return false;
        }

        _appointments[_count] = appointment;
        _count++;

        return true;
    }

    public bool Cancle(int id, string reason)
    {
        for(int i = 0; i < _count; i++)
        {
            if(_appointments[i].Id == id)
            {
                return _appointments[i].Cancel(reason);
            }
        }
        return false;   
    }

    public bool Complete(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i].Complete();
            }
        }
        return false;
    }

    public Appointment[] GetAll()
    {
        Appointment[] result = new Appointment[_count];
        Array.Copy(_appointments, result, _count);
        return result;
    }

    public Appointment[] GetUrgent()
    {
        int count = 0;

        foreach (Appointment appointment in _appointments)
        {
            if (appointment is UrgentAppointment)
            {
                count++;
            }
        }

        Appointment[] result = new Appointment[count];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetSpecialist()
{
    int count = 0;

    foreach (Appointment appointment in _appointments)
    {
        if (appointment is SpecialistAppointment)
        {
            count++;
        }
    }

    Appointment[] result = new Appointment[count];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_appointments[i] is SpecialistAppointment)
        {
            result[index] = _appointments[i];
            index++;
        }
    }

    return result;
}

    public Appointment[] GetByPetient(int patientId)
    {
        int number = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                number++;
            }
        }

        Appointment[] results = new Appointment[number];
        int ind = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                results[ind] = _appointments[i];
                ind++;
            }
        }
        return results;
    }

    public Appointment[] GetRegular()
    {
        int count = 0;

        foreach (Appointment appointment in _appointments)
        {
            if (appointment is RegularAppointment)
            {
                count++;
            }
        }

        Appointment[] result = new Appointment[count];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }



    public Appointment[] GetByDoctor(int doctorId)
    {
        int number = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                number++;
            }
        }

        Appointment[] results = new Appointment[number];
        int ind = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                results[ind] = _appointments[i];
                ind++;
            }
        }
        return results;
    }

    public Appointment[] GetByDate(DateTime data)
    {
        int number = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == data.Date)
            {
                number++;
            }
        }

        Appointment[] results = new Appointment[number];
        int ind = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == data.Date)
            {
                results[ind] = _appointments[i];
                ind++;
            }
        }
        return results;
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        DateTime date = new DateTime(year, month, day);
        return GetByDate(date);
    }

    public Appointment[] GetUpcoming()
    {
        int number = 0;
        for (int i = 0; i < _count; i++)
        {
            if(_appointments[i].IsUpcoming == true)
            {
                number++;
            }
        }

        Appointment[] results = new Appointment[number];
        int ind = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming == true)
            {
                results[ind] = _appointments[i];
                ind++;
            }
        }
        return results;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);
        Doctor? doctor = _doctors.FindById(appointment.DoctorId);
        string nameDoctor;
        string namePatient;
        if(patient != null)
        {
            namePatient = patient.FullName;
        }
        else
        {
            namePatient = $"Пацієнт #{appointment.PatientId}";
        }

        if(doctor != null)
        {
            nameDoctor = doctor.FullName;
        }
        else
        {
                nameDoctor = $"Лікаря #{appointment.DoctorId}";
        }
        Console.WriteLine($"[{appointment.Id}] {appointment.GetDescription()} | " +
        $"{namePatient} → {nameDoctor} | " +
        $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}-{appointment.ScheduledAt.AddMinutes(appointment.DurationMinutes):HH:mm} | " +
        $"{appointment.Status} | {appointment.GetCost():F2} грн");
    }
    
    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }

    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int cancelledCount = 0;

        foreach (ICancellable item in items)
        {
            if (item.Cancel(reason))
            {
                cancelledCount++;
            }
        }

        return cancelledCount;
    }
}