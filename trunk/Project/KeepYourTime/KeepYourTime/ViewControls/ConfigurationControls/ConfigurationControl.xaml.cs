using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.Utils;
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

namespace KeepYourTime.ViewControls.ConfigurationControls
{
    /// <summary>
    /// Interaction logic for ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {
        public ConfigurationControl()
        {
            InitializeComponent();
            btCancel.Click += btCancel_Click;
            btApply.Click += btApply_Click;
            this.Loaded += ConfigurationControl_Loaded;

        }

        void ConfigurationControl_Loaded(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                ObservableCollection<TaskAdapter> taskList;
                var lstCombo = new ObservableCollection<ConfigTaskComboShortcut>();
                mhResult = TaskConnector.ReadTaskList(out taskList, false);
                //cbShort1.Items.Add("");
                //cbShort2.Items.Add("");
                //cbShort3.Items.Add("");
                //cbShort4.Items.Add("");
                //cbShort5.Items.Add("");

                foreach (TaskAdapter t in taskList)
                {
                    lstCombo.Add(new ConfigTaskComboShortcut { TaskID = t.TaskId, TaskName = t.TaskName });

                    //cbShort1.Items.Add(taskList[i].TaskName);
                    //cbShort2.Items.Add(taskList[i].TaskName);
                    //cbShort3.Items.Add(taskList[i].TaskName);
                    //cbShort4.Items.Add(taskList[i].TaskName);
                    //cbShort5.Items.Add(taskList[i].TaskName);
                }

                cbShort1.ItemsSource = lstCombo;
                cbShort2.ItemsSource = lstCombo;
                cbShort3.ItemsSource = lstCombo;
                cbShort4.ItemsSource = lstCombo;
                cbShort5.ItemsSource = lstCombo;

                //if (cbShort1.SelectedItem != null)
                //    ((ConfigTaskComboShortcut)cbShort1.SelectedItem).TaskID = 0;

                ConfigurationAdapter configuration;
                mhResult = ConfigurationConnector.ReadConfiguration(out configuration);
                List<ShortcutAdapter> listShortcuts = configuration.Shortcuts;

                //TODO must select the task name for all the combo boxes

                chkShift1.IsChecked = listShortcuts[0].Shift;
                chkShift2.IsChecked = listShortcuts[1].Shift;
                chkShift3.IsChecked = listShortcuts[2].Shift;
                chkShift4.IsChecked = listShortcuts[3].Shift;
                chkShift5.IsChecked = listShortcuts[4].Shift;

                chkCtrl1.IsChecked = listShortcuts[0].Ctrl;
                chkCtrl2.IsChecked = listShortcuts[1].Ctrl;
                chkCtrl3.IsChecked = listShortcuts[2].Ctrl;
                chkCtrl4.IsChecked = listShortcuts[3].Ctrl;
                chkCtrl5.IsChecked = listShortcuts[4].Ctrl;

                chkAlt1.IsChecked = listShortcuts[0].Alt;
                chkAlt2.IsChecked = listShortcuts[1].Alt;
                chkAlt3.IsChecked = listShortcuts[2].Alt;
                chkAlt4.IsChecked = listShortcuts[3].Alt;
                chkAlt5.IsChecked = listShortcuts[4].Alt;

                txtShortcutKey1.Text = listShortcuts[0].ShortcutKey.ToString();
                txtShortcutKey2.Text = listShortcuts[1].ShortcutKey.ToString();
                txtShortcutKey3.Text = listShortcuts[2].ShortcutKey.ToString();
                txtShortcutKey4.Text = listShortcuts[3].ShortcutKey.ToString();
                txtShortcutKey5.Text = listShortcuts[4].ShortcutKey.ToString();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            { 
                //TODO Mostrar a mensagem
            }
        }

