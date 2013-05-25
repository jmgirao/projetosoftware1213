using KeepYourTime.DataBase;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeepYourTime.ViewWindows
{
    /// <summary>
    /// Interaction logic for TaskDetailsWindow.xaml
    /// </summary>
    public partial class TaskDetailsWindow : Window
    {
        public TaskDetailsWindow()
        {
            InitializeComponent();
            TaskDetails.OnEditTaskClick += TaskDetails_OnEditTaskClick;
        }

        void TaskDetails_OnEditTaskClick(object sender, EventArgs e)
        {
            TaskDetails.Visibility = Visibility.Collapsed;
            EditTask.Visibility = Visibility.Visible;
        }

    }
}
