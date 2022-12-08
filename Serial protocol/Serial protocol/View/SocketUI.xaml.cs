using Serial_protocol.ViewModel;
using System.Windows.Controls;

namespace Serial_protocol.View
{
    /// <summary>
    /// SocketUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SocketUI : UserControl
    {
        public SocketUI()
        {
            InitializeComponent();


            //    var socketViewModel = new SocketViewModel(/*path*/);

            //    //EventHandler handler = null;
            //    //handler = delegate
            //    //{
            //    //    serialSettingFormViewModel.RequestClose -= handler;
            //    //    serialSettingForm.Close();
            //    //};
            //    //serialSettingFormViewModel.RequestClose += handler;

            //    this.DataContext = socketViewModel;
        }
    }
}
