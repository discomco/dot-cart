using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Context.Abstractions;

namespace DotCart.Context.Behaviors;

public static class Guards
{
    public static IGuardClause BehaviorIDNotSet(this IGuardClause guard, IAggregate aggregate)
    {
        if (aggregate.ID == null)
            throw BehaviorIDNotSetException.New;
        return guard;
    }

    public class BehaviorIDNotSetException : Exception
    {
        private BehaviorIDNotSetException() : base("Behavior ID is not set. please use SetID().")
        {
        }

        protected BehaviorIDNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BehaviorIDNotSetException(string? message) : base(message)
        {
        }

        public BehaviorIDNotSetException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public static Exception New => new BehaviorIDNotSetException();
    }
}