using System;

namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count;

    public void Add(Doctor doctor)
    {
        if(_count >= MaxDoctors)
        {
            Console.WriteLine("Увага! Лікарня більше не може приймати лікарів!");
            return;
        }
        _doctors[_count] = doctor;
        _count++;
    }

    public Doctor? FindById(int id)
    {
        for(int i = 0; _count < i; i++)
        {
            if(_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        int number = 0;
        for(int i = 0; i < _count; i++)
        {
            if(_doctors[i].Speciality.Contains(speciality, StringComparison.OrdinalIgnoreCase))
            {
                number++;
            }
        }
        Doctor[] results = new Doctor[number];
            int ind = 0;
            
            for(int i = 0; i < _count; i++)
            {
                if(_doctors[i].Speciality.Contains(speciality, StringComparison.OrdinalIgnoreCase))
                {
                    results[ind] = _doctors[i];
                    ind++;
                }
            }
            return results;
    }

    public Doctor[] GetAll()
    {
        Doctor[] results = new Doctor[_count];

        for(int i = 0; i < _count; i++)
        {
            results[i] = _doctors[i];
        }
        return results;
    }

    public bool Remove(int id)
    {
        int ind = -1;
        for(int i = 0; i < _count; i++)
        {
            if(_doctors[i].Id == id)
            {
                ind = i;
                break;
            }
        }
        if(ind == -1)
        {
            return false;
        }
        for(int i = ind; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }
        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if(_count == 0)
        {
                Console.WriteLine("Лікарів у клініці немає!");
                return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");

        for(int i = 0; i < _count; i++)
        {
            Console.WriteLine($"{_doctors[i]} | {(_doctors[i].IsAvailableNow ? "Доступний зараз" : "Не робочий час")}");
        }
        Console.WriteLine("==============================");
    }

    public void DisplayStats()
{
    Console.WriteLine("\n=== Статистика лікарів ===");
    Console.WriteLine($"Всього: {_count}");

    int available = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_doctors[i].IsAvailableNow)
        {
            available++;
        }
    }

    Console.WriteLine($"Доступні зараз: {available}");
    Console.WriteLine("По спеціальностях:");

    for (int i = 0; i < _count; i++)
    {
        bool alreadyExists = false;

        for (int j = 0; j < i; j++)
        {
            if (string.Equals(
                _doctors[i].Speciality,
                _doctors[j].Speciality,
                StringComparison.OrdinalIgnoreCase))
            {
                alreadyExists = true;
                break;
            }
        }

        if (alreadyExists)
        {
            continue;
        }

        int specialityCount = 0;

        for (int j = 0; j < _count; j++)
        {
            if (string.Equals(
                _doctors[i].Speciality,
                _doctors[j].Speciality,
                StringComparison.OrdinalIgnoreCase))
            {
                specialityCount++;
            }
        }

        Console.WriteLine(
            $"  {_doctors[i].Speciality}: {specialityCount}");
    }

    Console.WriteLine("==============================");
}
}