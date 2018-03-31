using System;
using LinkDotNet.MessageHandling.Contracts;

namespace Sample
{
    public class ProgramTerminater
    {
        private bool _isInitialized;

        public ProgramTerminater(IMessageBus messageBus)
        {
            messageBus.Subscribe<ProgramInitializedMessage>(Initialize);
        }

        public void TryTerminate()
        {
            if (!_isInitialized)
            {
                Console.WriteLine("Could not terminate program, because it wasn't initialized");
            }
            else
            {
                Console.WriteLine("Termiate program");
            }
        }

        private void Initialize()
        {
            Console.WriteLine("ProgramTerminater received a ProgramInitializedMessage");
            _isInitialized = true;
        }
    }
}