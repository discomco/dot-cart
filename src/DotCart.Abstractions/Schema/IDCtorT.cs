namespace DotCart.Abstractions.Schema;

public delegate TID IDCtorT<out TID>(string value = "")
    where TID : IID;