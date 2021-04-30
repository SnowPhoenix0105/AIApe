using System;

namespace Buaa.AIBot.Services.Models
{
    public class OnStartEventArgs
    {
        public string Question { get; set; }
    }
    public class OnCompletedEventArgs
    {
        public string Question { get; set; }

        public string PageSource { get; set; }
        public long Milliseconds { get; set; }
    }

    public class OnErrorEventArgs
    {
        public string Question { get; set; }
        public Exception Error { get; set; }
    }
}