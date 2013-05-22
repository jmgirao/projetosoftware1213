using System;
using System.Data;
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
using KeepYourTime.DataBase;
using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;

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
            taskAdaptUi.Add(new TaskAdapterUI(Task));
        }

        /// <summary>
        /// Receives the task list.
        /// </summary>
        /// <param name="taskAdapt">The task adapt.</param>
        public void ReceiveTaskList(ObservableCollection<TaskAdapter> taskAdapt)
        {
            taskAdaptUi = new ObservableCollection<TaskAdapterUI>();
            foreach (TaskAdapter t in taskAdapt)
            {
                var ta = new TaskAdapterUI(t);
                taskAdaptUi.Add(ta);
                ta.OnTaskDeactivated += ta_OnTaskDeactivated;
            }
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
    }
}
