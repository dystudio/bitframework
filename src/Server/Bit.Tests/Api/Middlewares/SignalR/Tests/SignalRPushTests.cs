﻿using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.OData.Client;
using System.Threading.Tasks;

namespace Bit.Tests.Api.Middlewares.SignalR.Tests
{
    [TestClass]
    public class SignalRPushTests
    {
        [TestMethod]
        [TestCategory("SignalR"), TestCategory("WebApi")]
        public virtual async Task PushFromSomeUserToAnotherUserUsingWebApi()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse tokenOfUser1 = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                TokenResponse tokenOfUser2 = await testEnvironment.Server.Login("User2", "ValidPassword", clientId: "TestResOwner");

                IODataClient odataClientOfUser1 = testEnvironment.BuildTestODataClient(token: tokenOfUser1);

                TaskCompletionSource<bool> onMessageReceivedCalled = new TaskCompletionSource<bool>();

                await testEnvironment.Server.BuildSignalRClient(tokenOfUser2, (messageKey, messageArgs) =>
                {
                    if (messageKey == "NewWord" && messageArgs.Word == "Some word")
                        onMessageReceivedCalled.SetResult(true);
                });

                await odataClientOfUser1.TestModels()
                    .PushSomeWordToAnotherUser(to: "User2", word: "Some word")
                    .ExecuteAsync();

                Assert.AreEqual(true, await onMessageReceivedCalled.Task);
            }
        }

        [TestMethod]
        [TestCategory("SignalR"), TestCategory("WebApi"), TestCategory("BackgroundJobs")]
        public virtual async Task PushFromSomeUserToAnotherUserUsingWebApiAndBackgroundJobWorker()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse tokenOfUser1 = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                TokenResponse tokenOfUser2 = await testEnvironment.Server.Login("User2", "ValidPassword", clientId: "TestResOwner");

                IODataClient odataClientOfUser1 = testEnvironment.BuildTestODataClient(token: tokenOfUser1);

                TaskCompletionSource<bool> onMessageReceivedCalled = new TaskCompletionSource<bool>();

                await testEnvironment.Server.BuildSignalRClient(tokenOfUser2, (messageKey, messageArgs) =>
                {
                    onMessageReceivedCalled.SetResult(true);
                });

                await odataClientOfUser1.TestModels()
                    .PushSomeWordToAnotherUsingBackgroundJobWorker(to: "User2", word: "Some word")
                    .ExecuteAsync();

                Assert.AreEqual(true, await onMessageReceivedCalled.Task);
            }
        }
    }
}