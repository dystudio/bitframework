﻿using Bit.Core.Contracts;
using Bit.Core.Exceptions;
using Bit.Core.Models.Events;
using Bit.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bit.CSharpClientSample.ViewModels
{
    public class TestViewModel : BitViewModelBase
    {
        public virtual BitDelegateCommand CloseCommand { get; set; }

        public virtual BitDelegateCommand IncreaseStepsCountCommand { get; set; }
        public virtual BitDelegateCommand ThrowExceptionCommand { get; set; }
        public virtual BitDelegateCommand TestSignalrCommand { get; set; }

        public virtual IMessageReceiver MessageReceiver { get; set; }

        public virtual IEventAggregator EventAggregator { get; set; }

        public int StepsCount { get; set; }

        public TestViewModel()
        {
            CloseCommand = new BitDelegateCommand(Close);
            ThrowExceptionCommand = new BitDelegateCommand(ThrowException);
            IncreaseStepsCountCommand = new BitDelegateCommand(IncreaseSteps);
            TestSignalrCommand = new BitDelegateCommand(TestSignalr);
        }

        async Task ThrowException()
        {
            throw new DomainLogicException("It's not good!");
        }

        async Task IncreaseSteps()
        {
            StepsCount++;
            TelemetryServices.All()
                .TrackEvent("StepsCountChanged", new Dictionary<string, string>
                {
                    { "NewValue", StepsCount.ToString() }
                });
        }

        async Task Close()
        {
            await NavigationService.GoBackAsync();
        }

        private IDisposable signalr_MessageReceived_token;
        private IDisposable server_ConnectivityChanged_token;

        async Task TestSignalr()
        {
            if (MessageReceiver.ShouldBeConnected)
            {
                signalr_MessageReceived_token?.Dispose();
                server_ConnectivityChanged_token?.Dispose();
                await MessageReceiver.Stop(CancellationToken.None);
            }
            else
            {
                signalr_MessageReceived_token = EventAggregator.GetEvent<MessageReceivedEvent>()
                    .SubscribeAsync(OnMessageReceived);
                server_ConnectivityChanged_token = EventAggregator.GetEvent<ServerConnectivityChangedEvent>()
                    .SubscribeAsync(ServerConnectivityChanged);
                await MessageReceiver.Start(CancellationToken.None);
            }
        }

        async Task ServerConnectivityChanged(ServerConnectivityChangedEvent arg)
        {

        }

        async Task OnMessageReceived(MessageReceivedEvent message)
        {
            if (message.Key == "NewWord")
            {
                string word = message.AsJObject().Value<string>("Word");
            }
        }

        public override async Task OnDestroyAsync()
        {
            signalr_MessageReceived_token?.Dispose();
            server_ConnectivityChanged_token?.Dispose();
            await MessageReceiver.Stop(CancellationToken.None);
            await base.OnDestroyAsync();
        }
    }
}
