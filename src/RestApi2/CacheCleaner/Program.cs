using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CacheCleaner
{
	class Program
	{
		static void Main(string[] args)
		{
            InitTimer();
		}

		public static void InitTimer()
		{
			var timer = new System.Timers.Timer();
			timer.Interval = 60000;
			timer.Elapsed += (object sender, ElapsedEventArgs eargs) => {
				timer.Enabled = false;
				var cacheCleaner = new CacheCleaner();
				cacheCleaner.ClearRedis(300);
				cacheCleaner.ClearCassandra(600);
				timer.Enabled = true;
			};
			timer.Enabled = true;
		}
	}
}
