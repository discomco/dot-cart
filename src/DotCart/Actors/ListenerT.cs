// MIT License
//
// Copyright (c) 2022-2023 by DisComCo Sp.z.o.o.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;
using Serilog;

namespace DotCart.Actors;

public class ListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>
    : ActorT<TSpoke>, IListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>
    where TCmdPayload : IPayload
    where TMeta : IMetaB
    where TFactPayload : IPayload
    where TSpoke : ISpokeT<TSpoke>
    where TPipeInfo : IPipeInfoB
{
    private readonly Fact2Cmd<TCmdPayload, TMeta, TFactPayload> _fact2Cmd;
    private readonly IPipeBuilderT<TPipeInfo, TFactPayload> _pipeBuilder;

    protected ListenerT(
        IListenerDriverB driver,
        IExchange exchange,
        IPipeBuilderT<TPipeInfo, TFactPayload> pipeBuilder) : base(exchange)
    {
        Driver = driver;
        _pipeBuilder = pipeBuilder;
    }

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        try
        {
            var fact = (FactT<TFactPayload, TMeta>)msg;
            var pipe = _pipeBuilder.Build();
            Log.Information($"{AppFacts.Received} fact({FactTopicAtt.Get<TFactPayload>()}) ~> [{Name}]");
            var fdbk = await pipe.ExecuteAsync(fact, cancellationToken);
        }
        catch (Exception e)
        {
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())}");
        }
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        Driver.SetActor(this);
        return ((IListenerDriverB)Driver).StartListeningAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return ((IListenerDriverB)Driver).StopListeningAsync(cancellationToken);
    }
}