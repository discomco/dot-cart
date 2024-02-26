using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using Engine.Context;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class ChangeRpmController : ControllerBase
{
    private readonly IPipeT<ChangeRpm.IHopePipe, Contract.ChangeRpm.Payload> _pipe;

    public ChangeRpmController(IPipeBuilderT<ChangeRpm.IHopePipe, Contract.ChangeRpm.Payload> pipeBuilder)
    {
        _pipe = pipeBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Contract.ChangeRpm.Payload> hope)
    {
        var fbk = Feedback.New(hope.AggId);
        if (hope == null) return BadRequest(fbk);
        try
        {
            fbk = await _pipe.ExecuteAsync(hope);
            return Ok(fbk);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return BadRequest(fbk);
    }
}