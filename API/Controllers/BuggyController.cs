using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorizedRequest()
    {
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequestRequest()
    {
        return BadRequest(new ProblemDetails { Title = "This is a bad request" });
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        return NotFound();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        return StatusCode(500, "Server error");
    }

    [HttpGet("internalerror")]
    public ActionResult GetInternalError()
    {
        throw new Exception("This is a test exception");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError([FromBody] CreateProductDto product)
    {
        return Ok();
    }
}
