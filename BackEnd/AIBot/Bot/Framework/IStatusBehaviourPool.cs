using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.Framework
{
    public interface IStatusBehaviourPool<IdType>
    {
        IBotStatusBehaviour<IdType> this[IdType id] { get; }
    }

    public class StatusBehaviourPool<IdType> : IStatusBehaviourPool<IdType>
    {
        public IBotStatusBehaviour<IdType> this[IdType id] { get => pool[id]; }

        private Dictionary<IdType, IBotStatusBehaviour<IdType>> pool;

        public StatusBehaviourPool(Dictionary<IdType, IBotStatusBehaviour<IdType>> pool)
        {
            this.pool = pool;
        }
}
}
