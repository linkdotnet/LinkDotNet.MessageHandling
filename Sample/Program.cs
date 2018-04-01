using System;
using LinkDotNet.MessageHandling;

namespace Sample
{
    internal static class Program
    {
        /// <summary>
        /// A simple program to demonstrate how the messagebus works
        /// </summary>
        public static void Main()
        {
            var messageBus = new MessageBus();
            var initializer = new ProgramInitializer(messageBus);
            var terminator = new ProgramTerminater(messageBus);

            //// Try to terminate the program, without initialising
            terminator.TryTerminate();

            //// Initialize
            initializer.Initialize();

            //// Now try to terminate again
            terminator.TryTerminate();

            //// We can terminate our session... hurrraaa!
            //// Keep in mind, that the initializer and the terminator don't know each other
            //// They could be in different assemblies too
            Console.ReadKey();
        }
    }
}
