using System;
using GuestGroupChat.Services;
using OT.EDA.Bus;
using OT.EDA.Consumers;
using OT.Messaging.MDA.Consumer.Reservation.Events;

namespace GuestGroupChat.EventHandlers
{
    public class ReservationConsumer : IConsumeFromEda
    {
        public IPersistentConsumer Subscribe(IBus bus, string subscriptionId, Action<Exception> onException)
        {
            return bus.Subscribe(new SubscribeOptions<ReservationModified>
            {
                OnMessageHandler = OnMessage(),
                OnErrorHandler = OnException,
                SubscriptionId = subscriptionId
            });
        }
        
        private void OnException(Exception exception)
        {
            Console.WriteLine("Exception " + exception.Message);
        }

        private Action<ReservationModified> OnMessage()
        {
            return async edaEvent =>
            {
                var chatId = ChatService.CreateNewChatAsync(edaEvent.UpdatedState.ReservationDateTimeUTC);
                Console.WriteLine("Rid: " + edaEvent.UpdatedState.RestaurantID + "-ConfNumber" + edaEvent.UpdatedState.ConfirmationNumber + "-GPID: " + edaEvent.UpdatedState.GPID + "-ChatId:" + chatId);
            };
        }
    }
}