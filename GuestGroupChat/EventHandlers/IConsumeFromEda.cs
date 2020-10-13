using System;
using OT.EDA.Bus;
using OT.EDA.Consumers;

namespace GuestGroupChat.EventHandlers
{
    public interface IConsumeFromEda
    {
        IPersistentConsumer Subscribe(IBus edaBus, string subscriptionId, Action<Exception> onException);
    }
}