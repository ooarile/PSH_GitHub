using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using practice.Log;
using practice._15_Winform;

namespace practice
{
    internal static class Program
    {       
        // 아래 Win32 함수 가져온뒤
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint timeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint timeEndPeriod(uint uMilliseconds);

        public static Serilog.ILogger Logger { get; private set; } = null;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            timeBeginPeriod(1);

            Logger = Logger_v2.Initialize(/*log_sync*/true, /*log_console*/true, "..\\logs\\seq-.log");
            Logger.Information("=====================================================");
            Logger.Information("App Start");
            Logger.Information("Initialization Start");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Application.Run(new TextBoxForm());

            Logger.Information("Pass MainWindow Create/Open");
            Logger.Information("Initialization Finished");
            Logger.Information("=====================================================");
        }
    }
}
