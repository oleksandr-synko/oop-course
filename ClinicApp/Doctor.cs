using System;

namespace ClinicApp;

public class Doctor
{
    private static int _nextId = 1;
    
    public int Id { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }
    public int WorkStartHour { get; set; }
    public int WorkEndHour { get; set; }


    public Doctor()
    {
        Id = _nextId++;
        FirstName = "Невідомий";
        LastName = "Лікар";
        Speciality = "Не вказано";
        LicenseNumber = "Невідомо";
        Phone = "0000000000";
        WorkStartHour = 8;
        WorkEndHour = 17;
    }


    public Doctor(string firstName, string lastName, string speciality)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
    }


    public Doctor(string firstName, string lastName, string speciality, string licenseNumber, string phone)
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
        get{ return WorkEndHour - WorkStartHour; }
    }

    public string WorkSchedule
    {
        get{ return $"{WorkStartHour:D2}:00—{WorkEndHour:D2}:00"; }
    }


    public bool IsAvailableNow
    {
        get
        {
            int currentHour = DateTime.Now.Hour;
            return currentHour >= WorkStartHour && currentHour < WorkEndHour;
        }
    }


    public bool CanAcceptAt(int hour)
    {
        return hour >= WorkStartHour && hour < WorkEndHour;
        
    }


    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";

        return $"[{Id}] {FullName} | {Speciality} | LIC-{LicenseNumber} | " +
        $"Тел.{Phone} | {WorkSchedule} ({WorkingHoursPerDay}год) | {status}";
    } 
}