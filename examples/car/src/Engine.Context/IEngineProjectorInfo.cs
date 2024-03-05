using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Schema;
using Engine.Contract;

namespace Engine.Context;

[GroupName("engine-sub")]
[IDPrefix(IDConstants.EngineIDPrefix)]
public interface IEngineProjectorInfo : IProjectorInfoB;