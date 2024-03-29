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
using DotCart.Abstractions;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class OrderAttribute : Attribute
{
    public OrderAttribute(int order)
    {
        Order = order;
    }

    public int Order { get; }
}

public static class OrderAtt
{
    public static int Get<T>()
    {
        var atts = (OrderAttribute[])typeof(T).GetCustomAttributes(typeof(OrderAttribute),
            true);
        Guard.Against.AttributeNotDefined("Order", atts, typeof(T).FullName);
        return atts[0].Order;
    }

    public static int Get(object obj)
    {
        var atts = (OrderAttribute[])obj.GetType().GetCustomAttributes(typeof(OrderAttribute),
            true);
        Guard.Against.AttributeNotDefined("Order", atts, obj.GetType().FullName);
        return atts[0].Order;
    }
}