using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class ChangeRpmController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<Context.ChangeRpm.Cmd, ChangeRpm.Hope> _hope2Cmd;

    public ChangeRpmController(ICmdHandler cmdHandler, Hope2Cmd<Context.ChangeRpm.Cmd, ChangeRpm.Hope> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] ChangeRpm.Hope hope)
    {
        var fbk = Feedback.New(hope.AggId);
        if (hope == null) return BadRequest(fbk);
        try
        {
            fbk = await _cmdHandler.HandleAsync(_hope2Cmd(hope));
            return Ok(fbk);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return BadRequest(fbk);
    }
}