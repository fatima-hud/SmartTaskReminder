using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();
    }
}
