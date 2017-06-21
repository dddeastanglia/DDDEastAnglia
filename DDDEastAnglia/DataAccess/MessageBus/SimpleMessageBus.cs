using System;
using System.Collections.Generic;

namespace DDDEastAnglia.DataAccess.MessageBus
{
    public class SimpleMessageBus : IMessageBus
    {
        private readonly Dictionary<Type, IHandle> _handlers = new Dictionary<Type, IHandle>();

        public SimpleMessageBus(IEnumerable<IHandle> handlers)
        {
            if (handlers == null)
            {
                return;
            }
            foreach (var handler in handlers)
            {
                Register(handler);
            }
        }

        public void Send(IMessage message)
        {
            IHandle handler;
            if (_handlers.TryGetValue(message.GetType(), out handler))
            {
                handler.Handle(message);
            }
            else
            {
                throw new NoHandlerException(message);
            }
        }

        public void Register(IHandle handler)
        {
            _handlers.Add(handler.MessageType, handler);
        }
    }

    public class NoHandlerException : Exception
    {
        public NoHandlerException(IMessage message)
            : base("Could not find a handler for '" + message.GetType().Name + "'")
        {
            MessageReceived = message;
        }

        public IMessage MessageReceived { get; private set; }
    }
}