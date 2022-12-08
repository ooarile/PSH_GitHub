using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ControlExtensions
{
	// <summary>
	// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	//
	// Step 1a) Using this custom control in a XAML file that exists in the current project.
	// Add this XmlNamespace attribute to the root element of the markup file where it is 
	// to be used:
	//
	//     xmlns:MyNamespace="clr-namespace:ControlExtensions"
	//
	//
	// Step 1b) Using this custom control in a XAML file that exists in a different project.
	// Add this XmlNamespace attribute to the root element of the markup file where it is 
	// to be used:
	//
	//     xmlns:MyNamespace="clr-namespace:ControlExtensions;assembly=ControlExtensions"
	//
	// You will also need to add a project reference from the project where the XAML file lives
	// to this project and Rebuild to avoid compilation errors:
	//
	//     Right click on the target project in the Solution Explorer and
	//     "Add Reference"->"Projects"->[Browse to and select this project]
	//
	//
	// Step 2)
	// Go ahead and use your control in the XAML file.
	//
	//     <MyNamespace:AutoScrollViewer/>
	//
	// </summary>

	// 기본 동작을 true로 하고 생성자에서 이를 실행하면 이후 제어 루틴이 꼬여서 이상하게 동작함.
	public class AutoScrollViewer : ScrollViewer
	{
		// 		static AutoScrollViewer()
		// 		{
		// 			DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoScrollViewer), new FrameworkPropertyMetadata(typeof(AutoScrollViewer)));
		// 		}

		///<summary>
		///Define the IsAutoScroll property. If enabled, causes the ListBox to scroll to 
		///the last item whenever a new item is added.
		///</summary>
		public static readonly DependencyProperty AutoScrollProperty =
			DependencyProperty.RegisterAttached(
				"IsAutoScroll",
				typeof(bool),
				typeof(AutoScrollViewer),
				new FrameworkPropertyMetadata(
					false, //Default value. --> 기본값이 true 인 경우 외부에서 조정시 잘 동작하지 않는다...
					FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
					AutoScrollPropertyChanged));

		/// <summary>
		/// Gets or sets whether or not the list should scroll to the last item 
		/// when a new item is added.
		/// </summary>
		[Category("Common")] //Indicate where the property is located in VS designer.
		public bool IsAutoScroll
		{
			get { return (bool)GetValue(AutoScrollProperty); }
			set { SetValue(AutoScrollProperty, value); }
		}

		public static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			var obj = d as AutoScrollViewer;
			if (obj != null && (bool)args.NewValue)
			{
				obj.ScrollChanged += ScrollViewer_ScrollChanged;
				obj.ScrollToEnd();
			}
			else
			{
				obj.ScrollChanged -= ScrollViewer_ScrollChanged;
			}
		}

		private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			// Only scroll to bottom when the extent changed. Otherwise you can't scroll up
			var obj = sender as AutoScrollViewer;
			if (null != obj)
			{
				if (0 != e.ExtentHeightChange)
				{
					if (true == obj.IsAutoScroll)
					{
						// 						e.VerticalOffset + e.ViewportHeight e.ExtentHeight == 
						// 						if (true)
						// 							obj.IsAutoScroll = false;
						// 	var obj = sender as ScrollViewer;
						// 	obj?.ScrollToBottom();
						obj.ScrollToBottom();
					}
				}
			}
		}


	}
}
