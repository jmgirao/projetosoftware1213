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

            //this.Loaded += EditTaskControl_Loaded;
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click;
            btnAddTime.Click += btnAddTime_Click;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void EditTaskControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    InitializeControl();
        //}


        ///// <summary>
        ///// initializes the necessari data for the form
        ///// </summary>
        //private void InitializeControl()
        //{
           
        //}

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
            else if(PreviousWindow == EditTaskContex.DetailWindow)
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

            if (EditTaskId == -1)
            {
                //TODO - tratar excepção
                //HACK: User MethodHanlder para Isto - GANHOTO
                mhResult.Exception(new Exception(Languages.Language.InexistentTask));
                MessageWindow.ShowMethodHandler(mhResult, true);
                //MessageBox.Show(Languages.Language.InexistentTask);
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
                    mhResult.Exception(new Exception(Languages.Language.TimeTaskInvalid));
                    MessageWindow.ShowMethodHandler(mhResult, true);
                    return;
                }
            }

            String strTaskName = TxtTaskName.Text;

            if (string.IsNullOrEmpty(strTaskName))
            {
                mhResult.Exception(new Exception(Languages.Language.TaskNameMadatory));
                MessageWindow.ShowMethodHandler(mhResult, true);
                return;
            }           

            try
            {
                TaskToEdit.TaskId = EditTaskId;
                TaskToEdit.TaskName = strTaskName;
                TaskToEdit.Description = TxtDescription.Text;
                TaskToEdit.Times = new ObservableCollection<TaskTimeAdapter>();

                mhResult = TaskConnector.EditTask(TaskToEdit);
               
                btnCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
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
        /// method to validate if a task time is valid
        /// </summary>
        /// <param name="TaskTime"></param>
        /// <returns></returns>
        private bool ValidateTaskTime(TaskTimeAdapterUI TaskTime)
        {
            if (TaskTime.StartTime == null || TaskTime.StopTime == null)
                return false;
            if (TaskTime.StartTime < TaskTime.StopTime)
                return false;

            return true;
        }        

    }
}
