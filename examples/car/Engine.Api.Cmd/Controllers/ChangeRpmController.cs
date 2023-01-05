using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class ChangeRpmController : ControllerBase
{
    private readonly ISequenceT<ChangeRpm.Payload> _sequence;

    public ChangeRpmController(ISequenceBuilderT<ChangeRpm.Payload> sequenceBuilder)
    {
        _sequence = sequenceBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<ChangeRpm.Payload> hope)
    {
        var fbk = Feedback.New(hope.AggId);
        if (hope == null) return BadRequest(fbk);
        try
        {
            fbk = await _sequence.ExecuteAsync(hope);
            return Ok(fbk);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return BadRequest(fbk);
    }
}