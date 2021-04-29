using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Buaa.AIBot.Bot;
using System.Text.Json;

namespace Buaa.AIBot.Bot.Framework
{
    public interface IStatusPool<IdType>
    {
        Task<BotStatus<IdType>> GetStatusAsync(int sessionId);
        Task SaveStatusAsync(int sessionId, BotStatus<IdType> status);
    }

    public class StatusPoolInMemory<IdType> : IStatusPool<IdType>
    {
        private Dictionary<int, Node> memory = new Dictionary<int, Node>();
        private readonly TimeSpan liveTime = TimeSpan.FromMinutes(10);
        private readonly int gcCount = 128;
        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        private class Node
        {
            public DateTime VisitTime { get; set; }
            public string Json { get; set; }
        }

        private void GarbageCollection()
        {
            var tmp = new Dictionary<int, Node>();
            foreach (var pair in memory)
            {
                var span = DateTime.Now - pair.Value.VisitTime;
                if (span < liveTime)
                {
                    tmp.Add(pair.Key, pair.Value);
                }
            }
            memory = tmp;
        }

        public async Task<BotStatus<IdType>> GetStatusAsync(int sessionId)
        {
            await gate.WaitAsync();
            try
            {
                if (memory.TryGetValue(sessionId, out Node value))
                {
                    BotStatus<IdType> ret = JsonSerializer.Deserialize<BotStatus<IdType>>(value.Json);
                    value.VisitTime = DateTime.Now;
                    return ret;
                }
                return null;
            }
            finally
            {
                gate.Release();
            }
        }

        public async Task SaveStatusAsync(int sessionId, BotStatus<IdType> status)
        {
            await gate.WaitAsync();
            try
            {
                string json = JsonSerializer.Serialize(status);
                if (memory.TryGetValue(sessionId, out Node value))
                {
                    value.Json = json;
                    value.VisitTime = DateTime.Now;
                    return;
                }
                if (memory.Count > gcCount)
                {
                    GarbageCollection();
                }
                value = new Node()
                {
                    Json = json,
                    VisitTime = DateTime.Now
                };
                memory.Add(sessionId, value);
            }
            finally
            {
                gate.Release();
            }
        }
    }
}
