namespace ClinicApp.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredAttribute : Attribute
{
    public string ErrorMessage { get; }

    public RequiredAttribute(string errorMessage = "Field is required.")
    {
        ErrorMessage = errorMessage;
    }

    public bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is string text && string.IsNullOrWhiteSpace(text))
            return false;

        return true;
    }
}