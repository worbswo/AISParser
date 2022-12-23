using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
namespace AISParser.Code
{
	public class ScrollSelectedItemIntoViewBehavior : Behavior<ListView>
	{

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
		}
		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
		}

		private void AssociatedObject_SelectionChanged(object sender,
														SelectionChangedEventArgs e)
		{
			if(sender is ListView)
			{
				ListView listview =(sender as ListView);
				if (listview.SelectedItem != null)
				{
					listview.Dispatcher.BeginInvoke(
						(Action)(() =>
						{
							listview.UpdateLayout();
							if (listview.SelectedItem != null)
							{
								listview.ScrollIntoView(listview.SelectedItem);
							}
						}));
				}
			}
		}
		
	}
}
