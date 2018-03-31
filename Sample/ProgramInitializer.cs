using System;
using LinkDotNet.MessageHandling.Contracts;

namespace Sample
{
    public class ProgramInitializer
    {
        private readonly IMessageBus _messageBus;

        public ProgramInitializer(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Initialize()
        {
            //// We could do some stuff here...
            Console.WriteLine("Initializing and sending message...");
            _messageBus.Send(new ProgramInitializedMessage());
        }
    }
}
