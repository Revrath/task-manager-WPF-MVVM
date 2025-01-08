using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Model;

namespace TaskManager.ViewModel
{
	internal class TasksViewModel : BaseViewModel
	{
		public ICommand ExecuteCmdCommand { get; }
		public string CmdString { get; set; }
		public int CmdTime { get; set; }
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

			ExecuteCmdCommand = new RelayCommand(ExecuteCmd);
		}
		private void ExecuteCmd()
		{
			var t = new MyTask
			{
				Name = CmdString
			};
			Tasks.Add(t);
			// while (true)
			// {
			// 	// {
			// 	// 	if (cts.IsCancellationRequested)
			// 	// 	{
			// 	// 		return;
			// 	// 	}
			// 	CmdExecution();
			// }
		}

		private async Task CmdExecution()
		{
			while (true)
			{
				// {
				// 	if (cts.IsCancellationRequested)
				// 	{
				// 		return;
				// 	}
				System.Diagnostics.Process.Start("CMD.exe", CmdString);

				await Task.Delay(CmdTime);
			}
		}
	}
}
