namespace DDDEastAnglia.DataAccess
{
    public interface IMessageBus
    {
        void Send(IMessage message);
    }
}