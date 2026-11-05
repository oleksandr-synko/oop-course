using System;

using ClinicApp.Utils;
using ClinicApp.Enums;
namespace ClinicApp.Models;
using ClinicApp.Interfaces;

public class Doctor : ISchedulable, IIdentifiable
{
    private static int _nextId = 1;
    
    public int Id { get; }

    private string _firstName = "";
    public string FirstName
    {
        get => _firstName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));

            _firstName = value;
        }
    }
    private string _lastName = "";
    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));

            _lastName = value;
        }
    }
    public Speciality Speciality { get; set; }
    private string _licenseNumber = "";
    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));

            _licenseNumber = value;
        }
    }
    private string _phone = "";
    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);

            _phone = value;
        }
    }
    public WorkSchedule Schedule { get; set; }


    public Doctor()
    {
        FirstName = "Невідомий";
        LastName = "Лікар";
        Speciality = Speciality.Unknown;;
        LicenseNumber = "Невідомо";
        Phone = "0000000000";
        Schedule = new WorkSchedule(8, 17);
        Id = _nextId++;
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

    public bool CanSchedule(DateTime at)
    {
        return CanAcceptAt(at.Hour);
    }

    public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
    {
        if (slotCount < 0) ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

        int hours = Schedule.HoursPerDay;
        int count = Math.Min(slotCount, hours);

        DateTime[] slots = new DateTime[count];

        for (int i = 0; i < count; i++)
        {
            slots[i] = date.Date.AddHours(Schedule.Start + i);
        }

        return slots;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";

        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | LIC-{LicenseNumber} | " +
            $"Тел.{ClinicFormatter.FormatPhone(Phone)} | {WorkSchedule} ({WorkingHoursPerDay}год) | {status}";
    }
}