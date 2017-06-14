using RestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Middleware
{
    public interface IStaffDriver
    {
        IEnumerable<Staff> ListAll();
        Staff Get(int id);
        void Add(Staff staff);
        void Update(int id, Staff staff);
        void Delete(int id);
    }
}
