using System;
using System.Windows;
using System.Windows.Controls;
using KeepYourTime.Utils;


namespace KeepYourTime.ViewControls.TaskDetailsControls
{
    /// <summary>
    /// Interaction logic for EditTimeLineControl.xaml
    /// </summary>
    /// <remarks>
    /// Created by Carla Machado
    /// </remarks>
    public partial class EditTimeLineControl : UserControl
    {
        public EditTimeLineControl()
        {
            InitializeComponent();
            TxtStartDate.ValueChanged +=TxtStartDate_ValueChanged; 
            TxtEndDate.ValueChanged +=TxtEndDate_ValueChanged;            
        }

        private void TxtEndDate_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if(!TxtEndDate.IsOpen)
                TxtTimeSpent.Text = CalculateTimeSpent(TxtEndDate.Value, TxtStartDate.Value);             
        }

        void TxtStartDate_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if(!TxtStartDate.IsOpen)
                TxtTimeSpent.Text = CalculateTimeSpent(TxtEndDate.Value, TxtStartDate.Value);
        }

        public string CalculateTimeSpent(DateTime? dtStopTime, DateTime? dtStartTime)
        {
            if (dtStartTime != null & dtStopTime != null)
            {
                DateTime dtStartTimeLocal = (DateTime)dtStartTime;
                DateTime dtStoptTimeLocal = (DateTime)dtStopTime;

                TimeSpan tsTimeSpent = dtStoptTimeLocal.Subtract(dtStartTimeLocal);

                return tsTimeSpent.Hours + ":" + tsTimeSpent.Minutes + ":" + tsTimeSpent.Seconds;
            }
            return "";
        }

        private void btnRemoveTime_Click(object sender, System.Windows.RoutedEventArgs e)
        {            
            if (OnTimeDeleted != null)
                OnTimeDeleted(this.DataContext, new EventArgs());
        }

        public event EventHandler OnTimeDeleted;
        
    }
}
