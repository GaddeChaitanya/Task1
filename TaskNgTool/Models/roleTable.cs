using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskNgTool.Models
{
    public class RoleTable
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<TaskTable> TaskDetails { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
    }
}