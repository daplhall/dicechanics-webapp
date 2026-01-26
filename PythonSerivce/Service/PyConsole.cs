using Python.Runtime;
using PythonSerivce.Models;
namespace PythonSerivce.Service;

public interface IPyConsole
{
        public IPythonReturnValue Execute(string Program);
}

public class PyConsole : IPyConsole
{

        public IPythonReturnValue Execute(string program)
        {
                IPythonReturnValue res;
                // TODO This place is ribe for exception throwing
                using (Py.GIL()) {
                        try {
                                using PyModule scope = Py.CreateScope();
                                scope.Exec(program, null);
                                PyObject output = scope.Get("output");
                                res = new PythonReturnResult(output.As<string>());
                        } catch (PythonException e) {
                                res = new PythonReturnError(new PythonError(e).ToString());
                        }
                }
                return res;
        }
}
