using RestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Middleware
{
    public interface ICacheLayer
    {
        void Add(Staff staff);
        void Remove(int id);
        bool TryGet(int id, out Staff staff);
    }
}
