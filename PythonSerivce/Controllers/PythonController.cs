using Microsoft.AspNetCore.Mvc;
using PythonSerivce.Models;
using PythonSerivce.Service;

namespace PythonSerivce.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PythonController : ControllerBase
{

        [HttpPost]
        public ActionResult Post([FromBody] PythonProgram program)
        {
                IPythonReturnValue pval = PyConsole.Execute(program.Program);
                if (pval.Type == PyReturnType.result) {
                        return Ok(pval.Msg);
                } else {
                        return BadRequest(pval.Msg);
                }
        }
}
