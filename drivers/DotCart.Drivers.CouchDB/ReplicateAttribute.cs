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

using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage( AttributeTargets.Interface)]
public class ReplicateAttribute : Attribute
{
    public ReplicateAttribute(bool doIt)
    {
        DoIt = doIt;
    }

    public bool DoIt { get; set; }
}

public static class ReplicateAtt
{
    public static bool Get<T>()
    {
        var atts = (ReplicateAttribute[])typeof(T).GetCustomAttributes(typeof(ReplicateAttribute), true);
        Guard.Against.AttributeNotDefined("Replicate", atts, typeof(T).ToString());
        return atts[0].DoIt;
    }

    public static bool Get(object obj)
    {
        var atts = (ReplicateAttribute[])obj.GetType().GetCustomAttributes(typeof(ReplicateAttribute), true);
        Guard.Against.AttributeNotDefined("Replicate", atts, obj.GetType().ToString());
        return atts[0].DoIt;
    }
}