using CobimExplorer.Services.Messenger;
using System;
using Xunit;

namespace CobimExplorer.TestDataMessenger
{
    public class TestDataMessenger
    {
        private readonly Messenger _messenger = new Messenger();
        public record FakeMessage(string Message);
        public record AnotherFakeMessage(string Message);
        public interface IFakeObserver { void CallMeWithMessage(object message); }

        [Fact]
        public void Send_NotifiesSubscribers_WhenSingleSubscriber()
        {
            var fakeObserver = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _messenger.Subscribe<FakeMessage>(fakeObserver.Object, fakeObserver.Object.CallMeWithMessage);

            _messenger.Send(message);

            fakeObserver.Verify(x => x.CallMeWithMessage(message), Times.Once());
        }

        [Fact]
        public void Send_DoesNotNotifiesSubscribers_WhenSingleUnSubscriber()
        {
            var fakeObserver = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _messenger.Subscribe<FakeMessage>(fakeObserver.Object, fakeObserver.Object.CallMeWithMessage);

            _messenger.UnSubscribe<FakeMessage>(fakeObserver.Object);
            _messenger.Send(message);

            fakeObserver.Verify(x => x.CallMeWithMessage(message), Times.Never());
        }

        [Fact]
        public void Send_NotifiesSubscribers_WhenMoreThanOneSubscriber()
        {
            var fakeObserver1 = new Mock<IFakeObserver>();
            var fakeObserver2 = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _messenger.Subscribe<FakeMessage>(fakeObserver1.Object, fakeObserver1.Object.CallMeWithMessage);
            _messenger.Subscribe<FakeMessage>(fakeObserver2.Object, fakeObserver2.Object.CallMeWithMessage);

            _messenger.Send(message);

            fakeObserver1.Verify(x => x.CallMeWithMessage(message), Times.Once());
            fakeObserver2.Verify(x => x.CallMeWithMessage(message), Times.Once());
        }
    }
}
