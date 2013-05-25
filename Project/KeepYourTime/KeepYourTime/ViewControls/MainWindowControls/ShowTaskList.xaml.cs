using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;
using System.Windows.Data;
using System.Windows.Media;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Interaction logic for ShowTaskList.xaml
    /// </summary>
    /// <remarks>
    /// CREATED BY Mário Oliveira
    /// </remarks>  
    public partial class ShowTaskList : UserControl
    {
        ObservableCollection<TaskAdapterUI> taskAdaptUi = null;
        ObservableCollection<TaskAdapterUI> taskAdaptUiInactiveTask = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowTaskList"/> class.
        /// </summary>
        public ShowTaskList()
        {

            InitializeComponent();
            this.Loaded += ShowTaskList_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the ShowTaskList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void ShowTaskList_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }

        /// <summary>
        /// Createds the task.
        /// </summary>
        /// <param name="Task">The task.</param>
        public void CreatedTask(TaskAdapter Task)
        {
            //TODO: Ordenar lista quando se cria uma tarefa!
            taskAdaptUi.Add(new TaskAdapterUI(Task));
        }

        /// <summary>
        /// Receives the task list.
        /// </summary>
        /// <param name="taskAdapt">The task adapt.</param>
        public void ReceiveTaskList(ObservableCollection<TaskAdapter> taskAdapt)
        {
            taskAdaptUi = new ObservableCollection<TaskAdapterUI>();
            taskAdaptUiInactiveTask = new ObservableCollection<TaskAdapterUI>();

            foreach (TaskAdapter t in taskAdapt)
            {
                var ta = new TaskAdapterUI(t);
                taskAdaptUi.Add(ta);
                ta.OnTaskDeactivated += ta_OnTaskDeactivated;
            }


            //The system groups the active tasks and inactive tasks in different groups.
            foreach (TaskAdapter t in taskAdaptUi)
            {
                var ta = new TaskAdapterUI(t);
                taskAdaptUiInactiveTask.Add(ta);
            }

            taskAdaptUi.Clear();
            foreach (TaskAdapter t in taskAdaptUiInactiveTask)
            {
                var ta = new TaskAdapterUI(t);
                if (ta.Active)
                {
                    taskAdaptUi.Add(ta);
                    ta.OnTaskDeactivated += ta_OnTaskDeactivated;
                }
            }
            foreach (TaskAdapter t in taskAdaptUiInactiveTask)
            {
                var ta = new TaskAdapterUI(t);
                if (!ta.Active)
                {
                    taskAdaptUi.Add(ta);
                    ta.OnTaskDeactivated += ta_OnTaskDeactivated;
                }
            }
            //end
            dgTaskList.ItemsSource = taskAdaptUi;
        }

        /// <summary>
        /// Handles the OnTaskDeactivated event of the ta control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ta_OnTaskDeactivated(object sender, EventArgs e)
        {
            if (chkShowInactiveTask.IsChecked.Value == false)
            {
                Dispatcher.BeginInvoke((Action)(() => taskAdaptUi.Remove((TaskAdapterUI)sender)));
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        private void InitializeControl()
        {
            var mhResult = new MethodHandler();

            try
            {
                bool blnActive = chkShowInactiveTask.IsChecked.Value;

                ObservableCollection<TaskAdapter> taskAdapt = null;
                mhResult = TaskConnector.ReadTaskList(out taskAdapt, blnActive);
                if (mhResult.Exits) return;

                ReceiveTaskList(taskAdapt);

            }
            catch (Exception e)
            {
                mhResult.Exception(e);
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, false);
            }
        }

        /// <summary>
        /// Handles the LoadingRow event of the DataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.DataGridRowEventArgs"/> instance containing the event data.</param>
        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            TaskAdapter RowDataContaxt = e.Row.DataContext as TaskAdapter;
            if (RowDataContaxt != null)
                if (!RowDataContaxt.Active)
                    e.Row.Background = new SolidColorBrush(Colors.LightGray);
        }
        /// <summary>
        /// Handles the RowEditEnding event of the dgTaskList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridRowEditEndingEventArgs"/> instance containing the event data.</param>
        private void dgTaskList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            TaskAdapter RowDataContaxt = e.Row.DataContext as TaskAdapter;
            if (RowDataContaxt != null)
                if (!RowDataContaxt.Active)
                    e.Row.Background = new SolidColorBrush(Colors.LightGray);
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
        }  

        /// <summary>
        /// Handles the Click event of the btDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// CREATED BY Joao Girao
        /// </remarks>
        private void btDetails_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();

            try
            {
                object objTaskId = ((FrameworkElement)sender).DataContext;
                MinimalViewControl.TaskID = ((TaskAdapterUI)objTaskId).TaskId;
                var detailswindows = new TaskDetailsWindow();
                detailswindows.ShowDialog();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageWindow.ShowMethodHandler(mhResult, false);
            }

        }

        /// <summary>
        /// Handles the Click event of the btPlay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btPlay_Click(object sender, RoutedEventArgs e)
        {
            
            var mhResult = new MethodHandler();
            ObservableCollection<TaskAdapterUI> taskAdaptUiPlay = null;
            try
            {
                object objTaskId = ((FrameworkElement)sender).DataContext;
                MinimalViewControl.TaskID = ((TaskAdapterUI)objTaskId).TaskId;

                /*
                 
                MessageBox.Show("ID da tarefa a ir la para cima: " + MinimalViewControl.TaskID);

                taskAdaptUiPlay = new ObservableCollection<TaskAdapterUI>();

                foreach (TaskAdapter t in taskAdaptUi)
                {
                    var ta = new TaskAdapterUI(t);
                    if(ta.TaskId==MinimalViewControl.TaskID){
                        ta.IsRunning = true;
                        taskAdaptUiPlay.Add(ta);
                        taskAdaptUi.Remove(ta);
                        break;
                    }
                }

                foreach (TaskAdapter t in taskAdaptUiPlay)
                {
                    var ta = new TaskAdapterUI(t);
                    if (ta.IsRunning)
                    {
                        taskAdaptUi.Add(ta);
                        taskAdaptUiPlay.Remove(ta);
                        break;
                    }
                }
                 
                */
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageWindow.ShowMethodHandler(mhResult, false);
            }
        }

    }
}
