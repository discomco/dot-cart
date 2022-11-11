namespace DotCart.Context.Schemas;

public static class FlagExtensions
{
    public static bool HasAllFlags(this int source, params int[] flags)
    {
        var res = true;
        foreach (var flag in flags)
        {
            res = res && source.HasFlag(flag);
            if (res == false)
                return false;
        }

        return res;
    }

    public static bool HasFlag(this int flags, int flag)
    {
        return (flags & flag) == flag;
    }

    public static bool NotHasFlag(this int flags, int flag)
    {
        return !flags.HasFlag(flag);
    }

    public static int SetFlag(this int flags, int flag)
    {
        return flags | flag;
    }

    public static int SetFlags(this int flags, params int[] newFlags)
    {
        return newFlags.Aggregate(flags, (current, flag) => current.SetFlag(flag));
    }

    public static int UnsetFlag(this int flags, int flag)
    {
        return flags ^ flag;
    }

    public static int UnsetFlags(this int flags, params int[] oldFlags)
    {
        return oldFlags.Aggregate(flags, (current, flag) => current.UnsetFlag(flag));
    }
}