namespace PythonSerivce.Models;

public class PythonReturnValue
{
    public string Results { get; set; } = "";
    public string ErrorMessage { get; set; } = "";
    public bool ErrorOccoured()
    {
        return !string.IsNullOrEmpty(ErrorMessage);
    }
}