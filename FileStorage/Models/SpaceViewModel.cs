namespace FileStorage.Models
{
    public class SpaceViewModel
    {
        public string TotalSpace { get; set; }
        public string UsedSpace { get; set; }
        public string FreeSpace { get; set; }

        public int UsedSpacePercent { get; set; }

        public string Progress => (UsedSpacePercent > 10) ? UsedSpace+"/"+TotalSpace : UsedSpacePercent+"%";
    }
}