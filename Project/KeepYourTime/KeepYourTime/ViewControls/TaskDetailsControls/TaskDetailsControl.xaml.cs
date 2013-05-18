﻿using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewControls.MainWindowControls;
using KeepYourTime.ViewWindows;
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
    /// <remarks>
    /// CREATED BY João Girão
    /// </remarks>  
    public partial class TaskDetailsControl : UserControl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDetailsControl"/> class.
        /// </summary>
        /// <remarks>CREATED BY João Girão</remarks>
        public TaskDetailsControl()
        {
            InitializeComponent();
            this.Loaded += TaskDetailsControl_Loaded;
            btCloseTaskDetails.Click += btCloseTaskDetails_Click;
        }

        /// <summary>
        /// Handles the Loaded event of the TaskDetailsControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>CREATED BY João Girão</remarks>
        private void TaskDetailsControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        /// <remarks>CREATED BY João Girão</remarks>
        private void InitializeControl()
        {
            var mhResult = new MethodHandler();
            int intIdTask = MinimalViewControl.intTaskID;                 //To receive idTask another window (defined in the MinimalViewControl) 
            TaskAdapter taTaskAdapt = new TaskAdapter();

            try
            {
                mhResult = TaskConnector.ReadTask(intIdTask, out taTaskAdapt);
                if (mhResult.Exits)
                {
                    MessageWindow.ShowMethodHandler(mhResult, true);
                    return;
                }

                string strTaskName = taTaskAdapt.TaskName;
                string strTaskDescription = taTaskAdapt.Description;
                ObservableCollection<TaskTimeAdapter> ocTaskTimeList = taTaskAdapt.Times;

                lbTaskNameText.Text = strTaskName;
                lbTaskDescriptionText.Text = strTaskDescription;

                /*
                 *CREATE TIMES 
                 */
                TaskTimeAdapter tasktime = new TaskTimeAdapter();
                tasktime.TimeId = 1;
                tasktime.TaskId = 1;
                tasktime.StartTime = new DateTime(2013, 4, 12, 12, 00, 00);
                tasktime.StopTime = new DateTime(2013, 4, 12, 12, 30, 00);
                ocTaskTimeList.Add(tasktime);

                tasktime = new TaskTimeAdapter();
                tasktime.TimeId = 2;
                tasktime.TaskId = 1;
                tasktime.StartTime = new DateTime(2013, 2, 13, 1, 00, 00);
                tasktime.StopTime = new DateTime(2013, 2, 13, 6, 30, 00);
                ocTaskTimeList.Add(tasktime);

                dgTaskTimes.ItemsSource = ocTaskTimeList;
                

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageWindow.ShowMethodHandler(mhResult, true);
            }

        }

        /// <summary>
        /// Handles the Click event of the btCloseTaskDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>CREATED BY João Girão</remarks>
        private void btCloseTaskDetails_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }

}