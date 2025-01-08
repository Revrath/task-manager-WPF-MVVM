using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Model
{
	internal class MyTask : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public int Time  { get; set; }
		public DateTime LastRun;
		public CancellationTokenSource Cts;

		public string TimeLeft
		{
			get
			{
				return $"{(DateTime.Now - LastRun).TotalSeconds:F2} seconds since start";
			}
		}

		private int _timesRun;
		public int TimesRun
		{
			get => _timesRun;
			set
			{
				_timesRun = value;
				OnPropertyChanged("TimesRun");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
