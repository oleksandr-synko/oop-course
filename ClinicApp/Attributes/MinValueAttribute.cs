namespace ClinicApp.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MinValueAttribute : Attribute
{
    public double Min { get; }
    public string ErrorMessage { get; }

    public MinValueAttribute(double min, string errorMessage = "")
    {
        Min = min;
        ErrorMessage = errorMessage;
    }

    public bool IsValid(object? value)
    {
        if (value == null)
            return false;

        try
        {
            double number = Convert.ToDouble(value);
            return number >= Min;
        }
        catch
        {
            return false;
        }
    }
}