﻿using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role
                }).ToListAsync();
        }
        public async Task<UserViewModel?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role
                }).FirstOrDefaultAsync();
        }
        public async Task<bool> CreateAsync(CreateUserViewModel model)
        {
            var emailExists= await _context.Users.AnyAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (emailExists)
            {
                return false;
            }
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EditUserViewModel?> GetEditByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new EditUserViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task UpdateAsync(EditUserViewModel model)
        {
            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null) return;

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Role = model.Role;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
