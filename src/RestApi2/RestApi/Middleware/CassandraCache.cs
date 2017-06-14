using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.Model;

namespace RestApi.Middleware
{
    public class CassandraCache : ICacheLayer
    {
        private ICluster _cassandraCluster;

        public CassandraCache(ICluster cassandraCluster)
        {
            _cassandraCluster = cassandraCluster;
        }

        public void Add(Staff staff)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(int id, out Staff staff)
        {
            using (var session = _cassandraCluster.Connect())
            {
                Row res = session.Execute($"select * from staff where id = {id};").FirstOrDefault();
                if (res != null)
                {
                    staff = new Staff
                    {
                        Id = res.GetValue<int>("Id"),
                        FirstName = res.GetValue<string>("FirstName"),
                        LastName = res.GetValue<string>("LastName"),
                        Birthday = res.GetValue<DateTime>("Birthday"),
                        Position = res.GetValue<string>("Position"),
                        Salary = res.GetValue<decimal>("Salary")
                    };
                    return true;
                }
                staff = null;
                return false;
            }
        }
    }
}
