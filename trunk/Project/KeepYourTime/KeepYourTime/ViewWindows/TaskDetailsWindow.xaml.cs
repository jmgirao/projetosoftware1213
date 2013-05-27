using KeepYourTime.DataBase;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewControls.MainWindowControls;
using KeepYourTime.ViewControls.TaskDetailsControls;
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


        public static long TaskID = 0;    //identify task to select the task data

        public TaskDetailsWindow()
        {
            InitializeComponent();
            TaskDetails.OnEditTaskClick += TaskDetails_OnEditTaskClick;
            EditTask.onTaskEditCloseToDetailWindow += EditTask_onTaskEditCloseToDetailWindow; 
        }

        void EditTask_onTaskEditCloseToDetailWindow(object sender, EventArgs e)
        {
            TaskDetails.Visibility = Visibility.Visible;
            EditTask.Visibility = Visibility.Collapsed;            
        }

        void TaskDetails_OnEditTaskClick(object sender, EventArgs e)
        {
            TaskDetails.Visibility = Visibility.Collapsed;
            EditTask.Visibility = Visibility.Visible;
            EditTask.LoadTask(TaskID);
            EditTask.PreviousWindow = EditTaskContex.DetailWindow;
        }

    }
}
