using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq;

using Buaa.AIBot.Services;
using Buaa.AIBot.Services.Exceptions;
using Buaa.AIBot.Services.Models;
using Buaa.AIBot.Controllers;
using Buaa.AIBot.Controllers.Models;
using Buaa.AIBot.Repository;
using Buaa.AIBot.Repository.Models;
using Buaa.AIBot.Bot.Framework;
using Microsoft.Extensions.Options;

namespace AIBotTest.Controller
{
    public class BotControllerTest
    {
        private Mock<IBotRunner> mockBot;

        private Mock<IUserService> mockUser;

        private BotBody body;

        public BotControllerTest()
        {
            mockBot = new Mock<IBotRunner>();
            mockUser = new Mock<IUserService>();
            body = new BotBody();
        }

        [Fact]
        public async Task StartAsyncTest()
        {
            mockUser.Setup(user => user.GetUidFromToken(It.IsAny<HttpRequest>()))
            .Returns(0);
            mockBot.Setup(bot => bot.Start(0)).ReturnsAsync
            (
                new OutputInfo
                {
                    Messages = new string[]
                    {
                        "Hello",
                        "World"
                    },
                    Prompt = new string[]
                    {
                        "choice1",
                        "choice2"
                    }
                }
            );
            BotController controller = new BotController(mockBot.Object, mockUser.Object, null);

            var ret = await controller.StartAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = okRet.Value;
            var message = (IEnumerable<string>)resRet.GetType().GetProperty("Messages").GetValue(resRet, null);
            var prompt = (IEnumerable<string>)resRet.GetType().GetProperty("Prompt").GetValue(resRet, null);
            var messageIter = message.GetEnumerator();
            messageIter.MoveNext();
            Assert.Equal("Hello", messageIter.Current);
            messageIter.MoveNext();
            Assert.Equal("World", messageIter.Current);
            var promptIter = prompt.GetEnumerator();
            promptIter.MoveNext();
            Assert.Equal("choice1", promptIter.Current);
            promptIter.MoveNext();
            Assert.Equal("choice2", promptIter.Current);
        }

        [Fact]
        public async Task MessageAsyncTest()
        {
            mockUser.Setup(user => user.GetUidFromToken(It.IsAny<HttpRequest>()))
            .Returns(0);
            mockBot.Setup(bot => bot.Run(0, It.IsAny<InputInfo>())).ReturnsAsync
            (
                new OutputInfo
                {
                    Messages = new string[]
                    {
                        "Hello",
                        "World"
                    },
                    Prompt = new string[]
                    {
                        "choice1",
                        "choice2"
                    }
                }
            );
            BotController controller = new BotController(mockBot.Object, mockUser.Object, null);

            var ret = await controller.MessageAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = okRet.Value;
            var message = (IEnumerable<string>)resRet.GetType().GetProperty("Messages").GetValue(resRet, null);
            var prompt = (IEnumerable<string>)resRet.GetType().GetProperty("Prompt").GetValue(resRet, null);
            var messageIter = message.GetEnumerator();
            messageIter.MoveNext();
            Assert.Equal("Hello", messageIter.Current);
            messageIter.MoveNext();
            Assert.Equal("World", messageIter.Current);
            var promptIter = prompt.GetEnumerator();
            promptIter.MoveNext();
            Assert.Equal("choice1", promptIter.Current);
            promptIter.MoveNext();
            Assert.Equal("choice2", promptIter.Current);
        }
    }
}