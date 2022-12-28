namespace DotCart.Abstractions.Schema;

public delegate TValueObject ValueObjectCtorT<out TValueObject>()
    where TValueObject : IValueObject;