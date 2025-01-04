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
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private Action<object> execute { get; set; }
		private Func<object, bool> canExecute { get; set; }

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}


		public bool CanExecute(object parameter)
		{
			return this.canExecute == null || this.canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this.execute(parameter);
		}

		// public void RaiseCanExecuteChanged()
		// {
		// 	if (CanExecuteChanged != null)
		// 	{
		// 		CanExecuteChanged(this, EventArgs.Empty);
		// 	}
		// }
	}
}
