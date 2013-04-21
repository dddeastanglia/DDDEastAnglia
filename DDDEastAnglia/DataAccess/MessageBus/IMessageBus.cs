namespace DDDEastAnglia.DataAccess.MessageBus
{
    public interface IMessageBus
    {
        void Send(IMessage message);
    }
}