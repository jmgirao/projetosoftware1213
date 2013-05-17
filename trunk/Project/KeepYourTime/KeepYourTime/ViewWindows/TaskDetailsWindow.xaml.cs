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
            SourceInitialized += Window_Initialized;
            InitializeComponent();

            btCloseTaskDetails.Click += btCloseTaskDetails_Click; 
        }

        /// <summary>
        /// Handles the Initialized event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Window_Initialized(object sender, EventArgs e)
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
                dgTaskTimes.DataContext = taskTimeList;

            }catch(Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
            }

        }

        private void btCloseTaskDetails_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
