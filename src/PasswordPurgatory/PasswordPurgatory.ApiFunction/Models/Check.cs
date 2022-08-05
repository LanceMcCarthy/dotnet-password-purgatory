namespace PasswordPurgatory.ApiFunction.Models;

internal class Check
{
    public bool PasswordIsValid { get; set; }
    public string Message { get; set; }
    public InfuriationLevel InfuriationLevel { get; set; }
}