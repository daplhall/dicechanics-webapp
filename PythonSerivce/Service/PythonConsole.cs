using Python.Runtime;
using PythonSerivce.Models;
namespace PythonSerivce.Service;

class PythonConsole
{
    static public PythonReturnValue Execute(string program)
    {
        PythonReturnValue res = new();
        try {
            using (Py.GIL()) {
                var locals = new PyDict();
                PythonEngine.Exec(program, null, locals);
                var test = locals.Items();
                Console.WriteLine(test);
            }
        } catch (PythonException e) {
            res.ErrorMessage = e.Message ?? "";
        }
        return res;
    }
}
