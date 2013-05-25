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

namespace KeepYourTime.ViewControls.InactivityControls
{
    /// <summary>
    /// Interaction logic for InactivityControl.xaml
    /// </summary>
    public partial class InactivityControl : UserControl
    {
        public InactivityControl()
        {
            InitializeComponent();
            btnAddTime.Click += btnAddTime_Click;
            btnDiscardTime.Click += btnDiscardTime_Click;
        }

        void btnDiscardTime_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnAddTime_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
