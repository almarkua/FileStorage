using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FileStorage.Domain
{
    public class File
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public bool IsShared { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }

        public static string GetSizeString(long size)
        {
            //byte
            if (size > 1024)
            {
                //kb
                if (size > Math.Pow(1024, 2))
                {
                    //mb
                    if (size > Math.Pow(1024, 3))
                    {
                        return Math.Round(size / Math.Pow(1024, 3), 2) + "GB";
                    }
                    return Math.Round(size / Math.Pow(1024, 2), 2) + "MB";
                }
                return Math.Round((double)size / 1024, 2) + "KB";
            }
            return size + "B";
        }
    }
}