using System;
using Moq;
using NUnit.Framework;
using LinkDotNet.MessageHandling.Contracts;

namespace LinkDotNet.MessageHandling.Tests
{
    [TestFixture]
    public abstract class MessageBusFixtureBase<TMessageBus> where TMessageBus : MessageBus
    {
        protected TMessageBus MessageBus { get; private set; }

        [SetUp]
        public void SetUp()
        {
            MessageBus = CreateMessageBus();
        }

        [Test]
        public void Should_call_handler_on_registered_message()
        {
            var iWasCalled = false;
            MessageBus.Subscribe<IMessage>(() => iWasCalled = true);

            MessageBus.Send(new Mock<IMessage>().Object);

            Assert.That(iWasCalled, Is.True);
        }

        [Test]
        public void Should_not_call_handler_when_is_not_associated_message()
        {
            var iWasCalled = false;
            MessageBus.Subscribe<AnotherFakeMessage>(() => iWasCalled = true);

            MessageBus.Send(new FakeMessage());

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_pass_in_parameters()
        {
            var id = 0;
            MessageBus.Subscribe<AnotherFakeMessage>((msg) => id = msg.Id);

            MessageBus.Send(new AnotherFakeMessage(3));

            Assert.That(id, Is.EqualTo(3));
        }

        [Test]
        public void Should_be_covariant()
        {
            var baseWasCalled = false;
            var subWasCalled = false;
            MessageBus.Subscribe<IMessage>(() => baseWasCalled = true);
            MessageBus.Subscribe<AnotherFakeMessage>(() => subWasCalled = true);

            MessageBus.Send(new FakeMessage());

            Assert.That(baseWasCalled, Is.True);
            Assert.That(subWasCalled, Is.False);
        }

        [Test]
        public void Should_close_messagebus_and_stay_closed()
        {
            var wasCalled = false;
            var wasSubscribedAfterClose = false;
            MessageBus.Subscribe<FakeMessage>(() => wasCalled = true);
            MessageBus.Close();
            MessageBus.Subscribe<FakeMessage>(() => wasSubscribedAfterClose = true);

            MessageBus.Send(new FakeMessage());

            Assert.That(wasCalled, Is.False);
            Assert.That(wasSubscribedAfterClose, Is.False);
        }

        [Test]
        public void Should_throw_argument_exception_when_sending_message_is_null()
        {
            try
            {
                MessageBus.Send<IMessage>(null);
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
            var actionToUnsubscribe = new Action(() => { iWasCalled = true; });
            MessageBus.Subscribe<IMessage>(actionToUnsubscribe);
            MessageBus.Unsubscribe<IMessage>(actionToUnsubscribe);

            MessageBus.Send(new Mock<IMessage>().Object);

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_unsubscribe_parametrized_action_from_message()
        {
            var iWasCalled = false;
            var actionToUnsubscribe = new Action<IMessage>(msg => { iWasCalled = true; });
            MessageBus.Subscribe(actionToUnsubscribe);

            MessageBus.Unsubscribe(actionToUnsubscribe);

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_throw_argument_null_exception_when_action_is_null()
        {
            Assert.Throws(typeof(ArgumentNullException), () => MessageBus.Unsubscribe<IMessage>((Action) null));
            Assert.Throws(typeof(ArgumentNullException), () => MessageBus.Unsubscribe((Action<IMessage>) null));
        }

        [Test]
        public void Should_not_throw_exception_when_removing_not_added_subscription()
        {
            var unknownAction = new Action<IMessage>(msg => { });

            Assert.DoesNotThrow(() => MessageBus.Unsubscribe(unknownAction));
        }

        protected abstract TMessageBus CreateMessageBus();
    }

    public class MessageBusFixture : MessageBusFixtureBase<MessageBus>
    {
        protected override MessageBus CreateMessageBus()
        {
            return new MessageBus();
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