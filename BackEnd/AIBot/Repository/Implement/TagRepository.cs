using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Repository.Exceptions;
using Buaa.AIBot.Utils;

namespace Buaa.AIBot.Repository.Implement
{
    /// <summary>
    /// Implement of <see cref="ITagRepostory"/>
    /// </summary>
    /// <remarks><seealso cref="ITagRepostory"/></remarks>
    public class TagRepository : RepositoryBase, ITagRepostory
    {
        private class CachedData
        {
            public bool tagListChanged = true;
            public bool tagCategoryChanged = true;
            public Dictionary<string, int> cachedList;
            public IReadOnlyDictionary<TagCategory, IReadOnlyDictionary<int, string>> cachedCategory;
            public SemaphoreSlim listLock = new SemaphoreSlim(1);
            public SemaphoreSlim categoryLock = new SemaphoreSlim(1);
        }

        public TagRepository(DatabaseContext context, ICachePool<int> cachePool, GlobalCancellationTokenSource globalCancellationTokenSource)
            : base(context, cachePool, globalCancellationTokenSource.Token) 
        {
            var cached = cachePool.GetOrDefault<CachedData>(CacheId.Tag);
            if (cached == null)
            {
                using (cachePool.LockAsync(CacheId.Tag))
                {
                    cached = cachePool.GetOrDefault<CachedData>(CacheId.Tag);
                    if (cached == null)
                    {
                        cached = new CachedData();
                        cachePool.Set(CacheId.Tag, cached);
                    }
                }
            }
            sharedData = cached;
        }

        private readonly CachedData sharedData;

        public async Task<IReadOnlyDictionary<string, int>> SelectAllTagsAsync()
        {
            Dictionary<string, int> ret;
            if (sharedData.tagListChanged)
            {
                await sharedData.listLock.WaitAsync();
                try
                {
                    if (sharedData.tagListChanged)
                    {
                        var query = await Context
                             .Tags
                             // .AsQueryable()
                             .Select(t => new KeyValuePair<string, int>(t.Name, t.TagId))
                             .ToListAsync(CancellationToken);
                        CancellationToken.ThrowIfCancellationRequested();
                        var res = new Dictionary<string, int>(query);
                        sharedData.cachedList = res;
                        sharedData.tagListChanged = false;
                        ret = res;
                    }
                    else
                    {
                        ret = sharedData.cachedList;
                    }
                }
                finally
                {
                    sharedData.listLock.Release();
                }
            }
            else
            {
                ret = sharedData.cachedList;
            }
            return ret;
        }

        public async Task<IReadOnlyDictionary<TagCategory, IReadOnlyDictionary<int, string>>> SelectAllTagsCategorysAsync()
        {
            IReadOnlyDictionary<TagCategory, IReadOnlyDictionary<int, string>> ret;
            if (sharedData.tagCategoryChanged)
            {
                await sharedData.categoryLock.WaitAsync();
                try
                {
                    if (sharedData.tagCategoryChanged)
                    {
                        var tags = await Context.Tags.ToListAsync();
                        CancellationToken.ThrowIfCancellationRequested();
                        var query = from tag in tags
                                    group tag by tag.Category into groups
                                    select new KeyValuePair<TagCategory, IReadOnlyDictionary<int, string>>(
                                        (TagCategory)groups.Key, 
                                        new Dictionary<int, string>(groups.Select(t => new KeyValuePair<int, string>(t.TagId, t.Name))));
                        var dict = new Dictionary<TagCategory, IReadOnlyDictionary<int, string>>(query);
                        sharedData.cachedCategory = dict;
                        sharedData.tagCategoryChanged = false;
                        ret = dict;
                    }
                    else
                    {
                        ret = sharedData.cachedCategory;
                    }
                }
                finally
                {
                    sharedData.categoryLock.Release();
                }
            }
            else
            {
                ret = sharedData.cachedCategory;
            }
            return ret;
        }

        public async Task<TagInfo> SelectTagByIdAsync(int tagId)
        {
            var query = await Context
                .Tags
                .Select(t => new TagInfo()
                {
                    TagId = t.TagId,
                    Category = (TagCategory)t.Category,
                    Name = t.Name,
                    Desc = t.Desc
                })
                .Where(t => t.TagId == tagId)
                .FirstOrDefaultAsync(CancellationToken);
            CancellationToken.ThrowIfCancellationRequested();
            return query;
        }

        private async Task CheckTagName(string tagName)
        {
            var old = await Context
                .Tags
                .AsQueryable()
                .Where(t => t.Name == tagName)
                .SingleOrDefaultAsync(CancellationToken);
            CancellationToken.ThrowIfCancellationRequested();
            if (old != null)
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
            if (tag.Category == TagCategory.None)
            {
                throw new ArgumentNullException(nameof(tag.Category));
            }
            if (tag.Name.Length > Constants.TagNameMaxLength)
            {
                throw new TagNameTooLongException(tag.Name.Length, Constants.TagNameMaxLength);
            }
            var target = new TagData()
            {
                Category = (int)tag.Category,
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
            sharedData.tagListChanged = true;
            sharedData.tagCategoryChanged = true;
            return target.TagId;
        }

        public async Task UpdateTagAsync(TagInfo tag)
        {
            bool success = true;
            var target = await Context.Tags.FindAsync(tag.TagId);
            if (target == null)
            {
                throw new TagNotExistException(tag.TagId);
            }
            if (tag.Name != null)
            {
                if (tag.Name.Length > Constants.TagNameMaxLength)
                {
                    throw new TagNameTooLongException(tag.Name.Length, Constants.TagNameMaxLength);
                }
                success = false;
                target.Name = tag.Name;
            }
            if (tag.Desc != null)
            {
                success = false;
                target.Desc = tag.Desc;
            }
            if (tag.Category != TagCategory.None)
            {
                target.Category = (int)tag.Category;
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
            sharedData.tagListChanged = true;
            sharedData.tagCategoryChanged = true;
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var target = await Context.Tags.FindAsync(tagId);
            if (target != null)
            {
                Context.Tags.Remove(target);
                await SaveChangesAgainAndAgainAsync();
                sharedData.tagListChanged = true;
                sharedData.tagCategoryChanged = true;
            }
        }
    }
}
