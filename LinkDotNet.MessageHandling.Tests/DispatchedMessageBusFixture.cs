using LinkDotNet.MessageHandling.Windows;
using NUnit.Framework;

namespace LinkDotNet.MessageHandling.Tests
{
    //// todo: Restructure
    //// We need the following:
    //// This test-assembly should be an .netstandard - Assembly instead of .net core
    //// Afterwards we can refactor this test into its own assembly with WPF Stuff for testing
    [Ignore("Can't load PresentationFramework correctly")]
    public class DispatchedMessageBusFixture : MessageBusFixtureBase<DispatchedMessageBus>
    {
        protected override DispatchedMessageBus CreateMessageBus()
        {
            return new DispatchedMessageBus();
        }
    }
}