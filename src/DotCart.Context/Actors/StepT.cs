////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c)2023 DisComCo Sp.z.o.o. (http://discomco.pl)
////////////////////////////////////////////////////////////////////////////////////////////////
// Permission is hereby granted, free of charge,
// to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS",
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////

using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Actors;

public abstract class StepT<TPipeInfo, TPayload>
    : IStepT<TPipeInfo, TPayload>
    where TPayload : IPayload
    where TPipeInfo : IPipeInfoB
{
    public IPipeT<TPipeInfo, TPayload> Pipe { get; set; }

    public string Name => GetName();

    public int Order => GetOrder();

    public Importance Level { get; set; }

    public void SetPipe(IPipeT<TPipeInfo, TPayload> pipe)
    {
        Pipe = pipe;
    }

    public async Task<Feedback> DoStepAsync(IDto msg, Feedback fbk, CancellationToken cancellationToken)
    {
        Log.Information($"{AppVerbs.Do} step {Name}({msg.GetType().Name})");
        var f = await ExecuteAsync(msg, fbk, cancellationToken);
        Log.Information($"{AppFacts.Done} step {Name}({msg.GetType().Name}) ~> is_success:{f.IsSuccess}");
        return f;
    }

    public abstract Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
        CancellationToken cancellationToken = default);

    protected abstract string GetName();

    private int GetOrder()
    {
        try
        {
            return OrderAtt.Get(this);
        }
        catch (Exception)
        {
            return 0;
        }
    }
}