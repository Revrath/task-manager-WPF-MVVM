using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model
{
	public  class ProcessRepository
	{
		public static ObservableCollection<MyProcess> GetProcesses()
		{
			var p = Process.GetProcesses();
			var myProcesses = new ObservableCollection<MyProcess>();
			foreach (var process in p)
			{
				myProcesses.Add(new MyProcess(process.ProcessName));
			}
			return myProcesses;
		}

		
	}
}
