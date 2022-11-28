using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;


public delegate TCmd CmdCtorT<out TCmd, in TAggID, in TPayload>(TAggID ID, TPayload payload) 
    where TCmd: ICmd
    where TAggID : IID
    where TPayload: IPayload; 
