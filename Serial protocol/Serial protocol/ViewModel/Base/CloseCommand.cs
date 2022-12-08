using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;

namespace Serial_protocol.ViewModel.Base
{
    internal class CloseCommand : ObservableObject
    {
        public ICommand closeCommand { get; } = null;
        protected CloseCommand()
        {
            closeCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => this.OnClose());
        }
        public event EventHandler RequestClose;
        public void OnClose()         // 		private void OnButtonExit(object sender, RoutedEventArgs e)
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);

        }
    }
}
