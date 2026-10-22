using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Core.Entities
{
    public class Role : BaseEntity
    {
      
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
