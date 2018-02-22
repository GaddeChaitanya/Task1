using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskNgTool.Models;

namespace TaskNgTool.Controllers
{
    public class TasksController : ApiController
    {
       // private Ptg_Task_TrackerEntities db = new Ptg_Task_TrackerEntities();
        // GET api/Tasks
       //{
        //    int UserID = 0;
        //    string user = Environment.UserName;
        //    var res = db.users;
        //    foreach (var item in res)
        //    {
        //        if (user.Equals(item.UserName))
        //            UserID = item.UserID;
        //    }
        //    return UserID;
        //}
        public int GetUserID()
        {
            int UserID = 0;
            string user = Environment.UserName;
            using (var db = new Ptg_Task_TrackerEntities())
            {
                var res = db.users;
                foreach (var item in res)
                {
                    if (user.Equals(item.UserName))
                        UserID = item.UserID;
                }
            }           
            return UserID;
        }
        [ResponseType(typeof(position))]
        public IHttpActionResult GetPositionByID(int id)
        {
            RoleTable pt = new RoleTable();
            int i = 0, uid = 0,pid=0;
            List<RoleTable> rt = new List<RoleTable>();
            List<TaskTable> tt = null;
            RoleTable t = null;
            TaskTable tbTask = null;
            List<TaskTable> taskRec;
            string user = Environment.UserName;            
            try
            {
                using (var db = new Ptg_Task_TrackerEntities())
                {
                    var res = db.users;
                    foreach (var item in res)
                    {
                        if (user.Equals(item.UserName))
                            pid = Convert.ToInt32(item.PositionID);
                    }
                    uid = GetUserID();
                    var x = db.positions.Find(pid);
                    foreach (var item in x.roles)
                    {
                        t = new RoleTable();
                        t.RoleID = item.RoleID;
                        t.RoleName = item.RoleName;
                        t.Description = item.Description;
                        t.PositionID = Convert.ToInt32(item.PositionID);
                        t.PositionName = x.PositionName;
                        i = 0;
                        tt = new List<TaskTable>();
                        foreach (var ta in item.tasks)
                        {
                            //if(ta.taskrecords.Count <= 0)
                            //{
                            t.TaskDetails = null;
                            tbTask = new TaskTable();
                            tbTask.TaskName = ta.TaskName;
                            tbTask.Description = ta.Description;
                            tbTask.TaskID = ta.TaskID;
                            foreach (var taskr in ta.taskrecords)
                            {
                                if (taskr.UserID == uid && taskr.TaskID == tbTask.TaskID)
                                {
                                    tbTask.UserDuration = taskr.enterdDuration;
                                    tbTask.SystemDuration = taskr.Duration;
                                }
                            }
                            tt.Add(tbTask);
                            i++;

                        }
                        t.TaskDetails = tt;
                        rt.Add(t);
                    }
                    //taskRec = new List<int>();
                    taskRec = new List<TaskTable>();
                    foreach (var item in rt)
                    {
                        foreach (var r in item.TaskDetails)
                        {
                            if ((r.UserDuration != null) || (r.SystemDuration != null))
                            {
                                taskRec.Add(r);
                            //    //taskRec.Add(item.TaskDetails.IndexOf(r));
                            //    taskRec.Add(r.TaskID);
                            }
                        }
                        if (taskRec.Count > 0)
                        {
                            foreach (var r in taskRec)
                            {
                                item.TaskDetails.Contains(r);
                                item.TaskDetails.Remove(r);
                            }               
                        //    int remove = 0,k=1;
                        //    foreach (var r in taskRec)
                        //    {
                        //        foreach (var re in item.TaskDetails)
                        //        {
                        //            if(re.TaskID == r)
                        //            {
                        //                item.TaskDetails.Remove(re);
                        //            }
                        //        }
                        //        //if (remove == 0)
                        //        //{
                        //        //    item.TaskDetails.RemoveAt(r);
                        //        //    remove = 1;
                        //        //}
                        //        //else
                        //        //{
                        //        //    remove = r - k;
                        //        //    item.TaskDetails.RemoveAt(remove);
                        //        //    k++;
                        //        //}
                        //    }
                            taskRec.Clear();
                        }
                    }
                    return Ok(rt);
                }                
            }
            catch (Exception ex)
            {
                return Ok(rt);
            }
        }

        // POST: api/Tasks
        [ResponseType(typeof(RoleTable))]
        public IHttpActionResult PostPosition(RoleTable roleDetails)
        {
            taskrecord taskrd = new taskrecord();
            task taskVar = new task();
            int uid = 0;
            uid = GetUserID();
            try
            {
                using (var db = new Ptg_Task_TrackerEntities())
                {
                    if ((db.tasks.Max(t => t.TaskID)) < roleDetails.TaskDetails[0].TaskID)
                    {
                        taskVar.TaskName = roleDetails.TaskDetails[0].TaskName;
                        taskVar.RoleID = roleDetails.RoleID;
                        taskVar.Description = roleDetails.TaskDetails[0].Description;
                        taskVar.TaskStatus = "created";
                        taskVar.CreatedDate = DateTime.Now.Date;
                        taskVar.CreatedBy = Environment.UserName;
                        taskVar.CustomTaskUserID = uid;
                        db.tasks.Add(taskVar);
                        db.SaveChanges();
                    }
                    taskrd.UserID = uid;
                    taskrd.TaskName = roleDetails.TaskDetails[0].TaskName;
                    taskrd.Description = roleDetails.TaskDetails[0].Description;
                    taskrd.Duration = roleDetails.TaskDetails[0].SystemDuration;
                    taskrd.TaskStatus = "submitted";
                    taskrd.CreatedDate = Convert.ToDateTime(DateTime.Now);
                    taskrd.CreatedBy = Environment.UserName;
                    taskrd.PositionID = roleDetails.PositionID;
                    taskrd.RoleID = roleDetails.RoleID;
                    taskrd.enterdDuration = roleDetails.TaskDetails[0].UserDuration;
                    taskrd.TaskID = roleDetails.TaskDetails[0].TaskID;
                    db.taskrecords.Add(taskrd);
                    db.SaveChanges();
                    return CreatedAtRoute("DefaultApi", new { id = roleDetails.PositionID }, roleDetails);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // PUT api/Tasks/5
        [ResponseType(typeof(CustomTable))]
        public IHttpActionResult Put(int id, CustomTable ct)
        {
            int uid = GetUserID();
            task taskVar = new task();
            try
            {
                using (var db = new Ptg_Task_TrackerEntities())
                {                    
                    taskVar.TaskName = ct.TaskName;
                    taskVar.RoleID = ct.RoleID;
                    taskVar.Description = ct.TaskDescription;
                    taskVar.TaskStatus = "c";
                    taskVar.CreatedDate = DateTime.Now.Date;
                    taskVar.CreatedBy = Environment.UserName;
                    taskVar.CustomTaskUserID = uid;
                    db.tasks.Add(taskVar);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
            return CreatedAtRoute("DefaultApi", new { id = taskVar.TaskID }, taskVar);
        }
    }
}
