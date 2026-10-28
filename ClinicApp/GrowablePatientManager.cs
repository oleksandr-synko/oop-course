using System;

namespace ClinicApp;

public class GrowablePatientManager
{
    private Patient[] _patients = new Patient[4];
    private int _count = 0;
    public int Count => _count;
    public int Capacity => _patients.Length;

    private void IncreaseCapacity()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;
        Patient[] newPatients = new Patient[newCapacity];

        for(int i = 0; i < _count; i++)
        {
            newPatients[i] = _patients[i];
        }
        _patients = newPatients;

        Console.WriteLine($"Масив заповнений! Розширений: {oldCapacity} => {newCapacity}");
    }

    public void Add(Patient patient)
    {
        if(_count == _patients.Length)
        {
            IncreaseCapacity();
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

    public bool Remove(int id)
    {
        for(int i = 0; i < _count; i++)
        {
            if(_patients[i].Id == id)
            {
                for(int j = i; j < _count - 1; j++)
                {
                    _patients[j] = _patients[j + 1];
                }
                _count--;
                _patients[_count] = null!;

                return true;
            }
        }
        return false;
    }

    public void DisplayAll()
    {
        Console.WriteLine($"Список Пацієнтів:");
        for(int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine($"Кількість: {Count} / {Capacity}");
    }
}