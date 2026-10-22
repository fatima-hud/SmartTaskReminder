using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Infrastructure.Context;
using SmartTaskReminder.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Infrastructure.Seed
{
    public static class AdminSeeder
    {
      
        

        public static async Task SeedDataAsync(IUserRepository userRepository,IRoleRepository roleRepository , IPasswordHasherService passwordHasher,IConfiguration configuration)
        {
           
            string[] roles = { "Admin", "User"};

            foreach (var role in roles)
            {
                if (await roleRepository.GetRoleIdByNameAsync(role) == null)
                {
                    await roleRepository.AddAsync(new Role { Name = role });
                }
            }

            var adminEmail = configuration["SeedAdmin:Email"];
            var adminUserName = configuration["SeedAdmin:UserName"];
            var adminPassword = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminUserName) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException("SeedAdmin configuration is incomplete.");
                    
            }

        
            var existingAdmin =await userRepository.GetByEmailAsync(adminEmail);
                

            if (existingAdmin != null)
                return;

           
            var adminRoleId = await roleRepository.GetRoleIdByNameAsync("Admin");
               

            if (adminRoleId == null)
            {
                throw new InvalidOperationException(
                    "Admin role could not be created.");
            }

            
            var admin = new User
            {
                UserName = adminUserName,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                Gender=Gender.Male,
                BirthDate = new DateTime(1990, 1, 1),
                RoleId = adminRoleId.Value
            };

            
            admin.PasswordHash = await passwordHasher.HashPassword(admin, adminPassword);
               

            await userRepository.AddAsync(admin);
           
        }


    }
    }

