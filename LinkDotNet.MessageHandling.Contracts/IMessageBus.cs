using System;

namespace LinkDotNet.MessageHandling.Contracts
{
    /// <summary>
    /// Provides an interface for receiving and sending messages between components
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Sends a message over the messagebus
        /// </summary>
        /// <param name="message">Message to send</param>
        void Send<T>(T message) where T : IMessage;

        /// <summary>
        /// Subscribes to the specific message and executes the registered action on receiving the message
        /// </summary>
        /// <param name="action">Action to be called, when the message is received</param>
        void Subscribe<T>(Action action) where T : IMessage;

        /// <summary>
        /// Subscribes to the specific message and executes the registered action on receiving the message
        /// </summary>
        /// <param name="action">Action to be called, when the message is received</param>
        void Subscribe<T>(Action<T> action) where T : IMessage;

        /// <summary>
        /// Revokes the subscription from the method to the message
        /// </summary>
        /// <param name="action">Action to unsubscribe</param>
        void Unsubscribe<T>(Action action) where T : IMessage;

        /// <summary>
        /// Revokes the subscription from the method to the message
        /// </summary>
        /// <param name="action">Action to unsubscribe</param>
        void Unsubscribe<T>(Action<T> action) where T : IMessage;

        /// <summary>
        /// Closes the messagebus, which will unsubscribe all actions
        /// </summary>
        void Close();
    }
}
