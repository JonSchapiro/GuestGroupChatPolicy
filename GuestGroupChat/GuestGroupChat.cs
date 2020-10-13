using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GuestGroupChat.EventHandlers;
using Microsoft.Extensions.Hosting;

namespace GuestGroupChat
{
    public class GuestGroupChat : IHostedService
    {
        private readonly IEnumerable<IConsumeFromEda> _adapters;
        private readonly IEdaSource _edaSource;
        private Task _executingTask;
        private CancellationTokenSource _cts;
        
        public GuestGroupChat(
            IEnumerable<IConsumeFromEda> adapters,
            IEdaSource edaSource)
        {
            _adapters = adapters;
            _edaSource = edaSource;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting GuestGroupChat");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = Task.Run(
                () =>
                {
                    foreach (var adapter in _adapters)
                    {
                        _edaSource.BeginTracking(
                            adapter,
                            async _ =>
                            {
                                Console.WriteLine("Unrecoverable exception. Stopping the service");
                                await StopAsync(cancellationToken).ConfigureAwait(false);
                                Environment.Exit(1);
                            });
                    }

                    Console.WriteLine("GuestGroupChat Policy service started");
                    
                }, cancellationToken);

            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping the Comms Reservation Reminder Policy service...");

            if (_executingTask == null)
                return;

            try
            {
                _cts.Cancel();
            }
            finally
            {
                // TODO: set a reasonable timer here waiting for the executing task to complete
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }

            Console.WriteLine("Comms Reservation Reminder Policy service stopped.");
        }
    }
}