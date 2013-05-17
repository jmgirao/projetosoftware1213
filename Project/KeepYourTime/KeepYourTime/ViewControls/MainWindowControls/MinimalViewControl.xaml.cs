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

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Interaction logic for MinimalViewControl.xaml
    /// </summary>
    public partial class MinimalViewControl : UserControl
    {
        public MinimalViewControl()
        {
            InitializeComponent();
            btnFechar.Click += btnFechar_Click;
            btnAdd.Click += btnAdd_Click;
        }

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
        void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public delegate void TaskCreatedHandler(TaskAdapter Task);

        public event TaskCreatedHandler OnTaskCreated;

    }
}
