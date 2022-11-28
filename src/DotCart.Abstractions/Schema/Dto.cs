﻿using System.Text.Json.Serialization;
using DotCart.Core;

namespace DotCart.Abstractions.Schema;

public record Dto(string AggId, byte[] Data) : IDto
{
    public string AggId { get; } = AggId;

    [JsonIgnore] public byte[] Data { get; private set; } = Data;

    public void SetData(byte[] data)
    {
        Data = data;
    }

    public T GetPayload<T>() where T : IPayload
    {
        return !Data.Any()
            ? default
            : Data.FromBytes<T>();
    }

    public void SetPayload<T>(T state)
        where T : IPayload
    {
        Data = state.ToBytes();
    }
    public string MsgId { get; } = GuidUtils.LowerCaseGuid;
    public DateTime TimeStamp { get; private set; }
    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
    public static Dto New(string aggId, byte[] data)
    {
        return new Dto(aggId, data);
    }

    public static Dto New(string aggId, IPayload payload)
    {
        return new Dto(aggId, payload.ToBytes());
    }
}

public interface IDto<TPayload> : IMsg
{
}

public interface IDto : IMsg
{
    string AggId { get; }
}