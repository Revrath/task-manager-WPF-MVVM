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

		public ICommand SortCommand { get; }

		private string _sortButtonText;
		public string SortButtonText
		{
			get => _sortButtonText ?? (_sortButtonText = "Ascending");
			set
			{
				_sortButtonText = value;
				OnPropertyChanged("SortButtonText");
			}
		}

		private bool _isAscending = true;

		public string CmdString { get; set; }
		public int CmdTime { get; set; } = 1;
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
			SortCommand = new RelayCommand(Sort);
			StopTaskCommand = new RelayCommand(StopTask);
			ExecuteCmdCommand = new RelayCommand(ExecuteCmd);
		}
		private void StopTask(object tsk)
		{
			var myTask = (MyTask)tsk;
			myTask.Cts.Cancel();
			Tasks.Remove(myTask);
		}


		private void Sort()
		{
			_isAscending = !_isAscending;
			if (_isAscending)
			{
				SortButtonText = "Ascending";
				Tasks = new ObservableCollection<MyTask>(Tasks.OrderBy(x => x.Name));
			}
			else
			{
				SortButtonText = "Descending";
				Tasks = new ObservableCollection<MyTask>(Tasks.OrderByDescending(x => x.Name));
			}
		}

		private void ExecuteCmd()
		{
			CmdTime = Math.Max(1, CmdTime);
			string defaultString = CmdString;
			if (KeepOpen)
				CmdString = "/K " + CmdString;
			else
				CmdString = "/C " + CmdString;

			var t = new MyTask
			{
				Name = CmdString,
				Time = CmdTime,
				Cts = new CancellationTokenSource()
			};
			CmdString = defaultString;
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
				System.Diagnostics.Process.Start("CMD.exe", task.Name);
				task.TimesRun++;
				task.LastRun = DateTime.Now;
				await Task.Delay(task.Time* 1000);
			}
		}
	}
}
