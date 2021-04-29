using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.Framework
{
    public class BotException : Exception
    {
        public BotException() : base() { }
        public BotException(string msg) : base(msg) { }
        public BotException(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public class SessionNullException : BotException
    {
        public SessionNullException(int uid) :
            base($"User with uid={uid}'s bot session may has been timeout or not start yet.")
        { }
    }
}
