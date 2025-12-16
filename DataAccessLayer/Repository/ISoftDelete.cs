using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class Users : ISoftDelete
    {
        public bool IsDelete { get; set; }
    }

    public class Peoples : ISoftDelete
    {
        public bool IsDelete { get; set; }
    }

    public class Group_Users : ISoftDelete
    {
        public bool IsDelete { get; set; }
    }
    public class Access_Level : ISoftDelete
    {
        public bool IsDelete { get; set; }
    }
}
