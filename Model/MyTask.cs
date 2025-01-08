using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Model
{
	internal class MyTask
	{
		public string Name { get; set; }
		public int Time  { get; set; }
		public CancellationTokenSource Cts;
		public MyTask This { get; set; }

		public MyTask()
		{
			This = this;
		}
	}
}
