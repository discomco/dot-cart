﻿using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IDocStore<TDoc> : IClose, IDisposable, IAsyncDisposable, ICloseAsync
    where TDoc : IState
{
    Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default);
    Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> Exists(string id, CancellationToken cancellationToken = default);
    Task<TDoc?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> HasData(CancellationToken cancellationToken = default);
}