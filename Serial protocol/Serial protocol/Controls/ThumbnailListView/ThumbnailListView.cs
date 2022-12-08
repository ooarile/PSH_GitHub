using System.Windows;
using System.Windows.Controls;

namespace ControlExtensions
{
    public class ThumbnailListView : ViewBase
    {
        protected override object DefaultStyleKey { get { return new ComponentResourceKey(GetType(), "ThumbnailListViewKey"); } }
        protected override object ItemContainerDefaultStyleKey { get { return new ComponentResourceKey(GetType(), "ThumbnailListViewItemKey"); } }
    }
}

