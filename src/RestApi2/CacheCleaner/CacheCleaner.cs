﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using ServiceStack.Redis;

namespace CacheCleaner
{
	public class CacheCleaner
	{
		private readonly string _keysapce;
		private readonly IRedisClientsManager _redisClients;
		private readonly ICluster _cassandra;

		public CacheCleaner()
		{
			string host2 = "35.189.70.205";
			string user = "cassandra";
			string password = "cassandra";

			_keysapce = "db";
			_cassandra = Cluster.Builder()
				.AddContactPoint(host2)
				.WithPort(9042)
				.WithCredentials(user, password)
				.Build();

			_redisClients = new RedisManagerPool();
		}

		public void ClearRedis(int maxLifeTimeSeconds)
		{
			using (var redis = _redisClients.GetClient())
			{
				var keys = redis.GetAllKeys();
				var timeKeys = keys.Where(k => k.StartsWith("t")).ToList();
				timeKeys.ForEach(tk =>
				{
					var timeUnix = redis.Get<string>(tk);
					var lifeTime = (DateTime.Now - UnixTimestampToDateTime(Double.Parse(timeUnix))).TotalSeconds;
					if (lifeTime > maxLifeTimeSeconds)
					{
						redis.DeleteById<string>(tk);
						redis.DeleteById<string>(tk.Remove(0,1));
					}
				});
			}
		}

		public void ClearCassandra(int maxLifeTimeSeconds)
		{
			using (var session = _cassandra.Connect(_keysapce))
			{
				RowSet lifetimes = session.Execute("select * from lifetimes;");
				foreach (var time in lifetimes)
				{
					var lifeTime = (DateTime.Now - UnixTimestampToDateTime(Double.Parse(time.GetValue<string>("time")))).TotalSeconds;
                    if(lifeTime > maxLifeTimeSeconds)
                    {
                        var id = time.GetValue<int>("staff_Id");
                        session.Execute($"delete from lifetimes where staff_Id = {id};");
                        session.Execute($"delete from staff where Id = {id};");
                    }
                }
			}
		}

		public static DateTime UnixTimestampToDateTime(double unixTime)
		{
			DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
			return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
		}
	}
}
