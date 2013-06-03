using System;
using System.Windows.Controls;


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
        /// <summary>
        /// Constructor
        /// </summary>
        public EditTimeLineControl()
        {
            InitializeComponent();
                    
        }

        /// <summary>
        /// remove time line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveTime_Click(object sender, System.Windows.RoutedEventArgs e)
        {            
            if (OnTimeDeleted != null)
                OnTimeDeleted(this.DataContext, new EventArgs());
        }

        public event EventHandler OnTimeDeleted;
        
    }
}
