using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Repository.Implement
{
    public class TagRepository : ITagRepostory
    {
        public Task DeleteTagAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertTagAsync(TagInfo tag)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> SelectAllTags()
        {
            throw new NotImplementedException();
        }

        public Task<TagInfo> SelectTagByIdAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTagAsync(TagInfo tag)
        {
            throw new NotImplementedException();
        }
    }
}
