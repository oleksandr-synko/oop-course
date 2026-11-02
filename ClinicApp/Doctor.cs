using System;

namespace ClinicApp;

public class Doctor
{
    private static int _nextId = 1;
    
    public int Id { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Speciality Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }
    public WorkSchedule Schedule { get; set; }


    public Doctor()
    {
        Id = _nextId++;
        FirstName = "Невідомий";
        LastName = "Лікар";
        Speciality = Speciality = Speciality.Unknown;;
        LicenseNumber = "Невідомо";
        Phone = "0000000000";
        Schedule = new WorkSchedule(8, 17);
    }


    public Doctor(string firstName, string lastName, Speciality speciality)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
    }


    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    : this(firstName, lastName, speciality  )
    {
        LicenseNumber = licenseNumber;
        Phone = phone;
    }

    public string FullName
    {
        get{ return FirstName + " " + LastName; }
    }


    public int WorkingHoursPerDay
    {
        get { return Schedule.HoursPerDay; }
    }

    public string WorkSchedule
{
    get { return Schedule.Display; }
}


    public bool IsAvailableNow
    {
        get { return Schedule.IsNow; }
    }


    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }


    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";

        return $"[{Id}] {FullName} | {Speciality} | LIC-{LicenseNumber} | " +
        $"Тел.{Phone} | {WorkSchedule} ({WorkingHoursPerDay}год) | {status}";
    } 
}