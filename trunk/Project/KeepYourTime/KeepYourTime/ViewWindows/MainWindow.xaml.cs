using KeepYourTime.Utils;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeepYourTime.ViewWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool blnColappsed = false;
        //Hooks.ActivityHook ahActivityAnalyzer;

        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MainWindow_SourceInitialized;
            this.Loaded += MainWindow_Loaded;

            recMove.MouseDown += recMove_MouseDown;
            recSize.MouseDown += recSize_MouseDown;
            mvMinimalView.OnTaskCreated += mvMinimalView_OnTaskCreated;
            btnExpandir.Click += btnExpandir_Click;
            // ahActivityAnalyzer = new Hooks.ActivityHook();
            // ahActivityAnalyzer.InitTimer();
            // ahActivityAnalyzer.InactiveTimeRefresh += hk_InactiveTimeRefresh;
        }

        void hk_InactiveTimeRefresh(int InactiveSeconds)
        {
            //Dispatcher.BeginInvoke((Action)(() => mvMinimalView.txtNomeTask.Text = InactiveSeconds.ToString()));
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            //ahActivityAnalyzer.StopTimer();
        }

        Storyboard sbShowTaskList;
        Storyboard sbHideTaskList;

        void btnExpandir_Click(object sender, RoutedEventArgs e)
        {
            if (blnColappsed)
            {
                expand();
            }
            else
            {
                collapse();
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var mhResult = new MethodHandler();
            try
            {
                if (!DataBase.CreateDB.IsDatabaseCreated())
                {
                    var cdb = new DataBase.CreateDB();
                    mhResult = cdb.CreateDatabase();
                    if (mhResult.Exits) return;
                }

                mhResult = CurrentConfigurations.getConfigurations();
                if (mhResult.Exits) return;

                sbShowTaskList = this.FindResource("sbShowTaskList") as Storyboard;
                sbHideTaskList = this.FindResource("sbHideTaskList") as Storyboard;

                sbHideTaskList.Completed += sbHideTaskList_Completed;

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

        void mvMinimalView_OnTaskCreated(DataBase.Adapters.TaskAdapter Task)
        {
            stlShowTaskList.CreatedTask(Task);
        }

        void recMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //quando ta grande
        void expand()
        {
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            blnColappsed = false;
            sbHideTaskList.Stop();
            sbShowTaskList.Begin();
            grdDetalhes.Visibility = System.Windows.Visibility.Visible;
        }

        void collapse()
        {
            blnColappsed = true;
            sbShowTaskList.Stop();
            sbHideTaskList.Begin();
            this.ResizeMode = System.Windows.ResizeMode.CanResize;

        }

        void sbHideTaskList_Completed(object sender, EventArgs e)
        {
            grdDetalhes.Visibility = System.Windows.Visibility.Collapsed;
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
