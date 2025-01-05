using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Model;
using System.Diagnostics;
using System.Threading;
using TaskManager.Command;
namespace TaskManager.ViewModel
{
	public class ProcessViewModel : BaseViewModel
	{
		public ICommand RefreshCommand { get; }
		public ICommand SortCommand { get; }
		public ICommand KillCommand { get; }
		public ICommand ChoosePriorityCommand { get; }
		public ICommand SetPriorityCommand { get; }
		public ICommand ChangeRefreshTimeCommand { get; }
		
		private bool _isAscending = true;

		private string _filterString;
		public string FilterString
		{
			get => _filterString;
			set
			{
				_filterString = value;
				OnPropertyChanged("FilterString");
				FilterAndSort();
			}
		}

		private IEnumerable<Process> _processes;
		public IEnumerable<Process> Processes
		{
			get => _processes;
			set
			{
				_processes = value;
				OnPropertyChanged("Processes");
			}
		}
		private Process _selectedProcess;
		public Process SelectedProcess
		{
			get => _selectedProcess;
			set
			{
				if (value != null)
				{
					_selectedProcess = value;
				}

				OnPropertyChanged("SelectedProcess");
			}
		}

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

		private int _refreshTime = 1000;
		public int RefreshTime
		{
			get => _refreshTime;
			set
			{
				_refreshTime = value;
				OnPropertyChanged("RefreshTime");
			}
		}

		private ProcessPriorityClass _priority;

		public ProcessPriorityClass Priority
		{
			get => _priority;
			set
			{
				_priority = value;
				OnPropertyChanged("Priority");
			}
		}

		private CancellationTokenSource _cancelts = new CancellationTokenSource();
		public ProcessViewModel()
		{
			RefreshCommand = new RelayCommand(Refresh);
			SortCommand = new RelayCommand(Sort);
			KillCommand = new RelayCommand(Kill);
			SetPriorityCommand = new RelayCommand(SetPriority);
			ChangeRefreshTimeCommand = new RelayCommand(ChangeRefreshTime);

			//do not await for infinite loop
			RefreshIndefinetely(1000, _cancelts);
		}

		private void Kill(object nah)
		{
			SelectedProcess.Kill();
		}
		private void SetPriority(object p)
		{
			SelectedProcess.PriorityClass = (ProcessPriorityClass)p;
		}
		private async Task RefreshIndefinetely(int time, CancellationTokenSource cts)
		{
			while (true)
			{
				if (cts.IsCancellationRequested)
				{
					return;
				}
				Refresh(null);
				await Task.Delay(time);
			}
		}

		private void ChangeRefreshTime(object nah)
		{
			_cancelts.Cancel();
			_cancelts = new CancellationTokenSource();
			if (RefreshTime != 0)
				RefreshIndefinetely(RefreshTime, _cancelts);
		}

		private void Sort(object nah)
		{
			_isAscending = !_isAscending;
			if (_isAscending)
			{
				SortButtonText = "Ascending";
			}
			else
			{
				SortButtonText = "Descending";
			}
			FilterAndSort();
		}
		private void Refresh(object nah)
		{
			FilterAndSort();
		}

		private void FilterAndSort()
		{
			IEnumerable<Process> p = ProcessRepository.GetProcesses(); 
			if (_filterString != null)
				p = p.Where(x => x.ProcessName.Contains(_filterString));
			
			if (_isAscending)
			{
				p = p.OrderBy(x => x.ProcessName);
			}
			else
			{
				p = p.OrderByDescending(x => x.ProcessName);
			}
			Processes = p;
		}
	}
}
