using System;
using System.IO;
using System.Globalization;

using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        if (!File.Exists(filePath))
        {
            ImportResult errorResult = new ImportResult();
            errorResult.AddError(0, "Файл не знайдено.");
            return errorResult;
        }
        ImportResult result = new ImportResult();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string? header = reader.ReadLine();
            int lineNumber = 1;

            while ((reader.ReadLine()) is string line)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                string[] fields = line.Split(',');

                if (fields.Length != 5)
                {
                    result.AddError(lineNumber, "Неправильна кількість полів.");
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

                    BloodType bloodType = (BloodType)Enum.Parse(
                        typeof(BloodType),
                        fields[3].Trim(),
                        true
                    );

                    string phone = fields[4].Trim();

                    Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);
                    clinic.Patients.Add(patient);
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError(lineNumber, ex.Message);
                }
            }
        }

        return result;
    }
}