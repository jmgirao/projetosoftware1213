using System;
using System.Collections.Generic;
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
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.ViewWindows;
using KeepYourTime.ViewControls.ConfigurationControls;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Interaction logic for MinimalViewControl.xaml
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public partial class MinimalViewControl : UserControl
    {
        //public static long TaskID = 0;    //identify task to select the task data
        private TaskTimer ttTaskTimer;

        Hooks.ActivityHook ahInactivity;

        public static long CurrentTaskId = -1;

        public static event EventHandler OnTaskListChanged;
        public static void RaiseEventOnTaskListChanged()
        {
            if (OnTaskListChanged != null) OnTaskListChanged(null, new EventArgs());
        }

        public MinimalViewControl()
        {
            InitializeComponent();
            btnConfig.Click += btnConfig_Click;
            btnFechar.Click += btnFechar_Click;
            btnAdd.Click += btnAdd_Click;
            btnStop.Click += btnStop_Click;
            btnTaskDetails.Click += btnTaskDetails_Click;
            ttTaskTimer = new TaskTimer();
            ttTaskTimer.onTimeChanged += ttTaskTimer_onTimeChanged;

            ahInactivity = new Hooks.ActivityHook();
            ahInactivity.InactiveTimeRefresh += ahInactivity_InactiveTimeRefresh;
            this.Loaded += MinimalViewControl_Loaded;
            MinimalViewControl.OnTaskListChanged += MinimalViewControl_OnTaskListChanged;
        }

        void MinimalViewControl_OnTaskListChanged(object sender, EventArgs e)
        {
            var lstTaskID = new List<ConfigTaskComboShortcut>();

            foreach (TaskAdapter t in MainWindow.lstTaskAdapt)
            {
                if (t.Active)
                    lstTaskID.Add(new ConfigTaskComboShortcut() { TaskID = t.TaskId, TaskName = t.TaskName });
            }
            txtNomeTask.ItemsSource = lstTaskID;
        }

        void MinimalViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            ahInactivity.InitTimer();
        }

        void ttTaskTimer_onTimeChanged(string time)
        {
            Dispatcher.BeginInvoke((Action)(() => lblTempoDecorrido.Text = time));
        }

        #region basic buttons

        void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            var configWindow = new ConfigurationWindow();
            configWindow.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btnTaskDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>CREATED BY João Girão</remarks>
        void btnTaskDetails_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTaskId != -1)
            {
                TaskDetailsWindow.TaskID = CurrentTaskId;  //The task id that's running or that's selected in the textbox
                var detailswindows = new TaskDetailsWindow();
                detailswindows.Show();
            }
        }

        void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region custom events

        public delegate void TaskCreatedHandler(TaskAdapter Task);

        public event TaskCreatedHandler OnTaskCreated;

        #endregion

        #region task execution

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                long taskId = 0;
                var taTask = new TaskAdapter();
                taTask.TaskName = txtNomeTask.Text;
                taTask.Active = true;
                mhResult = TaskConnector.CreateTask(taTask.TaskName, out taskId);
                if (mhResult.Exits) return;

                taTask.TaskId = taskId;

                if (OnTaskCreated != null)
                {
                    OnTaskCreated(taTask);
                }
                StartTask(taskId, 0);
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

        void btnStop_Click(object sender, RoutedEventArgs e)
        {
            CurrentTaskId = -1;
            StopTask(0);
        }

        public MethodHandler StartTask(long TaskID, int RemoveSeconds)
        {
            var mhResult = new MethodHandler();
            try
            {

                if (CurrentTaskId != -1)
                    StopTask(0);


                mhResult = ttTaskTimer.StartTimingTask(TaskID, RemoveSeconds);
                if (mhResult.Exits) return mhResult;

                CurrentTaskId = TaskID;

                txtNomeTask.Text = ttTaskTimer.TaskName;

                btnStop.Visibility = System.Windows.Visibility.Visible;
                btnAdd.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, false);
            }
            return mhResult;
        }

        /// <summary>
        /// Ahes the inactivity_ inactive time refresh.
        /// </summary>
        /// <param name="InactiveSeconds">The inactive seconds.</param>
        /// <remarks>CREATED BY João Girão</remarks>
        void ahInactivity_InactiveTimeRefresh(int InactiveSeconds)
        {
            var mhResult = new MethodHandler();
            mhResult.Message = InactiveSeconds + "";
            //Dispatcher.BeginInvoke((Action)(() => MessageWindow.ShowMethodHandler(mhResult,true)));

            //if (Utils.CurrentConfigurations.allConfig.Inactivity == true)
            if (InactivityDetection.CheckInactive(InactiveSeconds))
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    var mhResultDispatcher = new MethodHandler();
                    try
                    {
                        TaskTimeAdapter taTime = ttTaskTimer.StopTimingTask(InactivityDetection.RemoveSeconds);
                        mhResultDispatcher = TaskConnector.AddTime(taTime);
                        if (mhResultDispatcher.Exits) return;
                        ttTaskTimer.StartTimingTask(CurrentTaskId, InactivityDetection.RemoveSeconds);

                        var inactWindow = new InactivityWindow();
                        inactWindow.ShowDialog();

                        if (InactivityControls.InactivityControl.blnDiscardTime)
                        {
                            ttTaskTimer.StopTimingTask(0);
                            ttTaskTimer.StartTimingTask(CurrentTaskId, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        mhResultDispatcher.Exception(ex);
                    }
                    finally
                    {
                        MessageWindow.ShowMethodHandler(mhResultDispatcher, false);
                    }
                }));
            }


        }

        public void StopTask(int RemoveSeconds)
        {
            var mhResult = new MethodHandler();
            try
            {
                if (ttTaskTimer.isRunningTask())
                {
                    mhResult = TaskConnector.AddTime(ttTaskTimer.StopTimingTask(RemoveSeconds));
                    if (mhResult.Exits) return;
                }
                btnStop.Visibility = System.Windows.Visibility.Collapsed;
                btnAdd.Visibility = System.Windows.Visibility.Visible;
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

        #endregion
    }
}
