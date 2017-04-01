using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContactManager.Presenters;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            DataContext = new ApplicationPresenter(this, new Model.ContactRepository());
        }

        public void AddTab<T>(PresenterBase<T> presenter)
        {
            TabItem item = null;
            for (int i = 0; i < tabs.Items.Count; i++)
            {
                TabItem existing = (TabItem)tabs.Items[i];

                if (existing.DataContext.Equals(item.DataContext))
                {
                    tabs.Items.Remove(existing);
                    item = existing;
                    break;
                }
            }

            if (item == null)
            {
                item = new TabItem();

                Binding headerBinding = new Binding(presenter.TabHeaderPath);
                BindingOperations.SetBinding(item, TabItem.HeaderProperty, headerBinding);

                item.DataContext = presenter;
                item.Content = presenter.View;
            }

            tabs.Items.Insert(0, item);
            item.Focus();
        }

        public void RemoveTab<T>(PresenterBase<T> presenter)
        {
            for (int i = 0; i < tabs.Items.Count; i++)
            {
                TabItem item = (TabItem) tabs.Items[i];
                if (item.DataContext.Equals(presenter))
                {
                    tabs.Items.Remove(item);
                    break;
                }
            }
        }

    }
}
