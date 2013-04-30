using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeepYourTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SourceInitialized += MainWindow_SourceInitialized;

            InitializeComponent();
            recMove.MouseDown += recMove_MouseDown;
            recSize.MouseDown += recSize_MouseDown;

            if (!DataBase.CreateDB.IsDbCreated())
            {
                var cdb = new DataBase.CreateDB();
                cdb.CreateDatabase();
            }
        }

        void recMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //quando ta graden
        void expand()
        {
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
        }

        void collapse()
        {
            this.ResizeMode = System.Windows.ResizeMode.CanResize;
        }

        #region Resize

        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
        private enum ResizeDirection
        {
            Left = 61441,
            Right = 61442,
            Top = 61443,
            TopLeft = 61444,
            TopRight = 61445,
            Bottom = 61446,
            BottomLeft = 61447,
            BottomRight = 61448,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        void recSize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)ResizeDirection.Right, IntPtr.Zero);
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        #endregion
    }
}
