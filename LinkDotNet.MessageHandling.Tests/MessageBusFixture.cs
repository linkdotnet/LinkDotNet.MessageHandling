﻿using System;
using Moq;
using NUnit.Framework;
using LinkDotNet.MessageHandling.Contracts;

namespace LinkDotNet.MessageHandling.Tests
{
    [TestFixture]
    public class MessageBusFixture
    {
        private MessageBus _messageBus;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBus();
        }

        [Test]
        public void Should_call_handler_on_registered_message()
        {
            var iWasCalled = false;
            _messageBus.Subscribe<IMessage>(() => iWasCalled = true);

            _messageBus.Send(new Mock<IMessage>().Object);

            Assert.That(iWasCalled, Is.True);
        }

        [Test]
        public void Should_not_call_handler_when_is_not_associated_message()
        {
            var iWasCalled = false;
            _messageBus.Subscribe<AnotherFakeMessage>(() => iWasCalled = true);

            _messageBus.Send(new FakeMessage());

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_pass_in_parameters()
        {
            var id = 0;
            _messageBus.Subscribe<AnotherFakeMessage>((msg) => id = msg.Id);

            _messageBus.Send(new AnotherFakeMessage(3));

            Assert.That(id, Is.EqualTo(3));
        }

        [Test]
        public void Should_be_covariant()
        {
            var baseWasCalled = false;
            var subWasCalled = false;
            _messageBus.Subscribe<IMessage>(() => baseWasCalled = true);
            _messageBus.Subscribe<AnotherFakeMessage>(() => subWasCalled = true);

            _messageBus.Send(new FakeMessage());

            Assert.That(baseWasCalled, Is.True);
            Assert.That(subWasCalled, Is.False);
        }

        [Test]
        public void Should_close_messagebus_and_stay_closed()
        {
            var wasCalled = false;
            var wasSubscribedAfterClose = false;
            _messageBus.Subscribe<FakeMessage>(() => wasCalled = true);
            _messageBus.Close();
            _messageBus.Subscribe<FakeMessage>(() => wasSubscribedAfterClose = true);

            _messageBus.Send(new FakeMessage());

            Assert.That(wasCalled, Is.False);
            Assert.That(wasSubscribedAfterClose, Is.False);
        }

        [Test]
        public void Should_throw_argument_exception_when_sending_message_is_null()
        {
            try
            {
                _messageBus.Send<IMessage>(null);
            }
            catch (ArgumentNullException argExc)
            {
                Assert.That(argExc.ParamName, Is.EqualTo("message"));
            }
        }

        [Test]
        public void Should_unsubscribe_action_from_message()
        {
            var iWasCalled = false;
            var actionToUnsubscribe = new Action(() =>
            {
                iWasCalled = true;
            });
            _messageBus.Subscribe<IMessage>(actionToUnsubscribe);
            _messageBus.Unsubscribe<IMessage>(actionToUnsubscribe);

            _messageBus.Send(new Mock<IMessage>().Object);

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_unsubscribe_parametrized_action_from_message()
        {
            var iWasCalled = false;
            var actionToUnsubscribe = new Action<IMessage>(msg =>
            {
                iWasCalled = true;
            });
            _messageBus.Subscribe(actionToUnsubscribe);

            _messageBus.Unsubscribe(actionToUnsubscribe);

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_throw_argument_null_exception_when_action_is_null()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _messageBus.Unsubscribe<IMessage>((Action)null));
            Assert.Throws(typeof(ArgumentNullException), () => _messageBus.Unsubscribe((Action<IMessage>)null));
        }

        [Test]
        public void Should_not_throw_exception_when_removing_not_added_subscription()
        {
            var unknownAction = new Action<IMessage>(msg => { });

            Assert.DoesNotThrow(() => _messageBus.Unsubscribe(unknownAction));
        }
    }

    public class FakeMessage : IMessage
    {
    }

    public class AnotherFakeMessage : IMessage
    {
        public int Id { get; }

        public AnotherFakeMessage(int id)
        {
            Id = id;
        }
    }
}
