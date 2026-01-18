using Python.Runtime;
namespace PythonSerivce.Models;

public class PythonError(PythonException exception)
{
        PyType Type { get; set; } = exception.Type;
        string Message { get; set; } = exception.Message;
        string StackTrace { get; set; } = exception.StackTrace;

        public override string ToString()
        {
                return $"Error: {Type.Name}{(string.IsNullOrEmpty(Message) ? "" : " - ")}{Message}";
        }
}
