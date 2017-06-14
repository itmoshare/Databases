using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.Model;
using Newtonsoft.Json;

namespace RestApi.Middleware
{
    public class RedisCache : ICacheLayer
    {
        private IRedisClientsManager _redisclient;

        public RedisCache(IRedisClientsManager redisclient)
        {
            _redisclient = redisclient;
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
            using (var redis = _redisclient.GetClient())
            {
                if (redis.ContainsKey(id.ToString()))
                {
                    staff = JsonConvert.DeserializeObject<Staff>(redis.Get<string>(id.ToString()));
                    return true;
                }
                staff = null;
                return false;
            }
        }
    }
}
