﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Repository.Models;

namespace Buaa.AIBot.Repository.Implement
{
    public class RepositoryBase
    {
        protected DatabaseContext Context { get; }

        public RepositoryBase(DatabaseContext context)
        {
            Context = context;
        }

        protected async Task SaveChangesAgainAndAgainAsync()
        {
            bool saved = false;
            while (!saved)
            {
                try
                {
                    await Context.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    foreach (var entry in e.Entries)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            proposedValues[property] = proposedValues[property];
                        }
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }
        }

        protected async Task<bool> TrySaveChangesAgainAndAgainAsync()
        {
            try
            {
                await SaveChangesAgainAndAgainAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        protected async Task TrySaveChangesAgainAndAgainAsync(Action<DbUpdateException> onException)
        {
            try
            {
                await SaveChangesAgainAndAgainAsync();
            }
            catch (DbUpdateException e)
            {
                onException(e);
            }
        }

        protected async Task<TResult> TrySaveChangesAgainAndAgainAsync<TResult>(TResult onSuccess, Func<DbUpdateException, TResult> onException)
        {
            try
            {
                await SaveChangesAgainAndAgainAsync();
                return onSuccess;
            }
            catch (DbUpdateException e)
            {
                return onException(e);
            }
        }
    }
}
