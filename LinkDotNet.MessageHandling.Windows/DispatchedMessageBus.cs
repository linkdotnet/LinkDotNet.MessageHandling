using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace LinkDotNet.MessageHandling.Windows
{
    /// <summary>
    /// MessageBus, which will automatically dispatch every action on the main-thread
    /// </summary>
    public class DispatchedMessageBus : MessageBus
    {
        /// <inheritdoc cref="MessageBus"/>
        public sealed override void Send<T>(T message)
        {
            EnsureMessageIsValid(message);
            var handler = GetExecutingHandler(typeof(T));
            if (handler.Any())
            {
                // Call every handler in the UI-Thread through the dispatcher
                Application.Current.Dispatcher.Invoke(() => CallHandler(message, handler),
                    DispatcherPriority.Background);
            }
        }
    }
}
