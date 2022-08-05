namespace PasswordPurgatory.ApiFunction.Models;

internal class Check
{
    public bool PasswordIsValid;
    public string Message { get; set; }
    public InfuriationLevel InfuriationLevel { get; set; }
}