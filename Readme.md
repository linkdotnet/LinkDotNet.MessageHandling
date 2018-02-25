# MessageBus

This MessageBus is a lean and mean implementation of a messagebus. In its current state it supports the basic features with
sending and receiving messages.

## Features
* Simple and fast
* Uses structuremap - So you can create it with DI

## Example
In the following example a class will listen to two types of message and will send a message on its close method:
```cs
using LinkDotNet.MessageHandling.Contracts

public class Foo
{
    private IMessageBus _messageBus;

    /// <summary>
    /// c'tor
    /// </summary>
    public Foo(IMessageBus messageBus)
    {
        messageBus.Subscribe<IMessage>(() => Console.WriteLine("I'm listening to every message"));
        messageBus.Subscribe<InfoMessage>((msg) => Console.WriteLine("Content: " + msg.InfoText));

        _messageBus = messageBus;
    }

    /// <summary>
    /// Closes something
    /// </summary>
    public void Close()
    {
        _messageBus.Send(new CloseMessage("Closing"));
    }
}

public class CloseMessage : IMessage
{
    public string Reason { get; set; }

    public CloseMessage(string reason)
    {
        Reason = reason;
    }
}
```

Thats everything.

## Using IoC
If you want to use Dependency Injection with structuremap just let structuremap scan the MessageBus.DI-Assembly.
This will create an IMessageBus object which exists only once per lifecycle.

## Advantages
**So why using a messagebus?**
The main advanteage is you don't have to create dependencies when they are not useful.
You can send messages and components can receive it without knowing that this particular component exists.
Another advantage is you send one message and you can have multiple receiver.
So you can decouple your components.

