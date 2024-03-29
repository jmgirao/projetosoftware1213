﻿using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.Utils;
using KeepYourTime.ViewWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
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

                lstCombo.Add(new ConfigTaskComboShortcut { TaskID = -1, TaskName = " " });

                foreach (TaskAdapter t in taskList)
                {
                    lstCombo.Add(new ConfigTaskComboShortcut { TaskID = t.TaskId, TaskName = t.TaskName });
                }

                cbShort1.ItemsSource = lstCombo;
                cbShort2.ItemsSource = lstCombo;
                cbShort3.ItemsSource = lstCombo;
                cbShort4.ItemsSource = lstCombo;
                cbShort5.ItemsSource = lstCombo;

                ConfigurationAdapter configuration;
                mhResult = ConfigurationConnector.ReadConfiguration(out configuration);
                List<ShortcutAdapter> listShortcuts = configuration.Shortcuts;

                chkInactivityAlert.IsChecked = configuration.Inactivity;
                udInactiveTime.Value = configuration.InactivityTime;


                var taskCombo = lstCombo.FirstOrDefault((t) => t.TaskID == listShortcuts[0].TaskId);
                if (taskCombo != null)
                {
                    cbShort1.SelectedItem = taskCombo;
                    chkShift1.IsChecked = listShortcuts[0].Shift;
                    chkCtrl1.IsChecked = listShortcuts[0].Ctrl;
                    chkAlt1.IsChecked = listShortcuts[0].Alt;
                    txtShortcutKey1.Text = listShortcuts[0].ShortcutKey.ToString()[0].ToString() ;
                }
                else
                {
                    cbShort1.SelectedIndex = 0;
                }

                taskCombo = lstCombo.FirstOrDefault((t) => t.TaskID == listShortcuts[1].TaskId);
                if (taskCombo != null)
                {
                    cbShort2.SelectedItem = taskCombo;
                    chkShift2.IsChecked = listShortcuts[1].Shift;
                    chkCtrl2.IsChecked = listShortcuts[1].Ctrl;
                    chkAlt2.IsChecked = listShortcuts[1].Alt;
                    txtShortcutKey2.Text =  listShortcuts[1].ShortcutKey.ToString();
                }
                else
                {
                    cbShort2.SelectedIndex = 0;
                }


                taskCombo = lstCombo.FirstOrDefault((t) => t.TaskID == listShortcuts[2].TaskId);
                if (taskCombo != null)
                {
                    cbShort3.SelectedItem = taskCombo;
                    chkShift3.IsChecked = listShortcuts[2].Shift;
                    chkCtrl3.IsChecked = listShortcuts[2].Ctrl;
                    chkAlt3.IsChecked = listShortcuts[2].Alt;
                    txtShortcutKey3.Text = listShortcuts[2].ShortcutKey.ToString() ;
                }
                else
                {
                    cbShort3.SelectedIndex = 0;
                }

                taskCombo = lstCombo.FirstOrDefault((t) => t.TaskID == listShortcuts[3].TaskId);
                if (taskCombo != null)
                {
                    cbShort4.SelectedItem = taskCombo;
                    chkShift4.IsChecked = listShortcuts[3].Shift;
                    chkCtrl4.IsChecked = listShortcuts[3].Ctrl;
                    chkAlt4.IsChecked = listShortcuts[3].Alt;
                    txtShortcutKey4.Text = listShortcuts[3].ShortcutKey.ToString() ;
                }
                else
                {
                    cbShort4.SelectedIndex = 0;
                }

                taskCombo = lstCombo.FirstOrDefault((t) => t.TaskID == listShortcuts[4].TaskId);
                if (taskCombo != null)
                {
                    cbShort5.SelectedItem = taskCombo;
                    chkShift5.IsChecked = listShortcuts[4].Shift;
                    chkCtrl5.IsChecked = listShortcuts[4].Ctrl;
                    chkAlt5.IsChecked = listShortcuts[4].Alt;
                    txtShortcutKey5.Text =  listShortcuts[4].ShortcutKey.ToString()[4].ToString() ;
                }
                else
                {
                    cbShort5.SelectedIndex = 0;
                }

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
            if (cbShort1.SelectedIndex > 0 && !isAlphaNumeric(txtShortcutKey1.Text))
                return false;
            if (cbShort2.SelectedIndex > 0 && !isAlphaNumeric(txtShortcutKey2.Text))
                return false;
            if (cbShort3.SelectedIndex > 0 && !isAlphaNumeric(txtShortcutKey3.Text))
                return false;
            if (cbShort4.SelectedIndex > 0 && !isAlphaNumeric(txtShortcutKey4.Text))
                return false;
            if (cbShort5.SelectedIndex > 0 && !isAlphaNumeric(txtShortcutKey5.Text))
                return false;
            return true;
        }

        /// <summary>
        /// Verify if a given string is letter or numeric (alphanumeric)
        /// </summary>
        /// <param name="input">A string to verify.</param>
        public static Boolean isAlphaNumeric(String input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            String shortcutKey = input.Trim();
            if (string.IsNullOrEmpty(shortcutKey))
                return false;

            for (int i = 0; i < shortcutKey.Length; i++)
            {
                if (!(char.IsLetter(shortcutKey[i])) && (!(char.IsNumber(shortcutKey[i]))))
                    return false;
            }
            return true;
        }

        private void btApply_Click(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();

            try
            {
                if (!isValideShortcuts())
                {
                    mhResult.Message = Languages.Language.ShortcutIncomplete;
                    mhResult.Status = MethodStatus.Cancel;
                    return;
                }

                if (chkInactivityAlert.IsChecked == true)
                {
                    if (udInactiveTime.Value <= 0 || udInactiveTime.Value >= 60)
                    {
                        mhResult.Message = Languages.Language.InvalidInactiveTime;
                        mhResult.Status = MethodStatus.Cancel;
                        return;
                    }

                }

                List<ShortcutAdapter> listShortcuts = new List<ShortcutAdapter>();

                if (((ConfigTaskComboShortcut)cbShort1.SelectedItem).TaskID >= 0)
                {
                    listShortcuts.Add(new ShortcutAdapter
                    {
                        TaskId = ((ConfigTaskComboShortcut)cbShort1.SelectedItem).TaskID,
                        ShortcutId = 1,
                        Shift = (chkShift1.IsChecked.HasValue) ? chkShift1.IsChecked.Value : false,
                        Ctrl = (chkCtrl1.IsChecked.HasValue) ? chkCtrl1.IsChecked.Value : false,
                        Alt = (chkAlt1.IsChecked.HasValue) ? chkAlt1.IsChecked.Value : false,
                        ShortcutKey =  txtShortcutKey1.Text.ToLower()
                    });
                }

                if (((ConfigTaskComboShortcut)cbShort2.SelectedItem).TaskID >= 0)
                {
                    listShortcuts.Add(new ShortcutAdapter
                    {
                        TaskId = ((ConfigTaskComboShortcut)cbShort2.SelectedItem).TaskID,
                        ShortcutId = 2,
                        Shift = (chkShift2.IsChecked.HasValue) ? chkShift2.IsChecked.Value : false,
                        Ctrl = (chkCtrl2.IsChecked.HasValue) ? chkCtrl2.IsChecked.Value : false,
                        Alt = (chkAlt2.IsChecked.HasValue) ? chkAlt2.IsChecked.Value : false,
                        ShortcutKey =  txtShortcutKey2.Text.ToLower()
                    });
                }

                if (((ConfigTaskComboShortcut)cbShort3.SelectedItem).TaskID >= 0)
                {
                    listShortcuts.Add(new ShortcutAdapter
                    {
                        TaskId = ((ConfigTaskComboShortcut)cbShort3.SelectedItem).TaskID,
                        ShortcutId = 3,
                        Shift = (chkShift3.IsChecked.HasValue) ? chkShift3.IsChecked.Value : false,
                        Ctrl = (chkCtrl3.IsChecked.HasValue) ? chkCtrl3.IsChecked.Value : false,
                        Alt = (chkAlt3.IsChecked.HasValue) ? chkAlt3.IsChecked.Value : false,
                        ShortcutKey =  txtShortcutKey3.Text.ToLower() 
                    });
                }

                if (((ConfigTaskComboShortcut)cbShort4.SelectedItem).TaskID >= 0)
                {
                    listShortcuts.Add(new ShortcutAdapter
                    {
                        TaskId = ((ConfigTaskComboShortcut)cbShort4.SelectedItem).TaskID,
                        ShortcutId = 4,
                        Shift = (chkShift4.IsChecked.HasValue) ? chkShift4.IsChecked.Value : false,
                        Ctrl = (chkCtrl4.IsChecked.HasValue) ? chkCtrl4.IsChecked.Value : false,
                        Alt = (chkAlt4.IsChecked.HasValue) ? chkAlt4.IsChecked.Value : false,
                        ShortcutKey =  txtShortcutKey4.Text.ToLower() 
                    });
                }

                if (((ConfigTaskComboShortcut)cbShort5.SelectedItem).TaskID >= 0)
                {
                    listShortcuts.Add(new ShortcutAdapter
                    {
                        TaskId = ((ConfigTaskComboShortcut)cbShort5.SelectedItem).TaskID,
                        ShortcutId = 5,
                        Shift = (chkShift5.IsChecked.HasValue) ? chkShift5.IsChecked.Value : false,
                        Ctrl = (chkCtrl5.IsChecked.HasValue) ? chkCtrl5.IsChecked.Value : false,
                        Alt = (chkAlt5.IsChecked.HasValue) ? chkAlt5.IsChecked.Value : false,
                        ShortcutKey =  txtShortcutKey5.Text.ToLower()
                    });
                }


                var cf = new ConfigurationAdapter();
                cf.Inactivity = chkInactivityAlert.IsChecked.Value ;
                cf.InactivityTime = udInactiveTime.Value.Value;
                cf.Shortcuts = listShortcuts;



                mhResult = ConfigurationConnector.SaveConfiguration(cf);
                if (mhResult.Exits) return;

                Utils.CurrentConfigurations.getConfigurations();

                Utils.CurrentConfigurations.ConfigureHotKeys();

                Window.GetWindow(this).Close();
            }
            catch (Exception ex1)
            {
                mhResult.Exception(ex1);
                return;
            }
            finally
            {
                MessageWindow.ShowMethodHandler(mhResult, true);
            }
        }

    }
}
