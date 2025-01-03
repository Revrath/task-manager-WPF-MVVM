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

		private CancellationTokenSource _cancelts = new CancellationTokenSource();
		public ProcessViewModel()
		{
			RefreshCommand = new RelayCommand(Refresh);
			SortCommand = new RelayCommand(Sort);
			ChangeRefreshTimeCommand = new RelayCommand(ChangeRefreshTime);

			//do not await for infinite loop
			RefreshIndefinetely(1000, _cancelts);
		}

		private async Task RefreshIndefinetely(int time, CancellationTokenSource cts)
		{
			while (true)
			{
				Refresh();
				await Task.Delay(time);
			}
		}

		private void ChangeRefreshTime()
		{
			_cancelts.Cancel();
			if (RefreshTime != 0)
				RefreshIndefinetely(RefreshTime, _cancelts);
		}

		private void Sort()
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
			Refresh();
		}
		private void Refresh()
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
