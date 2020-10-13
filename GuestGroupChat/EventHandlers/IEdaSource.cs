using System;

namespace GuestGroupChat.EventHandlers
{
    public interface IEdaSource
    {
        void BeginTracking(IConsumeFromEda messageBridge, Action<Exception> onUnhandledException);
    }
}