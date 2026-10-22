using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Infrastructure.InfrastructureBases
{
    public class UnitOfWork:IUnitOfWork
    {
        protected ApplicationDbContext _dbContext { get; set; }
        private IDbContextTransaction? _transaction;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();

        }

        public async Task RollBackAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();

        }
    }
}
