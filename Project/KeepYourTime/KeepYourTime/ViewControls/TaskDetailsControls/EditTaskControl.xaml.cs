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
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;

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
        long EditTaskId = -1; //Default value indicating not initialized

        public EditTask()
        {
            InitializeComponent();

            this.Loaded += EditTaskControl_Loaded;
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click; 
        }


        private void EditTaskControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }


        private void InitializeControl()
        {
            LoadTask(1);
        }

       /// <summary>
       /// loads all the information from a certain task
       /// </summary>
       /// <param name="TaskID">identifier of the task</param>
        /// <remarks>
        /// CREATED BY Carla Machado
        /// </remarks> 
        private void LoadTask(int TaskID)
        {            
            var mhResult = new MethodHandler();          
            TaskAdapter taskToEdit = new TaskAdapter();

            TaskID = 2;
            try
            {
                mhResult = TaskConnector.ReadTask(TaskID, out taskToEdit);
                
                if (mhResult.Exits)
                {
                    MessageBox.Show(mhResult.Message);
                    return;
                }


                //Test data
                //taskToEdit.TaskName = "teste";
                //taskToEdit.Description = "descrição";
                taskToEdit.Times = new ObservableCollection<TaskTimeAdapter>();
                taskToEdit.Times.Add(new TaskTimeAdapter() { TimeId = 1, TaskId = 1, StartTime = DateTime.Today, StopTime = DateTime.Now });
                taskToEdit.Times.Add(new TaskTimeAdapter() { TimeId = 2, TaskId = 2, StartTime = new DateTime(2013, 4, 12, 12, 00, 00), StopTime = new DateTime(2013, 4, 12, 12, 30, 00) });
                taskToEdit.Times.Add(new TaskTimeAdapter() { TimeId = 3, TaskId = 3, StartTime = new DateTime(2013, 4, 12, 12, 00, 00), StopTime = new DateTime(2013, 4, 12, 12, 00, 00) });


                EditTaskId = taskToEdit.TaskId;
                TxtTaskName.Text = taskToEdit.TaskName;
                TxtDescription.Text = taskToEdit.Description;

                taskTimesAdapterUI = new ObservableCollection<TaskTimeAdapterUI>();
                foreach (TaskTimeAdapter tta in taskToEdit.Times)
                    taskTimesAdapterUI.Add(new TaskTimeAdapterUI(tta));

                dgTaskTimes.ItemsSource = taskTimesAdapterUI;                               

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
            }
            
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
                MessageBox.Show("Sem Tarefa - alterar msg");
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
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                MessageBox.Show(mhResult.Message);
            }
        }


    }
}
