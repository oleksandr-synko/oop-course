using System;

using ClinicApp.Utils;
using ClinicApp.Enums;
namespace ClinicApp.Models;
public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }

    private string _firstName = "";
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Ім'я має містити від 1 до 50 символів.", nameof(FirstName));

            _firstName = value;
        }
    }
    private string _lastName = "";
    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Прізвище має містити від 1 до 50 символів.", nameof(LastName));

            _lastName = value;
        }
    }
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value < new DateTime(1900, 1, 1) || value > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження має бути між 1900 роком і сьогодні.");

            _dateOfBirth = value;
        }
    }
    public BloodType BloodType { get; set; }
    private string _phone = "";
    public string Phone
    {
        get => _phone;
        set
        {
            if (value == null || value.Length != 10)
                throw new ArgumentException("Номер телефону має містити рівно 10 цифр.", nameof(Phone));

            foreach (char digit in value)
            {
                if (!char.IsDigit(digit))
                    throw new ArgumentException("Номер телефону має містити лише цифри.", nameof(Phone));
            }

            _phone = value;
        }
    }
    public string Email { get; set; }

    public string FullName
    {
        get {return FirstName + " " + LastName;}
    }

    public int Age
    {
        get
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

    public bool IsAdult
    {
        get { return Age >= 18; }
    }

    public Patient()
        : this("Невідомий", "Пацієнт")
    {
    }

    public Patient(string firstName, string lastName)
        : this(
            firstName,
            lastName,
            DateTime.Today,
            BloodType.Unknown,
            "0000000000")
    {
    }

    public Patient(
        string firstName,
        string lastName,
        DateTime dob,
        BloodType bloodType,
        string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
        Id = _nextId++;
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
            return "Дитина";

        if (Age < 60)
            return "Дорослий";

        return "Літній";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} " +
            $"| Кров: {ClinicFormatter.FormatBloodType(BloodType)} " +
            $"| Тел: {ClinicFormatter.FormatPhone(Phone)}";
    }
}