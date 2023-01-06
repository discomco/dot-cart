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

using DotCart.Abstractions.Schema;
using DotCart.Defaults.Elastic;
using Nest;

namespace DotCart.Drivers.ElasticSearch;

public class ElasticStoreT<TDoc> : IElasticStore<TDoc>
    where TDoc : class, IState
{
    private readonly IElasticClient _client;

    public ElasticStoreT(IElasticClient client)
    {
        _client = client;
    }

    public void Close()
    {
    }

    public void Dispose()
    {
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.CompletedTask;
    }

    public Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDoc?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}