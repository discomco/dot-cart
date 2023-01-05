using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class StopController : ControllerBase
{
    private readonly ISequenceT<Stop.Payload> _sequence;

    public StopController(ISequenceBuilderT<Stop.Payload> sequenceBuilder)
    {
        _sequence = sequenceBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Stop.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            feedback = await _sequence.ExecuteAsync(hope);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return BadRequest(feedback);
    }
}