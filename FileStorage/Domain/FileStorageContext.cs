using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FileStorage.Domain
{
    public class FileStorageContext: IdentityDbContext<ApplicationUser>
    {
        //IdentityDbContext contains Users, Roles etc.
        public DbSet<File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }

        public FileStorageContext() : base("DebugConnection")
        {
            Database.SetInitializer<FileStorageContext>(new FileStorageContextInitializer());
        }

        public static FileStorageContext Create()
        {
            return new FileStorageContext();
        }
    }
}