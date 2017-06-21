using DDDEastAnglia.DataAccess.MessageBus;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess.MessageBus
{
    [TestFixture]
    public class Given_That_The_TestMessageHandler_Has_Been_Registered_The_SimpleMessageBus_Should
    {
        [Test]
        public void Throw_A_NoHandlerException_When_I_Send_A_Different_Message()
        {
            Given_That_A_TestMessageHandler_Has_Been_Registered();
            When_I_Send_A_Different_Message();
            Then_A_NoHandlerException_Is_Thrown();
        }

        [Test]
        public void Direct_The_Message_To_The_Correct_Handler()
        {
            Given_That_A_TestMessageHandler_Has_Been_Registered();
            When_I_Send_A_Message();
            Then_The_Handler_Received_The_Message();
        }

        private void Given_That_A_TestMessageHandler_Has_Been_Registered()
        {
            _testMessageHandler = Substitute.For<IHandle>();
            _testMessageHandler.MessageType.Returns(typeof(TestMessage));
            _messageBus = new SimpleMessageBus(new [] {_testMessageHandler});
        }

        private void When_I_Send_A_Message()
        {
            try
            {
                _testMessage = new TestMessage();
                _messageBus.Send(_testMessage);
            }
            catch (NoHandlerException e)
            {
                _expectedException = e;
            }
        }

        private void When_I_Send_A_Different_Message()
        {
            try
            {
                _messageBus.Send(new OtherTestMessage());
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

        private void Then_The_Handler_Received_The_Message()
        {
            _testMessageHandler.Received().Handle(_testMessage);
        }

        private SimpleMessageBus _messageBus;
        private NoHandlerException _expectedException;
        private IHandle _testMessageHandler;
        private TestMessage _testMessage;
    }
}