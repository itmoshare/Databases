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
            using(var redis = redisclient.GetClient()) 
            {
                redis.Set(staff.Id.ToString(), JsonConvert.Serialize<Staff>(staff));
                var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                redis.Set($"t{staff.Id}", unixTimestamp.ToString());
            }
        }

        public void Remove(int id)
        {
            using(var redis = redisclient.GetClient())
            {
                if(!redis.ContainsKey(id.ToString()))
                    return;
                throw new NotImplementedException();//TODO
            }
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
