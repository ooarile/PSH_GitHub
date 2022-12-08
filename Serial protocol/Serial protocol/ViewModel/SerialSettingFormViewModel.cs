using Microsoft.Toolkit.Mvvm.Input;
using Serial_protocol.SettingData;
using Serial_protocol.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Serial_protocol.ViewModel
{
    internal class SerialSettingFormViewModel : CloseCommand
    {
        public ICommand saveCommand { get; } = null;

        // 설정한 Config 불러오기
        private string _ComPort;
        private int _BuadRate;
        private int _DataBits;
        private System.IO.Ports.Parity _Parity;
        private System.IO.Ports.StopBits _StopBits;
        private System.IO.Ports.Handshake _Handshake;

        private bool _AppendNothing =false;
        private bool _AppendCR = false;
        private bool _AppendLR = false;
        private bool _AppendCRLR = false;

        private bool _HexOutput = false;
        private bool _MonoFont = false;
        private bool _LocalEcho = false;
        private bool _StayOnTop = false;
        private bool _FilterUseCase = false;


        public string ComPort
        {
            get { return _ComPort; }
            set { _ComPort = value; }
        }
        public int BuadRate
        {
            get { return _BuadRate; }
            set { _BuadRate = value; }
        }
        public int DataBits
        {
            get { return _DataBits; }
            set { _DataBits = value; }
        }
        public System.IO.Ports.Parity Parity
        {
            get { return _Parity; }
            set { _Parity = value; }
        }
        public System.IO.Ports.StopBits StopBits
        {
            get { return _StopBits; }
            set { _StopBits = value; }
        }
         public System.IO.Ports.Handshake Handshake
        {
            get { return _Handshake; }
            set { _Handshake = value; }
        }

        public bool AppendNothing
        {
            get { return _AppendNothing; }
            set { _AppendNothing = value; }
        }
        public bool AppendCR
        {
            get { return _AppendCR; }
            set { _AppendCR = value; }
        }
        public bool AppendLR
        {
            get { return _AppendLR; }
            set { _AppendLR = value; }
        }
        public bool AppendCRLR
        {
            get { return _AppendCRLR; }
            set { _AppendCRLR = value; }
        }
        public bool HexOutput
        {
            get { return _HexOutput; }
            set { _HexOutput = value; }
        }
        public bool MonoFont
        {
            get { return _MonoFont; }
            set { _MonoFont = value; }
        }
        public bool LocalEcho
        {
            get { return _LocalEcho; }
            set { _LocalEcho = value; }
        }
        public bool StayOnTop
        {
            get { return _StayOnTop; }
            set { _StayOnTop = value; }
        }
        public bool FilterUseCase
        {
            get { return _FilterUseCase; }
            set { _FilterUseCase = value; }
        }


        // PC에 불러올수 있는 Comport List
        public ObservableCollection<string> ComPortList { get; private set; } 
        // BoutRate List
        public ObservableCollection<int> BoutRateList { get; private set; } 
        public ObservableCollection<int> DataBitsList { get; private set; }
        public ObservableCollection<string> ParityList { get; private set; }
        public ObservableCollection<string> StopBitsList { get; private set; }
        public ObservableCollection<string> HandshakeList { get; private set; }
        public SerialSettingFormViewModel()
        {
            saveCommand = new RelayCommand(() => this.SaveSerialSetting());
                          
            ComPort = SettingsComm.Port.PortName; 
            ComPortList = new ObservableCollection<string>(System.IO.Ports.SerialPort.GetPortNames());

            BuadRate = SettingsComm.Port.BaudRate;
            BoutRateList = new ObservableCollection<int> 
            { 
                 300, 600, 1200, 2400, 4800,9600,14400, 19200,28800, 38400, 57600,  115200
            };

            DataBits = SettingsComm.Port.DataBits;
            DataBitsList = new ObservableCollection<int>
            {
                 5,6,7,8
            };

            Parity = SettingsComm.Port.Parity;
            ParityList = new ObservableCollection<string>(Enum.GetNames(typeof(System.IO.Ports.Parity)));

            StopBits = SettingsComm.Port.StopBits;
            StopBitsList = new ObservableCollection<string>(Enum.GetNames(typeof(System.IO.Ports.StopBits)));

            Handshake = SettingsComm.Port.Handshake;
            HandshakeList = new ObservableCollection<string>(Enum.GetNames(typeof(System.IO.Ports.Handshake)));

            switch (SettingsComm.Option.AppendToSend)
            {
                case SettingsComm.Option.AppendType.AppendNothing:
                    AppendNothing = true; break;
                case SettingsComm.Option.AppendType.AppendCR:
                    AppendCR = true; break;
                case SettingsComm.Option.AppendType.AppendLF:
                    AppendLR = true; break;
                case SettingsComm.Option.AppendType.AppendCRLF:
                    AppendCRLR = true; break;
            }

            HexOutput = SettingsComm.Option.HexOutput;
            MonoFont = SettingsComm.Option.MonoFont;
            LocalEcho = SettingsComm.Option.LocalEcho;
            StayOnTop = SettingsComm.Option.StayOnTop;
            FilterUseCase = SettingsComm.Option.FilterUseCase;


        }
        public void SaveSerialSetting()
        {
            try
            {
                SettingsComm.Port.PortName = ComPort;
                SettingsComm.Port.BaudRate = BuadRate;
                SettingsComm.Port.DataBits = DataBits;
                SettingsComm.Port.Parity = Parity;
                SettingsComm.Port.StopBits = StopBits;
                SettingsComm.Port.Handshake = Handshake;

                if (AppendNothing)
                    SettingsComm.Option.AppendToSend = SettingsComm.Option.AppendType.AppendNothing;
                else if(AppendCR)
                    SettingsComm.Option.AppendToSend = SettingsComm.Option.AppendType.AppendCR;
                else if (AppendLR)
                    SettingsComm.Option.AppendToSend = SettingsComm.Option.AppendType.AppendLF;
                else 
                    SettingsComm.Option.AppendToSend = SettingsComm.Option.AppendType.AppendCRLF;

                SettingsComm.Option.HexOutput = HexOutput;
                SettingsComm.Option.MonoFont = MonoFont;
                SettingsComm.Option.LocalEcho = LocalEcho;
                SettingsComm.Option.StayOnTop = StayOnTop;
                SettingsComm.Option.FilterUseCase = FilterUseCase;

                //Comm Write
                SettingsComm.Write();

                OnClose();
            }
            catch (System.Exception /*ex*/)
            {

            }
        }

    }
}
