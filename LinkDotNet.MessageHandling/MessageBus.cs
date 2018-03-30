using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LinkDotNet.MessageHandling.Contracts;

namespace LinkDotNet.MessageHandling
{
    /// <summary>
    /// Lean and mean implementation of the messagebus
    /// </summary>
    public class MessageBus : IMessageBus
    {
        /// <summary>
        /// Holds every message which is send through this messagebus mapped to there actions                           
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> _handler = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Determines whether the messagebus can send and receive messages
        /// </summary>
        private bool _messageBusOpen;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageBus()
        {
            _messageBusOpen = true;
        }

        /// <summary>
        /// Sends an message
        /// </summary>
        /// <param name="message">The message-object to be send</param>
        public void Send<T>(T message) where T : IMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var executingHandler = GetExecutingHandler<T>(typeof(T));
            if (executingHandler.Any())
            {
                // Call every handler
                CallHandler(message, executingHandler);
            }
        }

        /// <summary>
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        public void Subscribe<T>(Action action) where T : IMessage
        {
            AddHandlerToMessageType(typeof(T), action);
        }

        /// <summary>
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        public void Subscribe<T>(Action<T> action) where T : IMessage
        {
            AddHandlerToMessageType(typeof(T), action);
        }

        /// <summary>
        /// Revokes the subscription from the method to the message
        /// </summary>
        public void Unsubscribe<T>(Action action) where T : IMessage
        {
            DeleteHandlerByMessage<T>(action);
        }

        /// <summary>
        /// Revokes the subscription from the method to the message
        /// </summary>
        public void Unsubscribe<T>(Action<T> action) where T : IMessage
        {
            DeleteHandlerByMessage<T>(action);
        }

        /// <summary>
        /// Closes the messagebus
        /// </summary>
        public void Close()
        {
            _messageBusOpen = false;
            _handler.Clear();
        }

        private static void CallHandler<T>(T message, List<Delegate> executingHandler) where T : IMessage
        {
            foreach (var actionHandler in executingHandler)
            {
                if (actionHandler is Action<T> parametrizedAction)
                {
                    parametrizedAction(message);
                    continue;
                }

                (actionHandler as Action)?.Invoke();
            }
        }

        private List<Delegate> GetExecutingHandler<T>(Type type) where T : IMessage
        {
            var executedHandler = new List<Delegate>();
            foreach (var handler in _handler.Keys.Where(handler => handler.GetTypeInfo().IsAssignableFrom(type)))
            {
                executedHandler.AddRange(_handler[handler]);
            }

            return executedHandler;
        }

        private void AddHandlerToMessageType(Type messageType, Delegate action)
        {
            if (!_messageBusOpen)
            {
                return;
            }

            MapActionToHandlerByType(messageType, action);
        }

        private void MapActionToHandlerByType(Type messageType, Delegate action)
        {
            if (_handler.ContainsKey(messageType))
            {
                _handler[messageType].Add(action);
            }
            else
            {
                _handler[messageType] = new List<Delegate> { action };
            }
        }

        private void DeleteHandlerByMessage<T>(Delegate handlerToRemove) where T : IMessage
        {
            if (handlerToRemove == null)
            {
                throw new ArgumentNullException(nameof(handlerToRemove));
            }

            if (!_handler.ContainsKey(typeof(T)))
            {
                return;
            }

            var handler = _handler[typeof(T)];
            if (handler.Contains(handlerToRemove))
            {
                handler.Remove(handlerToRemove);
            }
        }
    }
}
