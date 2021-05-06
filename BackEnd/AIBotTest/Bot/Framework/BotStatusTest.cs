using System;
using Xunit;
using Buaa.AIBot.Bot.Framework;



namespace AIBotTest.Bot.Framework
{
    public class BotStatusTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("true")]
        [InlineData("null")]
        [InlineData("[abcd, efgh]")]
        [InlineData("123456789")]
        [InlineData("{\"msg\":true}")]
        [InlineData("\"\"")]
        public void GetSameAfterPut_String(string str)
        {
            IBotStatusContainer status = new BotStatus<int>();
            status.Put("key", str);
            var res = status.Get<string>("key");
            Assert.Equal(str, res);
        }
    }
}
