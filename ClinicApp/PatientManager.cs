using System;

namespace ClinicApp;
public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count;

    public int Count
    {
        get {return _count;}
    }

    public void Add(Patient patient)
    {
        if(_count >= MaxPatients)
        {
            Console.WriteLine("Увага! Клініка заповнена пацієнтами!");
            return;
        }
        _patients[_count] = patient;
        _count++;

    }

    public Patient? FindById(int id)
    {
        for(int i = 0; i < _count; i++)
        {
            if(_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string name)
        {
            int number = 0;
            for(int i = 0; i <_count; i++)
            {
                if(_patients[i].FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                _patients[i].LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    number++;
                }
            }
            Patient[] results = new Patient[number];
            int ind = 0;

            for(int i = 0; i < _count; i++)
            {
                if(_patients[i].FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                _patients[i].LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    results[ind] = _patients[i];
                    ind++;
                }
            }
            return results;
        }


        public bool Remove(int id)
    {
        int ind = -1;
        for(int i = 0; i < _count; i++)
        {
            if(_patients[i].Id == id)
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
            _patients[i] = _patients[i + 1];
        }
        _patients[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if(_count == 0)
        {
            Console.WriteLine("Пацієнтів у клініці немає!");
            return;
        }
        else
        {
            Console.WriteLine($"=== Пацієнти ({_count}/{MaxPatients})===");
            for(int i = 0; i < _count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }
    }

    public void DisplayStatus()
    {
        if(_count == 0)
        {
            Console.WriteLine("Пацієнтів у клініці немає!");
            return;
        }
        int sumAge = 0;
        for(int i = 0; i <_count; i++)
        {
            sumAge += _patients[i].Age;
        }
        double meanAge = sumAge / _count;

        int youngIndex = 0;
        int oldIndex = 0;
        int adults = 0;
        
        for(int i = 0; i < _count; i++)
        {
            if(_patients[i].Age < _patients[youngIndex].Age)
            {
                youngIndex = i;
            }else if(_patients[i].Age > _patients[oldIndex].Age)
            {
                oldIndex = i;
            }
            
            if(_patients[i].Age >= 18)
            {
                adults++;
            }
        }
        Console.WriteLine($"Всього: {_count}");
        Console.WriteLine($"Середній вік: {meanAge:F1}р.");
        Console.WriteLine($"Наймолодший: {_patients[youngIndex].FullName} ({_patients[youngIndex].Age}р.)");
        Console.WriteLine($"Найстарший: {_patients[oldIndex].FullName} ({_patients[oldIndex].Age}р.)");
        Console.WriteLine($"Дорослих: {adults} з {_count}");
        Console.WriteLine("==============================");
    }
}