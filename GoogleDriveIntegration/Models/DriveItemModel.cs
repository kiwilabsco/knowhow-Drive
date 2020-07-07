using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDriveIntegration.Models
{
    public class DriveItemModel
    {
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public string itemID { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public string sharableLink { get; set; }
        public string fileId { get; set; }


    }
}
