using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace imageanalyzer.dotnet.ui.operations
{
	public class ObserverOperation
		: Notifier, IObserverOperation
    {
		public ObserverOperation(string _task_name, ItemsControl _items)
		{
			task_name = _task_name;
			items = _items;
		}
		//IObserver
		public void HandleStart()
		{
            Title = task_name + " starting...";

            List<ObserverOperation> items_list = new List<ObserverOperation>();
            if (items.ItemsSource != null)
                items_list.AddRange(items.ItemsSource as IEnumerable<ObserverOperation>);
            items_list.Add(this);

            items.Dispatcher.Invoke(new Action(() => { items.ItemsSource = items_list; }));
        }
		public void HandleChangeProgress(double aPercent)
		{
			Title = task_name + "...";
			Progress = aPercent;
		}
		public void HandleComplete()
		{
			Title = task_name + " complete";
			List<ObserverOperation> items_list = new List<ObserverOperation>();
			if (items.ItemsSource != null)
				items_list.AddRange(items.ItemsSource as IEnumerable<ObserverOperation>);

			items_list.Remove(this);
			items.Dispatcher.Invoke(new Action(()=> { items.ItemsSource = items_list; }));
		}
		//Notifier fields
		public double Progress
		{
			get { return progress; }
			set
			{
				progress = value;
				NotifyPropertyChanged("Progress");
			}
		}
		public string Title
		{
			get { return title; }
			set
			{
				title = value;
				NotifyPropertyChanged("Title");
			}
		}

		private double progress;
		private string title;
		private string task_name;
		private ItemsControl items;
	}
}
