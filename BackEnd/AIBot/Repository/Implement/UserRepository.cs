﻿using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Buaa.AIBot.Repository.Implement
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base (context) { }

        public async Task<UserInfo> SelectUserByIdAsync(int userId)
        {

            var user = await Context
                .Users
                .Select(u => new UserInfo()
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Bcrypt = u.Bcrypt,
                    Name = u.Name,
                    Auth = u.Auth
                })
                .SingleOrDefaultAsync(user => user.UserId == userId);
            return user;
        }

        public async Task<UserInfo> SelectUserByEmailAsync(string email)
        {
            var user = await Context
                .Users
                .Select(u => new UserInfo()
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Bcrypt = u.Bcrypt,
                    Name = u.Name,
                    Auth = u.Auth
                })
                .SingleOrDefaultAsync(user => user.Email == email);
            return user;
        }

        public async Task<UserInfo> SelectUserByNameAsync(string name)
        {
            var user = await Context
                .Users
                .Select(u => new UserInfo()
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Bcrypt = u.Bcrypt,
                    Name = u.Name,
                    Auth = u.Auth
                })
                .SingleOrDefaultAsync(user => user.Name == name);
            return user;
        }

        public async Task<string> SelectBcryptByEmailAsync(string email)
        {
            var user = await Context
                .Users
                .Select(user => new { user.Email, user.Bcrypt })
                .SingleOrDefaultAsync(user => user.Email == email);
            if (user == null)
            {
                return null;
            }
            return user.Bcrypt;
        }

        public async Task<IEnumerable<int>> SelectAnswersIdByIdAsync(int userId)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var query = user
                .Answers
                .Select(answer => answer.AnswerId);
            return query.ToList();
        }

        public async Task<IEnumerable<int>> SelectAnswersIdByIdByModifyTimeAsync(int userId)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var query = user
                .Answers
                .OrderByDescending(a => a.ModifyTime)
                .Select(a => a.AnswerId);
            return query.ToList();
        }

        public async Task<IEnumerable<int>> SelectQuestionsIdByIdAsync(int userId)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var query = user
                .Questions
                .Select(q => q.QuestionId);
            return query.ToList();
            // return await Task.Run(() => query.ToList());
        }

        public async Task<IEnumerable<int>> SelectQuestionsIdByIdOrderByModifyTimeAsync(int userId)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var query = user
                .Questions
                .OrderByDescending(a => a.ModifyTime)
                .Select(q => q.QuestionId);
            return query.ToList();
        }

        private async Task CheckInsert(UserInfo user)
        {
            var dup = await Context
                .Users
                .Select(u => new { u.Name, u.Email })
                .FirstOrDefaultAsync(u => u.Name == user.Name || u.Email == user.Email);
            if (dup != null)
            {
                if (dup.Email == user.Email)
                {
                    throw new EmailHasExistException(user.Email);
                }
                throw new NameHasExistException(user.Name);
            }
        }

        public async Task<int> InsertUserAsync(UserInfo user)
        {
            if (user.Bcrypt == null)
            {
                throw new ArgumentNullException(nameof(user.Bcrypt));
            }
            if (user.Name == null)
            {
                throw new ArgumentNullException(nameof(user.Name));
            }
            if (user.Auth == AuthLevel.None)
            {
                throw new ArgumentNullException(nameof(user.Auth));
            }
            if (user.Email.Length > Constants.UserEmailMaxLength)
            {
                throw new UserEmailToLongException(user.Email.Length, Constants.UserEmailMaxLength);
            }
            if (user.Name.Length > Constants.UserNameMaxLength)
            {
                throw new UserNameToLongException(user.Name.Length, Constants.UserNameMaxLength);
            }
            if (user.Bcrypt.Length != Constants.UserBcryptLength)
            {
                throw new UserBycryptLengthException(user.Bcrypt.Length, Constants.UserBcryptLength);
            }
            var target = new UserData()
            {
                Email = user.Email,
                Bcrypt = user.Bcrypt,
                Name = user.Name,
                Auth = user.Auth
            };
            await CheckInsert(user);
            bool success = false;
            Context.Users.Add(target);
            while (!success)
            {
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckInsert(user);
            }
            return target.UserId;
        }

        public async Task UpdateUserAsync(UserInfo user)
        {
            var target = await Context.Users.FindAsync(user.UserId);
            if (target == null)
            {
                throw new UserNotExistException(user.UserId);
            }
            bool success = true;
            if (user.Bcrypt != null)
            {
                success = false;
                target.Bcrypt = user.Bcrypt;
                if (user.Bcrypt.Length != Constants.UserBcryptLength)
                {
                    throw new UserBycryptLengthException(user.Bcrypt.Length, Constants.UserBcryptLength);
                }
            }
            if (user.Name != null)
            {
                success = false;
                target.Name = user.Name;
                if (user.Name.Length > Constants.UserNameMaxLength)
                {
                    throw new UserNameToLongException(user.Name.Length, Constants.UserNameMaxLength);
                }
            }
            if (user.Auth != AuthLevel.None)
            {
                success = false;
                target.Auth = user.Auth;
            }
            while (!success)
            {
                if (user.Name != null && (
                    await Context.Users.FirstOrDefaultAsync(u => u.Name == user.Name)) != null)
                {
                    throw new NameHasExistException(user.Name);
                }
                if ((await Context.Users.FindAsync(user.UserId)) == null)
                {
                    throw new UserNotExistException(user.UserId);
                }
                success = await TrySaveChangesAgainAndAgainAsync();
            }
        }

        public async Task DeleteUserByIdAsync(int userId)
        {
            var target = await Context.Users.FindAsync(userId);
            if (target != null)
            {
                Context.Users.Remove(target);
                await SaveChangesAgainAndAgainAsync();
            }
        }
    }
}
