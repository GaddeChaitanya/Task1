using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaskNgTool.Controllers
{
    public class ValuesController : ApiController
    {
        private Ptg_Task_TrackerEntities db = new Ptg_Task_TrackerEntities();
        // GET api/values
        public int Get()
        {
            int count = 0;
            try
            {
                count = db.tasks.Max(t => t.TaskID);
                Console.WriteLine(count);
                return count;
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            string val = "Un Authorized user";
            string user = Environment.UserName;
            try
            {
                var res = db.users;
                foreach (var item in res)
                {
                    if (user.Equals(item.UserName))
                        val = item.UserName;
                }
            }
            catch(Exception ex)
            {
                return val;
            }            
            return val;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
