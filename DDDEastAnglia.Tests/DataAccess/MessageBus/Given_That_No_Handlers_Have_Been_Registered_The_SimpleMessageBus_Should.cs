using DDDEastAnglia.DataAccess.MessageBus;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess.MessageBus
{
    [TestFixture]
    public class Given_That_No_Handlers_Have_Been_Registered_The_SimpleMessageBus_Should
    {
        [Test]
        public void Throw_A_NoHandlerException_When_I_Send_A_Message()
        {
            Given_That_No_Handlers_Have_Been_Registered();
            When_I_Send_A_Message();
            Then_A_NoHandlerException_Is_Thrown();
        }

        private void Given_That_No_Handlers_Have_Been_Registered()
        {
            _messageBus = new SimpleMessageBus();
        }

        private void When_I_Send_A_Message()
        {
            try
            {
                _messageBus.Send(new TestMessage());
            }
            catch (NoHandlerException e)
            {
                _expectedException = e;
            }
        }

        private void Then_A_NoHandlerException_Is_Thrown()
        {
            Assert.That(_expectedException, Is.TypeOf<NoHandlerException>());
        }

        private SimpleMessageBus _messageBus;
        private NoHandlerException _expectedException;
    }
}