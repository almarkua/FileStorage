using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FileStorage.Domain
{
    public class FileStorageContextInitializer : DropCreateDatabaseIfModelChanges<FileStorageContext>
    {
        protected override void Seed(FileStorageContext context)
        {

            context.Users.Add(new ApplicationUser()
            {
                UserName = "admin",
                PasswordHash = "admin",
                Email = "admin@admin.mail",
                TotalSpace = (int)Math.Pow(1024,3)
            });
            base.Seed(context);
        }
    }
}