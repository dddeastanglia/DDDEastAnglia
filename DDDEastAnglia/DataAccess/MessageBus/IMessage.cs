using System;

namespace DDDEastAnglia.DataAccess.MessageBus
{
    public interface IMessage
    {
         
    }

    public interface ICommand : IMessage
    {
        
    }

    public interface IHandle 
    {
        Type MessageType { get; }
        void Handle(IMessage message);
    }

    public abstract class BaseHandler<TMessage> : IHandle where TMessage : class, IMessage
    {
        public Type MessageType { get { return typeof (TMessage); } }
        public void Handle(IMessage message)
        {
            var typedMessage = message as TMessage;
            if (typedMessage == null)
            {
                return;
            }
            Handle(typedMessage);
        }

        public abstract void Handle(TMessage message);
    }
}