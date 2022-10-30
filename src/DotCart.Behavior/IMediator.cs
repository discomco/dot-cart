namespace DotCart.Behavior;

public interface IMediator : IEffect,IDisposable
{
    void Subscribe<T>(Action<T> handler);
    void Subscribe<T>(Func<T, Task> handler);
    Task PublishAsync<T>(T publishedEvent);
}