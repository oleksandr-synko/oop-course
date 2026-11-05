using System;
using System.Linq;

using ClinicApp.Models;
using ClinicApp.Enums;
namespace ClinicApp.Managers;
public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();
    
    public int Count
    {
        get { return _patients.Count; }
    }

    public Patient this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
                throw new IndexOutOfRangeException("Пацієнта з таким індексом не існує!");

            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if(_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, out Patient? patient)
    {
        patient = FindById(id);

        return patient != null;
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        return _patients
            .Where(p => p.BloodType == bloodType)
            .ToArray();
    }

    public Patient[] FindByName(string name)
    {
        return _patients
            .Where(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }


        public bool Remove(int id)
        {
            int index = _patients.FindIndex(p => p.Id == id);

            if (index == -1)
            {
                return false;
            }

            _patients.RemoveAt(index);
            return true;
        }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Пацієнтів у клініці немає!");
            return;
        }
        else
        {
            Console.WriteLine($"=== Пацієнти ({_patients.Count})===");
            for(int i = 0; i < _patients.Count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }
    }

    public void DisplayStatus()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Пацієнтів у клініці немає!");
            return;
        }
        int sumAge = 0;
        for(int i = 0; i < _patients.Count; i++)
        {
            sumAge += _patients[i].Age;
        }
        double meanAge = sumAge / _patients.Count;

        int youngIndex = 0;
        int oldIndex = 0;
        int adults = 0;
        
        for(int i = 0; i < _patients.Count; i++)
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
        Console.WriteLine($"Всього: {_patients.Count}");
        Console.WriteLine($"Середній вік: {meanAge:F1}р.");
        Console.WriteLine($"Наймолодший: {_patients[youngIndex].FullName} ({_patients[youngIndex].Age}р.)");
        Console.WriteLine($"Найстарший: {_patients[oldIndex].FullName} ({_patients[oldIndex].Age}р.)");
        Console.WriteLine($"Дорослих: {adults} з {_patients.Count}");
        Console.WriteLine("==============================");
    }
}