using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BookBuddy.Utils.ReplacingBooks
{
    // To display the indexes next to the value in my ReplacingBooksView for the user to better understand the task
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as ListViewItem;
            if (item != null)
            {
                var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
                int index = listView.ItemContainerGenerator.IndexFromContainer(item);
                return index + 1; // +1 to start the index at 1 instead of 0
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
