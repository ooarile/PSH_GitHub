using Microsoft.Toolkit.Mvvm.Input;


using Serial_protocol.View;
using Serial_protocol.ViewModel.Base;

using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Serial_protocol.DataAccess;
using Serial_protocol.SettingData;
using Serial_protocol.Protocol;
using System.Collections.ObjectModel;
using Serial_protocol.Data;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

namespace Serial_protocol.ViewModel
{
    internal class MainWindowViewModel : CloseCommand
    {
        #region Fields
        SerialProtocol serialprotocol;

        //Setup 버튼 클릭 이벤트
        public ICommand openSerialSettingForm { get; } = null;
        //시리얼 통신 SendCommand 버튼 클릭 이벤트
        public ICommand sendCommand { get; } = null;
        public ICommand SerialDisconnect { get; } = null; 
        public ICommand Serialconnect { get; } = null;


        //시리얼 통신 Rec Data를 xaml과 연동시켜주는 개체
        public ObservableCollection<DataStruct> Data { get; private set; }// = new ObservableCollection<string>();
        private DataStruct recContent;
        private string _CONTENT;
        private DateTime _DateTime;
        private string _sendContent;

        private bool _isAutoScroll0 = true;

        //Serilog
        protected Serilog.ILogger _logger = Serial_protocol.App.Logger;
        //hread Test_thread;
        bool test = true;


        public string CONTENT
        {
            get { return _CONTENT; }
            set { _CONTENT = value; }
        }
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }
        public bool IsAutoScroll
        {
            get { return _isAutoScroll0; }
            set { if (value != _isAutoScroll0) { _isAutoScroll0 = value; OnPropertyChanged(); } }
        }
        public string SendContent
        {
            get { return _sendContent; }
            set { _sendContent = value; }
        }
        #endregion // Fields

        public MainWindowViewModel()
        {
            Data = new ObservableCollection<DataStruct>();
            openSerialSettingForm = new RelayCommand(() => this.OpenSerialSettingForm());
            sendCommand = new RelayCommand(() => this.SendCommand());
            SerialDisconnect = new RelayCommand(() => this.SerialDisconnectFunc());
            Serialconnect = new RelayCommand(() => this.SerialconnectFunc());

            //Comm Open
            SettingsComm.Read();

            //Test_thread = new Thread(Test_Func);
            //Test_thread.IsBackground = true;
            //Test_thread.Start();

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Thread.Sleep(1);
            //Console.WriteLine(sw.ElapsedMilliseconds);
            //sw.Stop();
        }

        #region Fuction

        private void Test_Func()
        {
            int i = 0;
            while(test)
            {
                _logger.Debug(i++.ToString());
            }
        }
        public void OpenSerialSettingForm()
        {
            try
            {
                var serialSettingForm = new SerialSettingForm();

                var serialSettingFormViewModel = new SerialSettingFormViewModel(/*path*/);

                EventHandler handler = null;
                handler = delegate
                {
                    serialSettingFormViewModel.RequestClose -= handler;
                    serialSettingForm.Close();
                };
                serialSettingFormViewModel.RequestClose += handler;

                serialSettingForm.DataContext = serialSettingFormViewModel;
                serialSettingForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                serialSettingForm.ShowDialog();
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        public void SerialconnectFunc()
        {
            try
            {
                if (serialprotocol == null )
                {
                    serialprotocol = new SerialProtocol();
                    serialprotocol.StatusChanged += OnStatusChanged;
                    serialprotocol.DataReceived += OnDataReceived;
                    serialprotocol.Open();
                    
                }else
                    serialprotocol.Open();
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        public void SerialDisconnectFunc()
        {
            try
            {
                if (serialprotocol != null)
                {
                    serialprotocol.Close();
                }
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        public void SendCommand()
        {
            try
            {
                if (SendContent.Length > 0)
                {
                    SendContent = serialprotocol.ConvertEscapeSequences(SendContent);
                    serialprotocol.Send(SendContent);
                }
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        #endregion

        #region Deligate
        public void OnStatusChanged(string status)
        {
            //textBox1.Text = status;
        }
        public void OnDataReceived(string dataIn)
        {
            try
            {
                recContent = new DataStruct();
                recContent.Time = DateTime.Now;
                recContent.Content = dataIn;
                DispatcherService.Invoke((System.Action)(() =>
                {
                    Data.Add(recContent);
                    // https://afsdzvcx123.tistory.com/entry/WPF-WPF-DataGrid-%EC%BB%A8%ED%8A%B8%EB%A1%A4-MVVM-%ED%8C%A8%ED%84%B4-%EB%8D%B0%EC%9D%B4%ED%84%B0-%EB%B0%94%EC%9D%B8%EB%94%A9%ED%95%98%EA%B8%B0
                }));
                //WPF 쓰레딩 이슈
                //https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=seokcrew&logNo=221309203938

                Console.WriteLine(dataIn);
            }catch(Exception ex)
            {

            }         
        }
        public void FormClosing()
        {
            try
            {
                if(serialprotocol!=null)                
                    serialprotocol.Close();

                test = false;
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        #endregion
    }
}
