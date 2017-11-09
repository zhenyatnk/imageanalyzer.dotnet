using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageanalyzer.dotnet.ui.view_model
{
	public class ObservableCollectionDisp<T> : ObservableCollection<T>
	{
		public ObservableCollectionDisp()
		{
			current_dispacter = Dispatcher.CurrentDispatcher;
		}

		public Dispatcher dispatcher
		{
			get
			{
				return current_dispacter;
			}
		}

		private Dispatcher current_dispacter;
	}
}
