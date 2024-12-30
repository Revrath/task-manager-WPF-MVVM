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
		// public ObservableCollection<MyProcess> MyProcess { get; set; } = ProcessRepository.GetProcesses();
		public IEnumerable<MyProcess> MyProcess { get; set; } = ProcessRepository.GetProcesses();
		public ICommand RefreshCommand { get; }
		public ICommand FilterAndSortCommand;
		private bool isAscending = true;
		

		private string _FilterString;
		public string FilterString
		{
			get { return _FilterString; }
			set
			{
				_FilterString = value;
				OnPropertyChanged("FilterString");
			}
		}

		public ProcessViewModel()
		{
			RefreshCommand = new RelayCommand(Refresh);
			FilterAndSortCommand = new RelayCommand(FilterAndSort);
		}

		private void Sort()
		{
			isAscending = !isAscending;
		}
		private void Refresh()
		{
			FilterAndSort();
		}

		private void FilterAndSort()
		{
			IEnumerable<MyProcess> p = ProcessRepository.GetProcesses();
			p = p.Where(x => x.Name.Contains(_FilterString));
			if (isAscending)
			{
				p = p.OrderBy(x => x.Name);
			}
			if (isAscending)
			{
				p = p.OrderByDescending(x => x.Name);
			}

			MyProcess = p;
			OnPropertyChanged(nameof(MyProcess));
		}
	}
}
