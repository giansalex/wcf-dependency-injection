using NUnit.Framework;
using System;
using WcfService.Behaviour;
using WcfService.Handler;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using WcfService.Services;
using System.ServiceModel.Channels;
using Moq;

namespace WcfService.Tests.Behaviour
{
    [TestFixture]
    public class ServiceErrorBehaviourAttributeTests
    {
        private readonly ServiceErrorBehaviourAttribute _behaviour = new ServiceErrorBehaviourAttribute(typeof(ElmahErrorHandler));

        [Test]
        public void EmptyActions()
        {
            _behaviour.Validate(null, null);
            _behaviour.AddBindingParameters(null, null, null, null);
        }

        [Test]
        public void ApplyBehaviour()
        {
            var serviceHost = GetServiceHost();

            _behaviour.ApplyDispatchBehavior(null, serviceHost);

            Assert.NotZero(serviceHost
                .ChannelDispatchers
                .OfType<ChannelDispatcher>()
                .Sum(r => r.ErrorHandlers.Count));
        }

        private ServiceHost GetServiceHost()
        {
            var serviceHost = new ServiceHost(typeof(CalculatorService), new[] { new Uri("http://127.0.0.1:80/") });

            serviceHost.ChannelDispatchers.Add(new ChannelDispatcher(Mock.Of<IChannelListener>()));

            return serviceHost;
        }
    }
}
