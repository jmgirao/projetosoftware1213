using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeepYourTime.ViewControls.TaskDetailsControls
{
    /// <summary>
    /// Interaction logic for TaskDetailsControl.xaml
    /// </summary>
    public partial class TaskDetailsControl : UserControl
    {
        public TaskDetailsControl()
        {
            InitializeComponent();
        }

        private void TaskDetailsControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }

        private void InitializeControl()
        {
            var mhResult = new MethodHandler();
            int idTask = 0;                 //TODO: To receive idTask another window
            TaskAdapter taskAdapt = new TaskAdapter();

            try
            {
                mhResult = TaskConnector.ReadTask(idTask, out taskAdapt);
                if (mhResult.Exits)
                {
                    MessageBox.Show(mhResult.Message);
                    return;
                }

                string taskName = taskAdapt.TaskName;
                string taskDescription = taskAdapt.Description;
                ObservableCollection<TaskTimeAdapter> taskTimeList = taskAdapt.Times;

                lbTaskNameText.Text = taskName;
                lbTaskDescriptionText.Text = taskDescription;
                dgTaskTimes.ItemsSource = taskTimeList;

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
            }

        }

        private void btCloseTaskDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

}
