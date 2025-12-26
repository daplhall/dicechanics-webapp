using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
using PythonSerivce.Models;

namespace PythonSerivce.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PythonController : ControllerBase
{

    [HttpPost]
    public PythonProgram Get([FromBody] PythonProgram program)
    {
        /*
        using (Py.GIL()) {
            PythonEngine.Exec(program.Program);
        }
        */
        Console.WriteLine(program.Program);
        return program;
    }
}
