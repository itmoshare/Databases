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
        private const string KeySpace = "db";

        public CassandraCache(ICluster cassandraCluster)
        {
            _cassandraCluster = cassandraCluster;
        }

        public void Add(Staff staff)
        {
            using (var session = _cassandraCluster.Connect(KeySpace))
            {
                session.Execute($"insert into staff values({staff.Id}, {staff.FirstName}, {staff.LastName}, {staff.Birthday}, {staff.Position}, {staff.Salary});");
                var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                session.Execute($"insert into lifetime values({staff.Id}, {unixTimestamp.ToString()});");
            }
        }

        public void Remove(int id)
        {
            using (var session = _cassandraCluster.Connect(KeySpace))
            {
                session.Execute($"delete from staff where Id = {id};");
                session.Execute($"delete from lifetime where staff_Id = {id};");
            }
        }

        public bool TryGet(int id, out Staff staff)
        {
            using (var session = _cassandraCluster.Connect(KeySpace))
            {
                Row res = session.Execute($"select * from staff where Id = {id};").FirstOrDefault();
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
