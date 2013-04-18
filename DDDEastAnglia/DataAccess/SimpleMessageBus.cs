using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDEastAnglia.DataAccess
{
    public class SimpleMessageBus : IMessageBus
    {
        private readonly Dictionary<Type, IHandle> _handlers;

        public SimpleMessageBus()
        {
            
        }

        public SimpleMessageBus(IEnumerable<IHandle> handlers)
        {
            _handlers = handlers.ToDictionary(handler => handler.MessageType);
        }

        public void Send(IMessage message)
        {
            IHandle handler;
            if (_handlers.TryGetValue(message.GetType(), out handler))
            {
                handler.Handle(message);
            }
        }

        public void Register(IHandle handler)
        {
            _handlers.Add(handler.MessageType, handler);
        }
    }
}