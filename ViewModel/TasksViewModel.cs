using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Model;

namespace TaskManager.ViewModel
{
	internal class TasksViewModel : BaseViewModel
	{
		public ICommand ExecuteCmdCommand { get; }
		public ICommand StopTaskCommand { get; }
		public string CmdString { get; set; }
		public int CmdTime { get; set; }
		public bool KeepOpen {get; set; }
		private ObservableCollection<MyTask> _tasks = new ObservableCollection<MyTask>();
		public ObservableCollection<MyTask> Tasks
		{
			get => _tasks;
			set
			{
				_tasks= value;
				OnPropertyChanged("Tasks");
			}
		}
		public TasksViewModel()
		{
			StopTaskCommand = new RelayCommand(StopTask);
			ExecuteCmdCommand = new RelayCommand(ExecuteCmd);
		}

		private void StopTask(object tsk)
		{
			var myTask = (MyTask)tsk;
			myTask.Cts.Cancel();
			Tasks.Remove(myTask);
		}
		private void ExecuteCmd()
		{
			//s -> ms, min 1 second 
			CmdTime = Math.Max(1, CmdTime);
			CmdTime *= 1000;	

			var t = new MyTask
			{
				Name = CmdString,
				Time = CmdTime,
				Cts = new CancellationTokenSource()
			};
			if (KeepOpen)
				CmdString = "/K " + CmdString;
			else
				CmdString = "/C " + CmdString;
			Tasks.Add(t);
			CmdExecution(t);
		}

		private async Task CmdExecution(MyTask task)
		{
			while (true)
			{
				if (task.Cts.IsCancellationRequested)
				{
					return;
				}
				System.Diagnostics.Process.Start("CMD.exe", CmdString);
				task.TimesRun++;
				await Task.Delay(CmdTime);
			}
		}
	}
}
