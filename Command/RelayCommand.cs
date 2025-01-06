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
		private Action<object> execute { get; set; }
		private Action execute2 { get; set; }
		private Func<object, bool> canExecute { get; set; }
		private Func<bool> canExecute2 { get; set; }

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}
		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			this.execute2 = execute;
			this.canExecute2 = canExecute;
		}


		public bool CanExecute(object parameter)
		{
			if (parameter != null)
				return this.canExecute == null || this.canExecute(parameter);
			return this.canExecute == null || this.canExecute2();
		}

		public void Execute(object parameter)
		{
			if (parameter != null) 
				this.execute(parameter);
			else
			{
				this.execute2();
			}
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
