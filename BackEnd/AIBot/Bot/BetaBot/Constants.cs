using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.BetaBot
{
    public static class Constants
    {
        public static class Key
        {
            public static readonly string ShortQuestion = "ShortQuestion";
            public static readonly string LongQuestion = "LongQuestion";
            public static readonly string Checking = "Checking";
            public static readonly string Cached_SortedSelectedQuestions = "Cached_SSQ";
            public static readonly string SelectedTags = "SelectedTags";
        }

        public static class Value
        {
            public static readonly string CheckingShort = "CheckingShort";
            public static readonly string CheckingLong = "CheckingLong";
        }
    }
}
