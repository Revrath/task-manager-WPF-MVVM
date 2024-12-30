using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskManager.Command
{
	public class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Action execute { get; set; }
		private Func<bool> canExecute { get; set; }


		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}


		public bool CanExecute(object parameter)
		{
			return canExecute == null || canExecute();
		}

		public void Execute(object parameter)
		{
			execute();
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}
