using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class StartController : ControllerBase
{
    private readonly IPipeT<Context.Start.IHopePipe, Start.Payload> _pipe;

    public StartController(
        IPipeBuilderT<Context.Start.IHopePipe, Start.Payload> pipeBuilder)
    {
        _pipe = pipeBuilder.Build();
    }


    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Start.Payload> hope)
    {
        if (hope == null) return BadRequest("Body cannot be null.");
        var fbk = Feedback.New(hope.AggId);
        try
        {
            fbk = await _pipe.ExecuteAsync(hope);
            return Ok(fbk);
        }
        catch (Exception e)
        {
            Log.Debug(e.InnerAndOuter());
            fbk.SetError(e.AsError());
        }

        return BadRequest(fbk);
    }
}