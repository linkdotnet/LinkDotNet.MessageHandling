# MessageBus

This MessageBus is a lean and mean implementation of a messagebus. In its current state it supports the basic features with
sending and receiving messages.

[![NuGet Version and Downloads count](https://buildstats.info/nuget/LinkDotNet.MessageBus)](https://www.nuget.org/packages/LinkDotNet.MessageBus) 

## Features
* Simple and fast
* Uses structuremap - So you can create it with DI
* Loose coupling
* No other dependencies (only if you use the DI-package in combination with structuremap)

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
    public string Reason { get; private set; }

    public CloseMessage(string reason)
    {
        Reason = reason;
    }
}
```

Thats everything.

## Using IoC
If you want to use Dependency Injection with structuremap just let structuremap scan the DI-Assembly.
This will create a single IMessageBus-instance.
Otherwise you can configure the messagebus yourself (to fit your needs).

## Advantages
**So why using a messagebus?**
The main advantage is you don't have to create dependencies where they are not useful.
You can send messages and components can receive those without knowing the counter part.
Another advantage is you send one message and you can have multiple receiver.
The big goal is the decoupling of individual components.
