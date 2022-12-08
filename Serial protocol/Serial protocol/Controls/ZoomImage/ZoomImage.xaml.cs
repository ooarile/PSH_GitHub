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

namespace ControlExtensions
{
	/// <summary>
	/// Interaction logic for ZoomImage.xaml
	/// </summary>
	public partial class ZoomImage : UserControl
	{
		#region "Properties"
		public ImageSource Source
		{
			get => (ImageSource)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}
		#endregion

		#region "DependencyProperties"
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ZoomImage),
			new FrameworkPropertyMetadata(OnImageSourceChanged/*이미지 소스?, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault*/));
		#endregion

		public ZoomImage()
		{
			InitializeComponent();

			// xaml 에 넣어도 됨. -> 백그라운드 컬러 넣어야 됨...
			this.Background = new SolidColorBrush(Colors.Transparent);
			// 			x.Background = new SolidColorBrush(Colors.White) { Opacity = 0.5 };
			this.PreviewMouseRightButtonDown += OnMouseRButtonDown_ResetScaleAndOffset;
			// 			this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(OnMouseRButtonDown_ResetScaleAndOffset);

			// 			this.Loaded += new RoutedEventHandler(MainView_Loaded);
		}
		// 
		// 		void MainView_Loaded(object sender, RoutedEventArgs e)
		// 		{
		// 			Window parentWindow = Window.GetWindow(this);
		// 			grid1.Width = parentWindow.Width;
		// 			grid1.Height = parentWindow.Height;
		// 		}

		#region "EventHandlers"
		private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = d as ZoomImage;
			if (null != obj)
			{
				if (object.Equals(obj.image1.Source, e.NewValue))
					return;

				var imageSource = (ImageSource)e.NewValue;
				if (null != imageSource)
				{
					// 크기를 1:1로 맞추지 않으면 자동으로 보간하면서 픽셀 정보가 일그러진다.
					// 작은 이미지 넣으면 이상하게 된다..ㅡㅡ;
					// 	obj.border1.Width = imageSource.Width;
					// 	obj.border1.Height = imageSource.Height;

					if (obj.grid1.ActualWidth >= imageSource.Width)
						obj.border1.Width = obj.grid1.ActualWidth;
					else
						obj.border1.Width = imageSource.Width;

					if (obj.grid1.ActualHeight >= imageSource.Height)
						obj.border1.Height = obj.grid1.ActualHeight;
					else
						obj.border1.Height = imageSource.Height;

					obj.ResetScaleAndOffset();
					obj.image1.Source = imageSource;
				}
			}
		}

		private void OnMouseRButtonDown_ResetScaleAndOffset(object sender, MouseButtonEventArgs e)
		{
			ResetScaleAndOffset();
		}

		private void ResetScaleAndOffset()
		{
			double scaleX = grid1.ActualWidth / border1.Width;
			double scaleY = grid1.ActualHeight / border1.Height;
			if (scaleX <= scaleY)
				border1.Reset(scaleX, 0, 0.5 * (grid1.ActualHeight - scaleX * border1.Height), 300 / border1.Width);
			else
				border1.Reset(scaleY, 0.5 * (grid1.ActualWidth - scaleY * border1.Width), 0, 300 / border1.Width);
		}

		private static Window FindParentWindow(DependencyObject child)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(child);

			//CHeck if this is the end of the tree
			if (parent == null) return null;

			Window parentWindow = parent as Window;
			if (parentWindow != null)
			{
				return parentWindow;
			}
			else
			{
				//use recursion until it reaches a Window
				return FindParentWindow(parent);
			}
		}
		#endregion
	}
}