namespace ClinicApp.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MaxLengthAttribute : Attribute
{
    public int Length { get; }
    public string ErrorMessage { get; }

    public MaxLengthAttribute(int length, string errorMessage = "")
    {
        Length = length;
        ErrorMessage = errorMessage;
    }

    public bool IsValid(object? value)
    {
        if (value is not string text)
            return false;

        return text.Length <= Length;
    }
}