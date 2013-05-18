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

        public ShowTaskList()
        {

            InitializeComponent();
            this.Loaded += ShowTaskList_Loaded;
        }

        void ShowTaskList_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }


        public void CreatedTask(TaskAdapter Task)
        {
            taskAdaptUi.Add(new TaskAdapterUI(Task));
        }

        public void ReceiveTaskList(ObservableCollection<TaskAdapter> taskAdapt)
        {
            taskAdaptUi = new ObservableCollection<TaskAdapterUI>();
            foreach (TaskAdapter t in taskAdapt)
            {
                taskAdaptUi.Add(new TaskAdapterUI(t));
            }
            dgTaskList.ItemsSource = taskAdaptUi;
        }

        private void InitializeControl()
        {
            var mhResult = new MethodHandler();

            try
            {

                bool blnActive = false;

                if (chkShowActiveTask.IsChecked == true) blnActive = true;
                else blnActive = false;

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

        private void chkActiveTask_Checked(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("Desactiva");
        }

        private void chkActiveTask_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Activa");   
        }
    }
}
