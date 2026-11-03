using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException(
                $"{fieldName} має містити від 1 до 50 символів.",
                fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (!Regex.IsMatch(phone, @"\A[0-9]{10}\z"))
            throw new ArgumentException(
                "Номер телефону має містити рівно 10 цифр.",
                nameof(phone));
    }

    public static void ValidateEmail(string email)
    {
        if (!Regex.IsMatch(email, @"\A[^@\s]+@[^@\s]+\.[^@\s]+\z"))
            throw new ArgumentException(
                "Некоректний формат email.",
                nameof(email));
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value < new DateTime(1900, 1, 1) || value > DateTime.Today)
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Дата має бути між 1900 роком і сьогодні.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Значення має бути більшим за 0.");
    }
}