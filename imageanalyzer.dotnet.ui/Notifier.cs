﻿using System.ComponentModel;

namespace imageanalyzer.dotnet.ui
{
	public class Notifier 
        : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected void NotifyPropertyChanged(
			string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
