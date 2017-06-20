using Cassandra;
using Newtonsoft.Json;
using RestApi.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Middleware
{
    public class DbDriver : IStaffDriver
    {

        public const string CassandraKeySpace = "db";
        private IRedisClientsManager _redisclient;
        private ICluster _cassandraCluster;
        private MySqlContext _mySqlContext;

        private ICacheLayer _firstCache;
        private ICacheLayer _middleCache;

        public DbDriver(IRedisClientsManager redisClient, ICluster cassandraCluster, MySqlContext mySqlContext)
        {
            _firstCache = new RedisCache(redisClient);
            _middleCache = new CassandraCache(cassandraCluster);
            _mySqlContext = mySqlContext;
        }

        public IEnumerable<Staff> ListAll()
        {
            return _mySqlContext.staff.ToList();
        }

        public Staff Get(int id)
        {
            if (_firstCache.TryGet(id, out Staff staff))
                return staff;

            if (_middleCache.TryGet(id, out staff))
            {
                _firstCache.Add(staff);
                return staff;
            }
            staff = _mySqlContext.staff.Find(id);
            if (staff != null)
            {
                _firstCache.Add(staff);
                _middleCache.Add(staff);
            }
            return staff;
        }

        public void Add(Staff staff)
        {
            _mySqlContext.staff.Add(staff);
            _mySqlContext.SaveChanges();
            //add to one cache level
            _middleCache.Add(staff); 
        }

        public void Delete(int id)
        {
            _mySqlContext.staff.Remove(new Staff { Id = id });
            _mySqlContext.SaveChanges();
            _firstCache.Remove(id);
            _middleCache.Remove(id);
        }

        public void Update(int id, Staff staff)
        {
            throw new NotImplementedException();
        }



    }
}
