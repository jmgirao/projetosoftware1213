using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;
using System.Windows.Data;
using System.Windows.Media;
using KeepYourTime.Utils;
using System.Linq;

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
        ObservableCollection<TaskAdapterUI> lstTaskAdaptUi = null;
        //ObservableCollection<TaskAdapterUI> lstTaskAdaptUiInactiveTask = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowTaskList"/> class.
        /// </summary>
        public ShowTaskList()
        {

            InitializeComponent();
            this.Loaded += ShowTaskList_Loaded;
            chkShowInactiveTask.Checked += chkShowInactiveTask_Checked;
            chkShowInactiveTask.Unchecked += chkShowInactiveTask_Checked;
            StaticEvents.OnTaskStarted += StaticEvents_OnTaskStarted;
            StaticEvents.OnTimeAdded += StaticEvents_OnTimeAdded;
        }

        void StaticEvents_OnTimeAdded(long TaskID)
        {
            //lstTaskAdaptUi.Clear();
            var mhResult = new MethodHandler();
            try
            {
                mhResult = TaskConnector.ReadTaskList(out MainWindow.lstTaskAdapt, chkShowInactiveTask.IsChecked.Value);
                if (mhResult.Exits) return;

                TaskAdapter taskBD = MainWindow.lstTaskAdapt.FirstOrDefault((t) => t.TaskId == TaskID);
                if (taskBD != null)
                {
                    TaskAdapterUI taskUI = lstTaskAdaptUi.FirstOrDefault((t) => t.TaskId == TaskID);
                    if (taskUI != null)
                    {
                        var intIndex = lstTaskAdaptUi.IndexOf(taskUI);
                        lstTaskAdaptUi.RemoveAt(intIndex);
                        taskUI = new TaskAdapterUI(taskBD);
                        taskUI.OnTaskDeactivated += ta_OnTaskDeactivated;
                        //lstTaskAdaptUi[intIndex] = taskUI;
                        lstTaskAdaptUi.Insert(intIndex, taskUI);
                        //taskUI.NotifyPropertyChanged("TaskRunning");
                    }
                }


                //ReceiveTaskList(MainWindow.lstTaskAdapt);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, false);
            }

        }
        
        void StaticEvents_OnTaskStarted(long TaskID)
        {

            var TaskUI = lstTaskAdaptUi.First((t) => t.TaskId == TaskID);
            if (TaskUI != null)
            {
                lstTaskAdaptUi.Move(lstTaskAdaptUi.IndexOf(TaskUI), 0);
            }

        }

        void chkShowInactiveTask_Checked(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                mhResult = TaskConnector.ReadTaskList(out MainWindow.lstTaskAdapt, chkShowInactiveTask.IsChecked.Value);
                if (mhResult.Exits) return;

                ReceiveTaskList(MainWindow.lstTaskAdapt);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, false);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the ShowTaskList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void ShowTaskList_Loaded(object sender, RoutedEventArgs e)
        {

            //InitializeControl();
        }

        /// <summary>
        /// Createds the task.
        /// </summary>
        /// <param name="Task">The task.</param>
        public void CreatedTask(TaskAdapter Task)
        {
            lstTaskAdaptUi.Insert(0, new TaskAdapterUI(Task));
        }

        /// <summary>
        /// Receives the task list.
        /// </summary>
        /// <param name="taskAdapt">The task adapt.</param>
        public void ReceiveTaskList(ObservableCollection<TaskAdapter> taskAdapt)
        {

            lstTaskAdaptUi = new ObservableCollection<TaskAdapterUI>();
            //lstTaskAdaptUiInactiveTask = new ObservableCollection<TaskAdapterUI>();

            //            foreach (TaskAdapter t in taskAdapt)
            //            {
            //                var ta = new TaskAdapterUI(t);
            //                lstTaskAdaptUi.Add(ta);
            //                ta.OnTaskDeactivated += ta_OnTaskDeactivated;
            //            }


            //            foreach (TaskAdapter t in lstTaskAdaptUi)
            //            {
            //                //HACK RG : why two 
            ////                var ta = new TaskAdapterUI(t);
            //                lstTaskAdaptUiInactiveTask.Add(ta);
            //            }

            //lstTaskAdaptUi.Clear();
            foreach (TaskAdapter t in taskAdapt)
            {
                var ta = new TaskAdapterUI(t);
                //if (ta.Active)
                //{
                lstTaskAdaptUi.Add(ta);
                ta.OnTaskDeactivated += ta_OnTaskDeactivated;
                //}
            }
            //The system groups the active tasks and inactive tasks in different groups.
            //foreach (TaskAdapter t in taskAdapt)
            //{
            //    var ta = new TaskAdapterUI(t);
            //    if (!ta.Active)
            //    {
            //        lstTaskAdaptUi.Add(ta);
            //        ta.OnTaskDeactivated += ta_OnTaskDeactivated;
            //    }
            //}
            //end
            dgTaskList.ItemsSource = lstTaskAdaptUi;
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
                Dispatcher.BeginInvoke((Action)(() => lstTaskAdaptUi.Remove((TaskAdapterUI)sender)));
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
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
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
                TaskDetailsWindow.TaskID = ((TaskAdapterUI)objTaskId).TaskId;
                var detailswindows = new TaskDetailsWindow();
                detailswindows.Show();
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
            //ObservableCollection<TaskAdapterUI> taskAdaptUiPlay = null;
            try
            {
                if (OnStartTask != null)
                {
                    var taTaskUi = ((FrameworkElement)sender).DataContext as TaskAdapterUI;
                    //TaskDetailsWindow.TaskID = ;
                    if (taTaskUi.Active == true)
                    {
                        OnStartTask(taTaskUi.TaskId);
                        lstTaskAdaptUi.Move(lstTaskAdaptUi.IndexOf(taTaskUi), 0);
                        taTaskUi.NotifyPropertyChanged("TaskRunning");
                    }
                }


                /* 
               
             MessageBox.Show("ID da tarefa a ir la para cima: " + MinimalViewControl.TaskID);

            taskAdaptUiPlay = new ObservableCollection<TaskAdapterUI>();
                
            foreach (TaskAdapter t in taskAdaptUi)
            {
                var ta = new TaskAdapterUI(t);
                if (ta.TaskId == TaskDetailsWindow.TaskID){
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
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, false);
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            bool blnShowResult = true; //this should be true for showing the result window
            var mhResult = new MethodHandler();
            //ObservableCollection<TaskAdapterUI> taskAdaptUiIsRunning = null;
            try
            {
                TaskAdapterUI objTaskId = ((FrameworkElement)sender).DataContext as TaskAdapterUI;

                MessageBoxResult mbrConfirmResult = MessageBox.Show(string.Format(Languages.Language.ComfirmTaskDelete, objTaskId.TaskName), Languages.Language.ConfirmDialog, MessageBoxButton.YesNo);

                if (mbrConfirmResult == MessageBoxResult.No)
                {
                    blnShowResult = false;
                    return;
                }

                // taskAdaptUiIsRunning = new ObservableCollection<TaskAdapterUI>();

                //foreach (TaskAdapter t in lstTaskAdaptUi)
                //{
                //    var ta = new TaskAdapterUI(t);
                //    if (ta.TaskId == TaskDetailsWindow.TaskID)
                //    {

                //        //if (ta.IsRunning == false)
                //        //{
                //        if (MinimalViewControl.CurrentTaskId != ta.TaskId)
                //            taskAdaptUiIsRunning.Add(ta);
                //        //break;
                //        //}
                //    }
                //}
                if (MinimalViewControl.CurrentTaskId != objTaskId.TaskId)
                {
                    mhResult = TaskConnector.DeleteTask(objTaskId.TaskId);
                    if (mhResult.Exits) return;

                    mhResult = TaskConnector.ReadTaskList(out MainWindow.lstTaskAdapt, chkShowInactiveTask.IsChecked.Value);
                    if (mhResult.Exits) return;

                    ReceiveTaskList(MainWindow.lstTaskAdapt);
                    //InitializeControl();
                }
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                if (blnShowResult)
                    MessageWindow.ShowMethodHandler(mhResult, true);
            }
        }

        public delegate void StartTaskHandler(long TaskID);

        public event StartTaskHandler OnStartTask;


    }
}
