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
        public EditTask()
        {
            InitializeComponent();
        }

       /// <summary>
       /// loads all the information from a certain task
       /// </summary>
       /// <param name="TaskID">identifier of the task</param>
        /// <remarks>
        /// CREATED BY Carla Machado
        /// </remarks> 
        public void LoadTask(int TaskID)
        {            
            var mhResult = new MethodHandler();          
            TaskAdapter taskToEdit = new TaskAdapter();

            try
            {
                mhResult = TaskConnector.ReadTask(TaskID, out taskToEdit);
                if (mhResult.Exits)
                {
                    MessageBox.Show(mhResult.Message);
                    return;
                }

                string taskName = taskToEdit.TaskName;
                string taskDescription = taskToEdit.Description;
                ObservableCollection<TaskTimeAdapter> taskTimeList = taskToEdit.Times;

                TxtTaskName.Text = taskToEdit.TaskName;
                TxtDescription.Text = taskToEdit.Description;
               //TODO - task times


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
            var mhResult = new MethodHandler();  

            try
            {
                TaskAdapter TaskToEdit = new TaskAdapter();                             

                TaskToEdit.TaskName = TxtTaskName.Text;
                TaskToEdit.Description = TxtDescription.Text;


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
