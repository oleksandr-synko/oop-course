using System;
using System.IO;
using System.Linq;

using ClinicApp.Enums;
using System.Globalization;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        using (StreamWriter writer = new StreamWriter(_sessionPath))
        {
            writer.WriteLine("[PATIENTS]");

            foreach (Patient patient in clinic.Patients.GetAll())
            {
                writer.WriteLine($"{patient.FirstName},{patient.LastName},{patient.DateOfBirth:dd.MM.yyyy},{patient.BloodType},{patient.Phone}");
            }

            writer.WriteLine("[END]");
        }
    }

    public int Load(Clinic clinic)
    {
        if (!Exists())
        {
            return 0;
        }

        int loadedCount = 0;

        using (StreamReader reader = new StreamReader(_sessionPath))
        {
            string? line;
            bool insidePatients = false;

            while ((line = reader.ReadLine()) != null)
            {
                if (line == "[PATIENTS]")
                {
                    insidePatients = true;
                    continue;
                }

                if (line == "[END]")
                {
                    break;
                }

                if (!insidePatients)
                {
                    continue;
                }

                string[] fields = line.Split(',');

                if (fields.Length != 5)
                {
                    continue;
                }

                try
                {
                    string firstName = fields[0].Trim();
                    string lastName = fields[1].Trim();

                DateTime dateOfBirth = DateTime.ParseExact(
                    fields[2].Trim(),
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture
                );

                bool alreadyExists = clinic.Patients.GetAll().Any(p =>
                    p.FirstName == firstName &&
                    p.LastName == lastName &&
                    p.DateOfBirth == dateOfBirth
                );

                if (alreadyExists)
                {
                    continue;
                }

                BloodType bloodType = (BloodType)Enum.Parse(
                    typeof(BloodType),
                    fields[3].Trim(),
                    true
                );

                string phone = fields[4].Trim();
                Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);

                clinic.Patients.Add(patient);
                loadedCount++;
                }
                catch
                {
                    continue;
                }
            }
        }
        return loadedCount;
    }
}