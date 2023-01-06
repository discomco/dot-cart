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

using EventStore.Client;

namespace DotCart.Drivers.EventStoreDB.Interfaces;

public interface IESDBEventSourcingClient : IESDBClientBase
{
    Task<IWriteResult> AppendToStreamAsync(
        string streamName,
        StreamRevision expectedRevision,
        IEnumerable<EventData> eventData,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<IWriteResult> AppendToStreamAsync(
        string streamName,
        StreamState expectedState,
        IEnumerable<EventData> eventData,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    // Task<DeleteResult> SoftDeleteAsync(
    //     string streamName,
    //     StreamRevision expectedRevision,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null,
    //     CancellationToken cancellationToken = default);
    //
    // Task<DeleteResult> SoftDeleteAsync(
    //     string streamName,
    //     StreamState expectedState,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null,
    //     CancellationToken cancellationToken = default);

    Task<StreamMetadataResult> GetStreamMetadataAsync(
        string streamName,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<IWriteResult> SetStreamMetadataAsync(
        string streamName,
        StreamState expectedState,
        StreamMetadata metadata,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<IWriteResult> SetStreamMetadataAsync(
        string streamName,
        StreamRevision expectedRevision,
        StreamMetadata metadata,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<ResolvedEvent> ReadAllAsync(
        Direction direction,
        Position position,
        long maxCount = 9223372036854775807,
        bool resolveLinkTos = false,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    EventStoreClient.ReadStreamResult ReadStreamAsync(
        Direction direction,
        string streamName,
        StreamPosition revision,
        long maxCount = 9223372036854775807,
        bool resolveLinkTos = false,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<StreamSubscription> SubscribeToAllAsync(
        FromAll startFrom,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared,
        bool resolveLinkTos = false,
        Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        SubscriptionFilterOptions? filterOptions = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    // Task<StreamSubscription> SubscribeToAllAsync(
    //     Position start,
    //     Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared,
    //     bool resolveLinkTos = false,
    //     Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     SubscriptionFilterOptions? filterOptions = null,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null,
    //     CancellationToken cancellationToken = default);

    // Task<StreamSubscription> SubscribeToStreamAsync(
    //     string streamName,
    //     Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared,
    //     bool resolveLinkTos = false,
    //     Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null,
    //     CancellationToken cancellationToken = default);

    Task<StreamSubscription> SubscribeToStreamAsync(
        string streamName,
        FromStream start,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared,
        bool resolveLinkTos = false,
        Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<DeleteResult> TombstoneAsync(
        string streamName,
        StreamRevision expectedRevision,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task<DeleteResult> TombstoneAsync(
        string streamName,
        StreamState expectedState,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);
}