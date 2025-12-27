using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
using PythonSerivce.Models;
using PythonSerivce.Service;

namespace PythonSerivce.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PythonController : ControllerBase
{

    [HttpPost]
    public ActionResult<PythonApiReturnValue> Post([FromBody] PythonProgram program)
    {
        PythonReturnValue pval = PythonConsole.Execute(program.Program);
        if (!pval.ErrorOccoured()) {
            return new PythonApiReturnValue { Results = pval.Results };
        } else {
            return BadRequest(pval.ErrorMessage);
        }
    }
}
