using Microsoft.EntityFrameworkCore;
using SmartTaskReminder.Application.Dtos.TaskItems.Responses;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Application.Paginations;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Infrastructure.Context;
using SmartTaskReminder.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace SmartTaskReminder.Infrastructure.Repositories
{
    public class TaskItemRepository : GenericRepositoryAsync<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<PagedResult<TaskItem>> GetAllTasksByUserAsync(int userId, int pageNumber, int pageSize)

        {
            var query = _dbContext.TaskItems.Where(t => t.UserId == userId);
            var totalCount = await query.CountAsync();
            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var metadata = new PaginationMetaData(
                pageSize, pageNumber, totalCount);

            return new PagedResult<TaskItem>(tasks, metadata);
        }

        public async Task<PagedResult<TaskItem>> FilterByDateAsync(int userId, DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            var endDateValue = endDate.Date.AddDays(1).AddTicks(-1); 
            var query = _dbContext.TaskItems
                .Where(t =>
                    t.UserId == userId &&
                    t.DueDate.HasValue &&
                    t.DueDate.Value >= startDate &&
                    t.DueDate.Value <= endDateValue);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var metadata = new PaginationMetaData(
                pageSize, pageNumber, totalCount);

            return new PagedResult<TaskItem>(tasks, metadata);
        }

        public async Task<PagedResult<TaskItem>> FilterByPriorityAsync(int userId, TaskItemPriority priority, int pageNumber, int pageSize)
        {
            var query = _dbContext.TaskItems
                .Where(t =>
                    t.UserId == userId &&
                    t.Priority == priority);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var metadata = new PaginationMetaData(pageSize, pageNumber, totalCount);


            return new PagedResult<TaskItem>(tasks, metadata);
        }

        public async Task<PagedResult<TaskItem>> FilterByStatusAsync(int userId, TaskItemStatus status, int pageNumber, int pageSize)
        {
            var query = _dbContext.TaskItems
                .Where(t =>
                    t.UserId == userId &&
                    t.Status == status);
            var totalCount = await query.CountAsync();
            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var metadata = new PaginationMetaData(pageSize, pageNumber, totalCount);
            return new PagedResult<TaskItem>(tasks, metadata);
        }
    }
    }
