using System.Linq;

using ClinicApp.Enums;
namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt)
    {
        return bt switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            _ => "Невідомо"
        };
    }

    public static string FormatSpeciality(Speciality s)
    {
        return s switch
        {
            Speciality.Therapist => "Терапевт",
            Speciality.Cardiologist => "Кардіолог",
            Speciality.Neurologist => "Невролог",
            Speciality.Surgeon => "Хірург",
            Speciality.Dermatologist => "Дерматолог",
            Speciality.Ophthalmologist => "Офтальмолог",
            Speciality.Dentist => "Стоматолог",
            Speciality.Pediatrician => "Педіатр",
            _ => "Невідома спеціалізація"
        };
    }

    public static string FormatAge(int age)
    {
        int lastTwo = age % 100;

        if (lastTwo >= 11 && lastTwo <= 19)
            return $"{age} років";

        int lastDigit = age % 10;

        return lastDigit switch
        {
            1 => $"{age} рік",
            2 or 3 or 4 => $"{age} роки",
            _ => $"{age} років"
        };
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10 || !phone.All(char.IsDigit))
            return phone;

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
    }

    public static string FormatAppointmentStatus(AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Scheduled => "Заплановано",
            AppointmentStatus.Cancelled => "Скасовано",
            AppointmentStatus.Completed => "Завершено",
            _ => "Невідомий статус"
        };
    }
}