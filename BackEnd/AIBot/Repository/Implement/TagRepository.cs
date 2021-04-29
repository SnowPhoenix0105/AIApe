using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Repository.Exceptions;

namespace Buaa.AIBot.Repository.Implement
{
    /// <summary>
    /// Implement of <see cref="ITagRepostory"/>
    /// </summary>
    /// <remarks><seealso cref="ITagRepostory"/></remarks>
    public class TagRepository : RepositoryBase, ITagRepostory
    {
        public TagRepository(DatabaseContext context) : base(context) { }

        private volatile bool changed = true;
        private Dictionary<string, int> cache = null;

        public async Task<Dictionary<string, int>> SelectAllTagsAsync()
        {
            if (!changed)
            {
                return cache;
            }
            var query = await Context
                 .Tags
                 .AsQueryable()
                 .Select(t => new KeyValuePair<string, int>(t.Name, t.TagId))
                 .ToListAsync();
            cache = new Dictionary<string, int>(query);
            changed = false;
            return cache;
        }

        public async Task<TagInfo> SelectTagByIdAsync(int tagId)
        {
            var query = await Context
                .Tags
                .Select(t => new TagInfo()
                {
                    TagId = t.TagId,
                    Name = t.Name,
                    Desc = t.Desc
                })
                .FirstOrDefaultAsync();
            return query;
        }

        private async Task CheckTagName(string tagName)
        {
            if ((await Context
                .Tags
                .AsQueryable()
                .Where(t => t.Name == tagName)
                .SingleOrDefaultAsync()) != null)
            {
                throw new TagNameHasExistException(tagName);
            }
        }

        public async Task<int> InsertTagAsync(TagInfo tag)
        {
            if (tag.Name == null)
            {
                throw new ArgumentNullException(nameof(tag.Name));
            }
            if (tag.Desc == null)
            {
                throw new ArgumentNullException(nameof(tag.Desc));
            }
            if (tag.Name.Length > Constants.TagNameMaxLength)
            {
                throw new TagNameToLongException(tag.Name.Length, Constants.TagNameMaxLength);
            }
            var target = new TagData()
            {
                Name = tag.Name,
                Desc = tag.Desc
            };
            await CheckTagName(tag.Name);
            Context.Add(target);
            bool success = false;
            while (!success)
            {
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckTagName(tag.Name);
            }
            return target.TagId;
        }

        public async Task UpdateTagAsync(TagInfo tag)
        {
            bool success = true;
            var target = await Context.Tags.FindAsync(tag.TagId);
            if (tag.Name != null)
            {
                if (tag.Name.Length > Constants.TagNameMaxLength)
                {
                    throw new TagNameToLongException(tag.Name.Length, Constants.TagNameMaxLength);
                }
                success = false;
                target.Name = tag.Name;
            }
            if (tag.Desc == null)
            {
                throw new ArgumentNullException(nameof(tag.Desc));
            }
            await CheckTagName(tag.Name);
            while (!success)
            {
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckTagName(tag.Name);
            }
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var target = await Context.Tags.FindAsync(tagId);
            if (target != null)
            {
                Context.Tags.Remove(target);
                await SaveChangesAgainAndAgainAsync();
            }
        }
    }
}
