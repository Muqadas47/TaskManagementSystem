using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


public class TaskManagementDBContext : DbContext
{
    public TaskManagementDBContext() : base("name=TaskManagementDBContext")
    {
    }

    public DbSet<User> Users { get; set; }
}

public class User
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public DateTime? SignupDate { get; internal set; }
    public bool IsBlocked { get; internal set; }
    public bool IsDeleted { get; internal set; }
    
}
