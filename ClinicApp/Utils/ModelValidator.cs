namespace ClinicApp.Utils;

using ClinicApp.Attributes;
using System.Reflection;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        Type type = obj.GetType();
        ValidationResult result = new ValidationResult();

        foreach (PropertyInfo prop in type.GetProperties())
        {
            object? value = prop.GetValue(obj);
            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();

            if (required != null && !required.IsValid(value))
            {
                result.AddError($"{prop.Name}: {required.ErrorMessage}");
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();

            if (maxLength != null && !maxLength.IsValid(value))
            {
                result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();

            if (minValue != null && !minValue.IsValid(value))
            {
                result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
            }
        }
        
        return result;
    }
}