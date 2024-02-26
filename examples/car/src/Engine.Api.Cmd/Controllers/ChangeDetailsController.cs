using System.Diagnostics.Contracts;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using Engine.Context;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class ChangeDetailsController : ControllerBase
{
    private readonly IPipeT<ChangeDetails.IHopePipe, Contract.ChangeDetails.Payload> _pipe;

    public ChangeDetailsController(
        IPipeBuilderT<ChangeDetails.IHopePipe, Contract.ChangeDetails.Payload> pipeBuilder
    )
    {
        _pipe = pipeBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Contract.ChangeDetails.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            feedback = await _pipe.ExecuteAsync(hope);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return BadRequest(feedback);
    }
}