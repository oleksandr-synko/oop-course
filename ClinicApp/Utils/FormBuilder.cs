namespace ClinicApp.Utils;
using System.Reflection;
using ClinicApp.Attributes;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            if (prop.Name == "Id" ||
                prop.Name == "Status" ||
                prop.GetSetMethod() == null)
            {
                continue;
            }

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();

            string hint = "";

            if (required != null)
                hint += "обов'язкове";

            if (maxLength != null)
                hint += $", макс. {maxLength.Length}";

            if (minValue != null)
                hint += $", мін. {minValue.Min}";

            Console.Write($"{prop.Name}");

            if (!string.IsNullOrEmpty(hint))
                Console.Write($" ({hint.TrimStart(',', ' ')})");

            Console.Write(": ");
            while (true)
            {
                string? input = Console.ReadLine();

                try
                {
                    object? converted = Convert.ChangeType(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                    Console.Write($"{prop.Name}");

                    if (!string.IsNullOrEmpty(hint))
                        Console.Write($" ({hint.TrimStart(',', ' ')})");

                    Console.Write(": ");
                }
            }
        }
        return obj;
    }
}