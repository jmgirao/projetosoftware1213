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
using System.Windows.Shapes;

namespace KeepYourTime.ViewWindows
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
            this.Loaded += MessageWindow_Loaded;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = 50;
            btnClick.Click += btnClick_Click;
        }

        void btnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void MessageWindow_Loaded(object sender, RoutedEventArgs e)
        {

            this.DataContext = mhResult;
            if (mhResult.Status != Utils.MethodStatus.Exception)
                HideMessage(5000);
        }

        MethodHandler mhResult = null;

        public static void ShowMethodHandler(MethodHandler Result, bool ShowSucess)
        {
            if (Result.Status == Utils.MethodStatus.Sucess && ShowSucess == false) return;

            var winMessage = new MessageWindow();
            winMessage.mhResult = Result;
            winMessage.Show();
        }


        async void HideMessage(int Time)
        {
            await Task.Delay(Time);
            this.Close();
        }
    }
}
