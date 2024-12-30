using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Model;
using System.Diagnostics;
using TaskManager.Command;
namespace TaskManager.ViewModel
{
	public class ProcessViewModel : BaseViewModel
	{
		public ICommand RefreshCommand { get; }
		public ICommand SortCommand { get; }
		
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

		private readonly IEnumerable<MyProcess> _allProcesses = ProcessRepository.GetProcesses();
		private IEnumerable<MyProcess> _processes;
		public IEnumerable<MyProcess> Processes
		{
			get => _processes;
			set
			{
				_processes = value;
				OnPropertyChanged("Processes");
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

		public ProcessViewModel()
		{
			RefreshCommand = new RelayCommand(Refresh);
			SortCommand = new RelayCommand(Sort);
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
			IEnumerable<MyProcess> p = _allProcesses;
			p = p.Where(x => x.Name.Contains(_filterString));
			if (_isAscending)
			{
				p = p.OrderBy(x => x.Name);
			}
			else
			{
				p = p.OrderByDescending(x => x.Name);
			}
			Processes = p;
		}
	}
}
