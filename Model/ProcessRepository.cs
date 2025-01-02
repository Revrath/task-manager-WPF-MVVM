using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model
{
	public class ProcessRepository
	{
		public static ObservableCollection<Process> GetProcesses()
		{
			var p = Process.GetProcesses();
			
			var myProcesses = new ObservableCollection<Process>(p);
			// foreach (var process in p)
			// {
			// 	myProcesses.Add(new MyProcess(process.ProcessName, process));
			//
			// }
			return myProcesses;
		}

		
	}
}
