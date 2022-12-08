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
	//     <MyNamespace:AutoScrollListBox/>
	//
	// </summary>

	// 기본 동작을 true로 하고 생성자에서 이를 실행하면 이후 제어 루틴이 꼬여서 이상하게 동작함.
	public class AutoScrollListBox : ListBox
	{
		// 		static AutoScrollListBox()
		// 		{
		// 			DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoScrollListBox), new FrameworkPropertyMetadata(typeof(AutoScrollListBox)));
		// 		}

		///<summary>
		///Define the IsAutoScroll property. If enabled, causes the ListBox to scroll to 
		///the last item whenever a new item is added.
		///</summary>
		public static readonly DependencyProperty AutoScrollProperty =
			DependencyProperty.Register(
				"IsAutoScroll",
				typeof(Boolean),
				typeof(AutoScrollListBox),
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

		/// <summary>
		/// Event handler for when the IsAutoScroll property is changed.
		/// This delegates the call to SubscribeToAutoScrollItemsCollectionChanged().
		/// </summary>
		/// <param name="d">The DependencyObject whose property was changed.</param>
		/// <param name="e">Change event args.</param>
		private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = d as AutoScrollListBox;
			if (null != obj)
				SubscribeToAutoScrollItemsCollectionChanged(obj, (bool)e.NewValue);
		}

		/// <summary>
		/// Subscribes to the list items' collection changed event if IsAutoScroll is enabled.
		/// Otherwise, it unsubscribes from that event.
		/// For this to work, the underlying list must implement INotifyCollectionChanged.
		///
		/// (This function was only creative for brevity)
		/// </summary>
		/// <param name="obj">The list box containing the items collection.</param>
		/// <param name="subscribe">Subscribe to the collection changed event?</param>
		private static void SubscribeToAutoScrollItemsCollectionChanged(AutoScrollListBox obj, bool subscribe)
		{
			INotifyCollectionChanged notifyCollection = obj.Items.SourceCollection as INotifyCollectionChanged;
			if (notifyCollection != null)
			{
				//IsAutoScroll is turned on, subscribe to collection changed events.
				if (subscribe) { notifyCollection.CollectionChanged += obj.AutoScrollItemsCollectionChanged; }

				//IsAutoScroll is turned off, unsubscribe from collection changed events.
				else { notifyCollection.CollectionChanged -= obj.AutoScrollItemsCollectionChanged; }
			}
		}

		/// <summary>
		/// Event handler called only when the ItemCollection changes
		/// and if IsAutoScroll is enabled.
		/// </summary>
		/// <param name="sender">The ItemCollection.</param>
		/// <param name="e">Change event args.</param>
		private void AutoScrollItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				int count = Items.Count;
				if (0 < count)
					ScrollIntoView(Items[count - 1]);
			}
		}

		/// <summary>
		/// Constructor a new AutoScrollListBox.
		/// </summary>
		// 		public AutoScrollListBox()
		// 		{
		// 			//Subscribe to the IsAutoScroll property's items collection 
		// 			//changed handler by default if IsAutoScroll is enabled by default.
		// // 			if ((bool)AutoScrollProperty.DefaultMetadata.DefaultValue)
		// // 				SubscribeToAutoScrollItemsCollectionChanged(this, (bool)AutoScrollProperty.DefaultMetadata.DefaultValue);
		// 		}


	}
}
