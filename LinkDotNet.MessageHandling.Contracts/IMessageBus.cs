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
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        void Subscribe<T>(Action action) where T : IMessage;

        /// <summary>
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        void Subscribe<T>(Action<T> action) where T : IMessage;

        /// <summary>
        /// Closes the messagebus, which will unsubscribe all actions
        /// </summary>
        void Close();
    }
}
