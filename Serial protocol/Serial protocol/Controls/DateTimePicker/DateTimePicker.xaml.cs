using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ControlExtensions
{
	/// Changes and improvements made =>
	/// 1. Removed AM/PM chooser and made time selectable from 6:00 to 18:00
	/// 2. Popup improvements
	/// 3. Added SelectedDate DependencyProperty
	///
	/// Things that will be needed for this control to work properly (and look good :) ) =>
	/// 1. A bitmap image 32x32 added as an embedded resource
	///
	/// Licensing =>
	/// The Code Project Open License (CPOL)
	/// http://www.codeproject.com/info/cpol10.aspx

	public partial class DateTimePicker : UserControl
	{
		private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		#region "Properties -> DependencyProperties"
		public DateTime SelectedDate
		{
			get => (DateTime)GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value);
		}
		public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate",
			typeof(DateTime), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));
		#endregion

		public DateTimePicker()
		{
			InitializeComponent();

			// 순서가 중요합니다...
			
			// 첫 값을 컨트롤에 넣어줍니다. - 별로 의미가 없을것 같긴 합니다만.. 
			var dateTime = SelectedDate.AddMilliseconds(-SelectedDate.Millisecond);
			{
				Calendar.SelectedDate = dateTime;

				Hours.SelectedItem = Hours.Items[dateTime.Hour];
				Minutes.SelectedItem = Minutes.Items[dateTime.Minute];
				Seconds.SelectedItem = Seconds.Items[dateTime.Second];

				DateDisplay.Text = dateTime.ToString(DateTimeFormat);
			}

			Calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
			Hours.SelectionChanged += Time_SelectionChanged;
			Minutes.SelectionChanged += Time_SelectionChanged;
			Seconds.SelectionChanged += Time_SelectionChanged;

			// 			BitmapSource ConvertGDI_To_WPF(Bitmap bm)
			// 			{
			// 				BitmapSource bms = null;
			// 				IntPtr h_bm = IntPtr.Zero;
			// 				h_bm = bm.GetHbitmap();
			// 				bms = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(h_bm, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			// 				bms.Freeze();
			// 				h_bm = IntPtr.Zero;
			// 				return bms;
			// 			}
			// 			Bitmap bitmap1 = Properties.Resources.DateTimePicker;
			// 			bitmap1.MakeTransparent(System.Drawing.Color.Black);
			// 			IconImage.Source = null;// ConvertGDI_To_WPF(bitmap1);
		}

		#region "EventHandlers"
		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = d as DateTimePicker;
			if (null != obj)
			{
				// millisecond -> 000 으로 정리
				var dateTime = ((DateTime)e.NewValue).AddMilliseconds(-((DateTime)e.NewValue).Millisecond);
				if (obj.SelectedDate != dateTime)
					obj.SelectedDate = dateTime;
				obj.UpdateControlSelection(dateTime);
			}
		}

		private void UpdateControlSelection(DateTime dateTime)
		{
			// 이벤트 핸들러 처리하지 않으면 무한 재귀호출로 스텍 오버플로 발생~!!!
			if (Calendar.SelectedDate?.Date != dateTime.Date)
			{
				Calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;
				Calendar.SelectedDate = dateTime.Date;
				Calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
			}
			if (Hours.SelectedItem != Hours.Items[dateTime.Hour])
			{
				Hours.SelectionChanged -= Time_SelectionChanged;
				Hours.SelectedItem = Hours.Items[dateTime.Hour];
				Hours.SelectionChanged += Time_SelectionChanged;
			}
			if (Minutes.SelectedItem != Minutes.Items[dateTime.Minute])
			{
				Minutes.SelectionChanged -= Time_SelectionChanged;
				Minutes.SelectedItem = Minutes.Items[dateTime.Minute];
				Minutes.SelectionChanged += Time_SelectionChanged;
			}
			if (Seconds.SelectedItem != Seconds.Items[dateTime.Second])
			{
				Seconds.SelectionChanged -= Time_SelectionChanged;
				Seconds.SelectedItem = Seconds.Items[dateTime.Second];
				Seconds.SelectionChanged += Time_SelectionChanged;
			}

			if (DateDisplay.Text != dateTime.ToString(DateTimeFormat))
				DateDisplay.Text = dateTime.ToString(DateTimeFormat);
		}

		private void Calendar_SelectedDatesChanged(object sender, EventArgs e)
		{
			// 새로운값 계산
			var hours = (Hours?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "12";
			var minutes = (Minutes?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "00";
			var seconds = (Seconds?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "00";
			var timeSpan = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
			var dates = (sender as Calendar)?.SelectedDate?.Date ?? SelectedDate.Date;
		
			var dateTimeNew = dates + timeSpan;
			if (SelectedDate != dateTimeNew)
				SelectedDate = dateTimeNew;
		}

		private void SaveTime_Click(object sender, RoutedEventArgs e)
		{
			///Calendar_SelectedDatesChanged(sender, e);
			PopUpCalendarButton.IsChecked = false;
		}

		private void Time_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Calendar_SelectedDatesChanged(sender, e);
		}

		private void Calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{   // that it's not necessary to click twice after opening the calendar  https://stackoverflow.com/q/6024372
			if (Mouse.Captured is CalendarItem)
			{
				Mouse.Capture(null);
			}
		}

		#endregion
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class InvertBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool)value;
		}
	}
}