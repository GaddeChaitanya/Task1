using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskNgTool.Models
{
    public class TaskTable
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string UserDuration { get; set; }
        public string SystemDuration { get; set; }
    }
}