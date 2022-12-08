using Serial_protocol.ViewModel;
using System;
using System.Windows;
using System.Runtime.InteropServices;

namespace Serial_protocol
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class App : Application
    {
        // 아래 Win32 함수 가져온뒤
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint timeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint timeEndPeriod(uint uMilliseconds);

        public static Serilog.ILogger Logger { get; private set; } = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            timeBeginPeriod(1);

            #region Serilog
            //// 디버깅 용으로 처리함...
            //bool log_sync = false;
            //bool log_console = false;
            //bool write_raw_packet = false;
            //foreach (var arg in e.Args)
            //{
            //    log_sync |= arg.Contains("-LogSync");
            //    //log_console |= arg.Contains("-LogConsole"); 사용하지 않는 것이 좋을듯...
            //    write_raw_packet |= arg.Contains("-WriteRawPacket");
            //}

            //if (log_console)
            //    Helper.Win32.console.ShowConsole(System.Text.Encoding.Default);

            Logger = Logger_v2.Initialize(/*log_sync*/true, /*log_console*/true, "..\\logs\\seq-.log");
            #endregion


            Logger.Information("=====================================================");
            Logger.Information("App Start");
            Logger.Information("Initialization Start");




            #region View 실행
            MainWindow window = new MainWindow();

            // Create the ViewModel to which 
            // the main window binds.
            //string path = "Data/customers.xml";
            var viewModel = new MainWindowViewModel(/*path*/);

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                viewModel.FormClosing();    // Main폼 닫을때 처리할것들
                window.Close();
            };
            viewModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;

            window.Show();
            #endregion


            Logger.Information("Pass MainWindow Create/Open");
            Logger.Information("Initialization Finished");
            Logger.Information("=====================================================");
        }
    }
}
