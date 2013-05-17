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
        public ShowTaskList()
        {
            
            InitializeComponent();
            //InitializeControl();
            this.Loaded += ShowTaskList_Loaded;
        }

        void ShowTaskList_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControl();
        }

        private void InitializeControl() { 
            var mhResult = new MethodHandler();
            ObservableCollection<TaskAdapter> taskAdapt = new ObservableCollection<TaskAdapter>();
            bool blnActive=false;
            
            try
            {
                mhResult=TaskConnector.ReadTaskList(out taskAdapt, blnActive);

                //tratar mhResult

                //codigo dgTaskList 
                //dgTaskList.
                dgTaskList.ItemsSource = taskAdapt;

            }
            catch (Exception e) {
                mhResult.Exception(e);
                MessageBox.Show(mhResult.Message);
            }
        }
    }
}
