using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewControls.MainWindowControls;
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
        EditTaskContex PreviousWindow;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditTask()
        {
            InitializeComponent();

            this.Loaded += EditTaskControl_Loaded;
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click;
            btnAddTime.Click += btnAddTime_Click;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTaskControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }


        /// <summary>
        /// initializes the necessari data for the form
        /// </summary>
        private void InitializeControl()
        {
            //HACK: Não faz sentido fazeres load disto no inicio, 
            //HACK.V2: Faz load quando carregas no botão de Editar -GANHOTO
           
            
            //TODO - get taskID
            //if (MinimalViewControl.TaskID)
            //{
            //EditTaskId = TaskDetailsWindow.TaskID;
            //    PreviousWindow = EditTaskContex.MainWindow;
            //}
            //else
            //{
            //    //TODO get context from details
            //    PreviousWindow = EditTaskContex.DetailWindow;
            //}

            //LoadTask(EditTaskId);
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
            else
            {
                //TODO get context e go back
            }
        }

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
            if (EditTaskId == -1)
            {
                //TODO - tratar excepção
                //HACK: User MethodHanlder para Isto - GANHOTO
                MessageBox.Show(Languages.Language.InexistentTask);
                return;
            }

            var mhResult = new MethodHandler();

            try
            {
                TaskAdapter TaskToEdit = new TaskAdapter();

                TaskToEdit.TaskId = EditTaskId;
                TaskToEdit.TaskName = TxtTaskName.Text;
                TaskToEdit.Description = TxtDescription.Text;
                TaskToEdit.Times = new ObservableCollection<TaskTimeAdapter>();

                foreach (var time in taskTimesAdapterUI)
                    TaskToEdit.Times.Add(time);

                mhResult = TaskConnector.EditTask(TaskToEdit);

                btnCancel_Click(sender, e);

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
            }
        }

        void OnTimeDeleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => taskTimesAdapterUI.Remove((TaskTimeAdapterUI)sender)));
        }

    }
}
