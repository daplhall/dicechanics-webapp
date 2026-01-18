namespace PythonSerivce.Models;

public enum PyReturnType
{
        result,
        error
}
public interface IPythonReturnValue
{
        public string Msg { get; set; }
        public PyReturnType Type { get; set; }
}

public class PythonReturnResult(string Message) : IPythonReturnValue
{
        public string Msg { get; set; } = Message;
        public PyReturnType Type { get; set; } = PyReturnType.result;
}


public class PythonReturnError(string Message) : IPythonReturnValue
{
        public string Msg { get; set; } = Message;
        public PyReturnType Type { get; set; } = PyReturnType.error;
}