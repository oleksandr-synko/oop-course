using System;

namespace ClinicApp;

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

        if(_count >= MaxAppointments) return false;

        Appointment appointment = new Appointment(
        patientId,
        doctorId,
        scheduledAt,
        durationMinutes
        );

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
                return _appointments[i].Cancle(reason);
            }
        }
        return false;   
    }

    public bool Complete(int id)
    {
        for(int i = 0; i < _count; i++)
        {
            if(_appointments[i].Id == id)
            {
                return _appointments[i].Complete();
            }
        }
        return false;
    }

    public Appointment[] GetByPetient(int patientId)
    {
        int number = 0;
        for(int i = 0; i < _count; i++)
        {
            if(_appointments[i].PatientId == patientId)
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

    public Appointment[] GetByDoctor(int doctorId)
    {
        int number = 0;
        for(int i = 0; i < _count; i++)
        {
            if(_appointments[i].DoctorId == doctorId)
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
        for(int i = 0; i < _count; i++)
        {
            if(_appointments[i].ScheduledAt.Date == data.Date)
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

    public Appointment[] GetUpcoming()
    {
        int number = 0;
        for(int i = 0; i < _count; i++)
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
        Console.WriteLine($"[{appointment.Id}] {namePatient} => {nameDoctor} | " +
        $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}-" +
        $"{appointment.ScheduledAt.AddMinutes(appointment.DurationMinutes):HH:mm} | " +
        $"{appointment.Status}"
        );
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
}