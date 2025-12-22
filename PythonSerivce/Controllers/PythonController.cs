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
    public PythonController() { }

    [HttpGet]
    public PythonProgram Get([FromQuery] string program)
    {
        using (Py.GIL())
        {
            PythonEngine.Exec(program);
        }
        return new PythonProgram() { Program = program };
    }
}
