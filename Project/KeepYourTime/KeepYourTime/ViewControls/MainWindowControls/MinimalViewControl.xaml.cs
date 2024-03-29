﻿using System;
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
using KeepYourTime.Utils;
using System.Threading;
using System.Collections.ObjectModel;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Interaction logic for MinimalViewControl.xaml
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public partial class MinimalViewControl : UserControl
    {
        //public static long TaskID = 0;    //identify task to select the task data
        public static TaskTimer ttTaskTimer;
        public static long CurrentTaskId = -1;

        Hooks.ActivityHook ahInactivity;

        ObservableCollection<ConfigTaskComboShortcut> lstTaskID;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimalViewControl"/> class.
        /// </summary>
        public MinimalViewControl()
        {
            InitializeComponent();
            btnConfig.Click += btnConfig_Click;
            btnFechar.Click += btnFechar_Click;
            btnAdd.Click += btnAdd_Click;
            btnStop.Click += btnStop_Click;
            btnResume.Click += btnResume_Click;
            btnTaskDetails.Click += btnTaskDetails_Click;
            btnEdit.Click += btnEdit_Click;

            btnAdd.IsEnabled = false;

            ttTaskTimer = new TaskTimer();
            ttTaskTimer.onTimeChanged += ttTaskTimer_onTimeChanged;

            ahInactivity = new Hooks.ActivityHook();
            ahInactivity.InactiveTimeRefresh += ahInactivity_InactiveTimeRefresh;
            this.Loaded += MinimalViewControl_Loaded;
            StaticEvents.OnTaskListChanged += MinimalViewControl_OnTaskListChanged;
            StaticEvents.OnTaskDeleted += StaticEvents_OnTaskDeleted;
            txtNomeTask.SelectionChanged += txtNomeTask_SelectionChanged;

            StaticEvents.OnStartTaskPressed += StaticEvents_OnStartTaskPressed;
            StaticEvents.OnStopTaskPressed += StaticEvents_OnStopTaskPressed;
            StaticEvents.OnTaskUpdated += StaticEvents_OnTaskUpdated;

        }

        void StaticEvents_OnTaskUpdated(long TaskID)
        {
            int intSelectedIndex = txtNomeTask.SelectedIndex;
            txtNomeTask.ItemsSource = null;
            txtNomeTask.ItemsSource = lstTaskID;
            txtNomeTask.SelectedIndex = intSelectedIndex;
        }

        void StaticEvents_OnStopTaskPressed(long TaskID)
        {
            CurrentTaskId = -1;
            StopTask(0);
        }

        void StaticEvents_OnStartTaskPressed(long TaskID)
        {
            var mhResult = new MethodHandler();
            try
            {
                if (ttTaskTimer.isRunningTask())
                {
                    mhResult = TaskConnector.AddTime(ttTaskTimer.StopTimingTask(0));
                    if (mhResult.Exits) return;
                }
                //txtNomeTask.SelectedItem = "";
                //var x = txtNomeTask.SelectedItem as ConfigTaskComboShortcut;

                ConfigTaskComboShortcut cts = lstTaskID.FirstOrDefault((x) => { return x.TaskID == TaskID; });
                txtNomeTask.SelectedItem = cts;

                StartTask(TaskID, 0);
                btnResume.Visibility = Visibility.Collapsed;
                btnStop.Visibility = Visibility.Visible;
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

        void StaticEvents_OnTaskDeleted(long TaskID)
        {
            var taTask = lstTaskID.FirstOrDefault((t) => t.TaskID == TaskID);
            if (taTask != null)
                lstTaskID.Remove(taTask);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the txtNomeTask control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        void txtNomeTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtNomeTask.Text == "" && e.AddedItems.Count != 1)
            {

                txtNomeTask.SelectedIndex = -1;
                if (ttTaskTimer.isRunningTask())
                {
                    btnStop.Visibility = Visibility.Visible;
                    btnAdd.Visibility = Visibility.Collapsed;
                    btnResume.Visibility = Visibility.Collapsed;
                    btnEdit.IsEnabled = false;
                    btnTaskDetails.IsEnabled = true;
                }
                else
                {
                    btnStop.Visibility = Visibility.Collapsed;
                    btnAdd.Visibility = Visibility.Visible;
                    btnResume.Visibility = Visibility.Collapsed;
                    btnEdit.IsEnabled = false;
                    btnTaskDetails.IsEnabled = false;
                }

            }
            else
            {
                if (txtNomeTask.SelectedItem == null)
                {
                    btnAdd.Visibility = Visibility.Visible;
                    btnStop.Visibility = Visibility.Collapsed;
                    btnResume.Visibility = Visibility.Collapsed;
                    btnEdit.IsEnabled = false;
                    btnTaskDetails.IsEnabled = false;
                }
                else
                {
                    var selectedTask = txtNomeTask.SelectedItem as ConfigTaskComboShortcut;
                    if (selectedTask.TaskID == CurrentTaskId)
                    {
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnStop.Visibility = Visibility.Visible;
                        btnResume.Visibility = Visibility.Collapsed;
                        btnEdit.IsEnabled = false;
                        btnTaskDetails.IsEnabled = true;
                    }
                    else
                    {
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnStop.Visibility = Visibility.Collapsed;
                        btnResume.Visibility = Visibility.Visible;
                        btnEdit.IsEnabled = true;
                        btnTaskDetails.IsEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the OnTaskListChanged event of the MinimalViewControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void MinimalViewControl_OnTaskListChanged(object sender, EventArgs e)
        {
            lstTaskID = new ObservableCollection<ConfigTaskComboShortcut>();

            foreach (TaskAdapter t in MainWindow.lstTaskAdapt)
            {
                if (t.Active)
                    lstTaskID.Add(new ConfigTaskComboShortcut() { TaskID = t.TaskId, TaskName = t.TaskName });
            }
            txtNomeTask.ItemsSource = lstTaskID;
        }

        /// <summary>
        /// Handles the Loaded event of the MinimalViewControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void MinimalViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            ahInactivity.InitTimer();
        }

        /// <summary>
        /// Tts the task timer_on time changed.
        /// </summary>
        /// <param name="time">The time.</param>
        void ttTaskTimer_onTimeChanged(string time)
        {
            Dispatcher.BeginInvoke((Action)(() => lblTempoDecorrido.Text = time));
        }

        #region basic buttons

        /// <summary>
        /// Handles the Click event of the btnConfig control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
            if (txtNomeTask.SelectedIndex != -1)
            {

                TaskDetailsWindow.TaskID = lstTaskID[txtNomeTask.SelectedIndex].TaskID;  //The task id that's running or that's selected in the textbox
                var detailswindows = new TaskDetailsWindow();
                detailswindows.ShowDialog();
            }
        }


        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomeTask.SelectedIndex != -1)
            {
                if (lstTaskID[txtNomeTask.SelectedIndex].TaskID != CurrentTaskId)
                {
                    TaskDetailsWindow.TaskID = lstTaskID[txtNomeTask.SelectedIndex].TaskID;  //The task id that's running or that's selected in the textbox

                    var detailswindows = new TaskDetailsWindow();
                    detailswindows.TaskDetails.Visibility = System.Windows.Visibility.Collapsed;
                    detailswindows.EditTask.Visibility = System.Windows.Visibility.Visible;
                    detailswindows.EditTask.LoadTask(TaskDetailsWindow.TaskID);
                    detailswindows.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnFechar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region custom events

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Task">The task.</param>
        //private delegate void TaskCreatedHandler(TaskAdapter Task);

        ///// <summary>
        ///// Occurs when [on task created].
        ///// </summary>
        //private event TaskCreatedHandler OnTaskCreated;

        #endregion

        #region task execution

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                if (txtNomeTask.Text.Trim() == "")
                {
                    mhResult.Message = Languages.Language.TaskMustHaveName;
                    mhResult.Status = MethodStatus.Cancel;
                    return;
                }

                long taskId = 0;
                var taTask = new TaskAdapter();
                taTask.TaskName = txtNomeTask.Text;
                taTask.Active = true;
                mhResult = TaskConnector.CreateTask(taTask.TaskName, out taskId);
                if (mhResult.Exits) return;

                taTask.TaskId = taskId;
                lstTaskID.Add(new ConfigTaskComboShortcut() { TaskID = taTask.TaskId, TaskName = taTask.TaskName });

                txtNomeTask.SelectedIndex = lstTaskID.Count - 1;

                StaticEvents.RaiseEventOnTaskCreated(taTask);
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

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void btnStop_Click(object sender, RoutedEventArgs e)
        {
            CurrentTaskId = -1;
            StopTask(0);
        }

        /// <summary>
        /// Handles the Click event of the btnResume control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void btnResume_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                if (ttTaskTimer.isRunningTask())
                {
                    mhResult = TaskConnector.AddTime(ttTaskTimer.StopTimingTask(0));
                    if (mhResult.Exits) return;
                }
                var x = txtNomeTask.SelectedItem as ConfigTaskComboShortcut;
                StartTask(x.TaskID, 0);
                btnResume.Visibility = Visibility.Collapsed;
                btnStop.Visibility = Visibility.Visible;
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
        /// Starts the task.
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="RemoveSeconds">The remove seconds.</param>
        /// <returns></returns>
        private MethodHandler StartTask(long TaskID, int RemoveSeconds)
        {
            var mhResult = new MethodHandler();
            try
            {

                if (CurrentTaskId == TaskID && RemoveSeconds == 0)
                    return mhResult;

                if (CurrentTaskId != -1)
                    StopTask(0);


                mhResult = ttTaskTimer.StartTimingTask(TaskID, RemoveSeconds);
                if (mhResult.Exits) return mhResult;

                CurrentTaskId = TaskID;

                txtNomeTask.Text = ttTaskTimer.TaskName;

                btnStop.Visibility = System.Windows.Visibility.Visible;
                btnAdd.Visibility = System.Windows.Visibility.Collapsed;
                btnResume.Visibility = Visibility.Collapsed;

                StaticEvents.RaiseEventOnTaskStarted(TaskID);
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

        private void StopTaskUI()
        {
            btnStop.Visibility = System.Windows.Visibility.Collapsed;
            btnAdd.Visibility = System.Windows.Visibility.Collapsed;
            btnResume.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Ahes the inactivity_ inactive time refresh.
        /// </summary>
        /// <param name="InactiveSeconds">The inactive seconds.</param>
        /// <remarks>
        /// CREATED BY João Girão
        /// </remarks>
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

                        InactivityControls.InactivityControl.InactiveTime = InactivityDetection.RemoveSeconds / 60;
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

        /// <summary>
        /// Stops the task.
        /// </summary>
        /// <param name="RemoveSeconds">The remove seconds.</param>
        private void StopTask(int RemoveSeconds)
        {
            var mhResult = new MethodHandler();
            try
            {
                CurrentTaskId = -1;
                if (ttTaskTimer.isRunningTask())
                {
                    mhResult = TaskConnector.AddTime(ttTaskTimer.StopTimingTask(RemoveSeconds));
                    if (mhResult.Exits) return;
                }
                btnStop.Visibility = System.Windows.Visibility.Collapsed;
                btnAdd.Visibility = System.Windows.Visibility.Collapsed;
                btnResume.Visibility = Visibility.Visible;
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

        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => btnAdd.IsEnabled = txtNomeTask.Text != ""));
        }

        private void PART_EditableTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtNomeTask.Text.Trim() != string.Empty)
            {
                if (btnAdd.IsVisible)
                {
                    btnAdd_Click(null, null);
                }
                else if (btnResume.IsVisible)
                {
                    btnResume_Click(null, null);
                }
            }
        }

    }
}
