using LinkDotNet.MessageHandling.Contracts;
using StructureMap;

namespace LinkDotNet.MessageHandling.DI
{
    /// <summary>
    /// Class to register the components
    /// </summary>
    public class MessageBusRegistry : Registry
    {
        /// <summary>
        /// c'tor
        /// </summary>
        public MessageBusRegistry() => ForSingletonOf<IMessageBus>().Use<MessageBus>();
    }
}
