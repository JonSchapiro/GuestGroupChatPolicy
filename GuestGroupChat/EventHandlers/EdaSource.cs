using System;
using System.Collections.Generic;
using OT.EDA.Bus;
using OT.EDA.Consumers;

namespace GuestGroupChat.EventHandlers
{
    public class EdaSource : IEdaSource
    {
        private const string SubscriberId = "Comms.Reservation.Reminder.Policy";
        private readonly IBus _bus;
        private readonly IDictionary<Type, IPersistentConsumer> _consumers;
        
        private bool _disposed;
        private Action<Exception> _onUnhandledException;
        
        public EdaSource(IBus bus)
        {
            _bus = bus;
            _consumers = new Dictionary<Type, IPersistentConsumer>();
        }
        
        public void BeginTracking(IConsumeFromEda messageBridge, Action<Exception> onUnhandledException)
        {
            _onUnhandledException = onUnhandledException;
            if (_consumers.ContainsKey(messageBridge.GetType()))
            {
                throw new InvalidOperationException(
                    $"Already tracking events for the message type {messageBridge.GetType()}");
            }

            _consumers.Add(messageBridge.GetType(), RegisterConsumer(messageBridge));
        }
        
        public void Dispose()
        {
            if (!(_bus is IDisposable disposableBus))
                return;

            if (_disposed)
                return;

            disposableBus.Dispose();
            _disposed = true;
        }

        private IPersistentConsumer RegisterConsumer(IConsumeFromEda consumer)
        {
            return consumer.Subscribe(_bus, SubscriberId, exception => _onUnhandledException(exception));
        }
    }
}