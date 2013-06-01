using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;


namespace KeepYourTime.ViewControls.TaskDetailsControls
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    /// <remarks>
    /// CREATED BY Carla Machado
    /// </remarks>  
    public partial class EditTask : UserControl
    {
        ObservableCollection<TaskTimeAdapterUI> taskTimesAdapterUI = null;
        long EditTaskId = -1; //Value indicating not initialized with a task id
        public EditTaskContex PreviousWindow;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditTask()
        {
            InitializeComponent();
                      
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click;
            btnAddTime.Click += btnAddTime_Click;
        }       

        /// <summary>
        /// loads all the information from a certain task
        /// </summary>
        /// <param name="TaskID">identifier of the task</param>
        /// <remarks>
        /// CREATED BY Carla Machado
        /// </remarks> 
        public bool LoadTask(long TaskID)
        {
            var mhResult = new MethodHandler();
            TaskAdapter taskToEdit = new TaskAdapter();

            try
            {
                mhResult = TaskConnector.ReadTask(TaskID, out taskToEdit);

                EditTaskId = taskToEdit.TaskId;
                TxtTaskName.Text = taskToEdit.TaskName;
                TxtDescription.Text = taskToEdit.Description;

                taskTimesAdapterUI = new ObservableCollection<TaskTimeAdapterUI>();
                foreach (TaskTimeAdapter tta in taskToEdit.Times)
                    taskTimesAdapterUI.Add(new TaskTimeAdapterUI(tta));

                dgTaskTimes.ItemsSource = taskTimesAdapterUI;

                return true;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
                return false;
            }
        }

        /// <summary>
        /// Function to add a new time line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddTime_Click(object sender, RoutedEventArgs e)
        {
            var ttaTimeToAdd = new TaskTimeAdapterUI();

            ttaTimeToAdd.StartTime = DateTime.Now;
            ttaTimeToAdd.StopTime = DateTime.Now;

            taskTimesAdapterUI.Add(ttaTimeToAdd);

            dgTaskTimes.ItemsSource = taskTimesAdapterUI;
        }

        /// <summary>
        /// Cancels the edition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  /// <remarks>
        /// CREATED BY Carla Machado
        /// </remarks> 
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (PreviousWindow == EditTaskContex.MainWindow)
                Window.GetWindow(this).Close();
            else if (PreviousWindow == EditTaskContex.DetailWindow)
            {
                if (onTaskEditCloseToDetailWindow != null)
                    onTaskEditCloseToDetailWindow(this, new EventArgs());
            }
        }

        public event EventHandler onTaskEditCloseToDetailWindow;

        /// <summary>
        /// Saves the changes to the task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  /// <remarks>
        /// CREATED BY Carla Machado
        /// </remarks> 
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            TaskAdapter TaskToEdit = new TaskAdapter();
            TaskToEdit.Times = new ObservableCollection<TaskTimeAdapter>();
            String strTaskName = TxtTaskName.Text;

            try
            {
                if (EditTaskId == -1)
                {
                    mhResult.Status = Utils.MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.InexistentTask;
                    return;
                }

                foreach (var time in taskTimesAdapterUI)
                {
                    if (ValidateTaskTime(time))
                    {
                        TaskToEdit.Times.Add(time);
                    }
                    else
                    {
                        mhResult.Status = Utils.MethodStatus.Cancel;
                        mhResult.Message = Languages.Language.InvalidField + ". " + Languages.Language.TimeTaskInvalid;
                        return;
                    }
                }

                if (!ValidateDistinctTime(taskTimesAdapterUI))
                {
                    mhResult.Status = Utils.MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.InvalidField + ". " + Languages.Language.TaskTimesOverlap;
                    return;
                }

                if (string.IsNullOrEmpty(strTaskName))
                {
                    mhResult.Status = Utils.MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.InvalidField + ". " + Languages.Language.TaskNameMadatory;                    
                    return;
                }

                TaskToEdit.TaskId = EditTaskId;
                TaskToEdit.TaskName = strTaskName;
                TaskToEdit.Description = TxtDescription.Text;

                mhResult = TaskConnector.EditTask(TaskToEdit);
                if (mhResult.Exits) 
                    return;

                Utils.StaticEvents.RaiseEventOnTaskUpdated(TaskToEdit.TaskId);

                btnCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);                
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, true);
            }
        }

        /// <summary>
        /// method to eliminate a time line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTimeDeleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => taskTimesAdapterUI.Remove((TaskTimeAdapterUI)sender)));
        }

        /// <summary>
        /// method to validate if a task time has value and time interval is bigger than 0
        /// </summary>
        /// <param name="TaskTime"></param>
        /// <returns>true if time is valid</returns>
        public bool ValidateTaskTime(TaskTimeAdapterUI TaskTime)
        {
            if (TaskTime.StartTime == DateTime.MinValue || TaskTime.StopTime == DateTime.MinValue)
                return false;
            if (TaskTime.StartTime >= TaskTime.StopTime)
                return false;

            return true;
        }

        /// <summary>
        /// Validates if time intevals are valid, and not subintervals exist
        /// </summary>
        /// <param name="TimesList"></param>
        /// <returns>true if time list is valid</returns>
        public bool ValidateDistinctTime(ObservableCollection<TaskTimeAdapterUI> TimesList)
        {
            foreach (var timeValidate in TimesList)
            {
                foreach (var timeLine in TimesList)
                {
                    if (timeLine.TimeId != timeValidate.TimeId)
                    {                       
                        if (timeValidate.StartTime == timeLine.StartTime)
                            return false;
                        if (timeValidate.StopTime == timeLine.StopTime)
                            return false;
                        if (timeValidate.StartTime >= timeLine.StartTime & timeValidate.StopTime <= timeLine.StopTime)
                            return false;
                        if (timeValidate.StartTime >= timeLine.StartTime & timeValidate.StartTime <= timeLine.StopTime)
                            return false;
                    }
                }
            }

            return true;
        }

    }
}