        void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btExportData_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)| All Files (*.*)";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                CsvExporter.ExportDatabaseToCSV(dialog.FileName);

            }
        }

        private bool isValideShortcuts()
        {
            if (cbShort1.SelectedIndex != 0 && txtShortcutKey1.Text.Trim() == "")
                return false;
            if (cbShort2.SelectedIndex != 0 && txtShortcutKey2.Text.Trim() == "")
                return false;
            if (cbShort3.SelectedIndex != 0 && txtShortcutKey3.Text.Trim() == "")
                return false;
            if (cbShort4.SelectedIndex != 0 && txtShortcutKey4.Text.Trim() == "")
                return false;
            if (cbShort5.SelectedIndex != 0 && txtShortcutKey5.Text.Trim() == "")
                return false;
            return true;
        }

        private void btApply_Click(object sender, RoutedEventArgs e)
        {
            if (!isValideShortcuts())
            {
                MessageBox.Show("One shortcut appear to be incomplete, you need to choose any character for it",
                    "Incomplete shortcut");
                return;
            }


            var mhResult = new MethodHandler();

            //TODO taskid must be correctly "generated/readed" and be saved to list
            try
            {
                List<ShortcutAdapter> listShortcuts = new List<ShortcutAdapter>();
                listShortcuts.Add(new ShortcutAdapter
                {
                    TaskId = 1,
                    Shift = (chkShift1.IsChecked.HasValue) ? chkShift1.IsChecked.Value : false,
                    Ctrl = (chkCtrl1.IsChecked.HasValue) ? chkCtrl1.IsChecked.Value : false,
                    Alt = (chkAlt1.IsChecked.HasValue) ? chkAlt1.IsChecked.Value : false,
                    ShortcutKey = txtShortcutKey1.Text[0]
                });
                listShortcuts.Add(new ShortcutAdapter
                {
                    TaskId = 1,
                    Shift = (chkShift2.IsChecked.HasValue) ? chkShift2.IsChecked.Value : false,
                    Ctrl = (chkCtrl2.IsChecked.HasValue) ? chkCtrl2.IsChecked.Value : false,
                    Alt = (chkAlt2.IsChecked.HasValue) ? chkAlt2.IsChecked.Value : false,
                    ShortcutKey = txtShortcutKey2.Text[0]
                });

                listShortcuts.Add(new ShortcutAdapter
                {
                    TaskId = 1,
                    Shift = (chkShift3.IsChecked.HasValue) ? chkShift3.IsChecked.Value : false,
                    Ctrl = (chkCtrl3.IsChecked.HasValue) ? chkCtrl3.IsChecked.Value : false,
                    Alt = (chkAlt3.IsChecked.HasValue) ? chkAlt3.IsChecked.Value : false,
                    ShortcutKey = txtShortcutKey3.Text[0]
                });

                listShortcuts.Add(new ShortcutAdapter
                {
                    TaskId = 1,
                    Shift = (chkShift4.IsChecked.HasValue) ? chkShift4.IsChecked.Value : false,
                    Ctrl = (chkCtrl4.IsChecked.HasValue) ? chkCtrl4.IsChecked.Value : false,
                    Alt = (chkAlt4.IsChecked.HasValue) ? chkAlt4.IsChecked.Value : false,
                    ShortcutKey = txtShortcutKey4.Text[0]
                });

                listShortcuts.Add(new ShortcutAdapter
                {
                    TaskId = 1,
                    Shift = (chkShift5.IsChecked.HasValue) ? chkShift5.IsChecked.Value : false,
                    Ctrl = (chkCtrl5.IsChecked.HasValue) ? chkCtrl5.IsChecked.Value : false,
                    Alt = (chkAlt5.IsChecked.HasValue) ? chkAlt5.IsChecked.Value : false,
                    ShortcutKey = txtShortcutKey5.Text[0]
                });

                var cf = new ConfigurationAdapter();
                cf.Inactivity = (chkInactivityAlert.IsChecked.HasValue) ? chkInactivityAlert.IsChecked.Value : false;
                //cf.InactivityTime = 5;
                cf.Shortcuts = listShortcuts;
                mhResult = ConfigurationConnector.SaveConfiguration(cf);

                Window.GetWindow(this).Close();
            }
            catch (Exception ex1)
            {
                mhResult.Exception(ex1);
            }
        }

    }
}
