using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ControlExtensions
{
    public class ZoomBorder : Border
    {
        private UIElement child = null;
        private Point origin;
        private Point start;
		private double ScaleLimitMin = 1.0;

		private TranslateTransform GetTranslateTransform(UIElement element)
        { return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform); }

        private ScaleTransform GetScaleTransform(UIElement element)
        { return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform); }

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                    this.Initialize(value);
                base.Child = value;
            }
        }
        public void Initialize(UIElement element)
        {
            this.child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();
				{
					var st = new ScaleTransform();
					var tt = new TranslateTransform();
					group.Children.Add(st);
					group.Children.Add(tt);
				}
				child.RenderTransform = group;
                child.RenderTransformOrigin = new Point(0.0, 0.0);

                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseLeftButtonUp;
                this.MouseMove += child_MouseMove;
//                 this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
            }
        }

        public void Reset(double scale, double sx, double sy, double scaleMin)
        {
            if (child != null)
            {
				if (double.IsNaN(scale))
					scale = 1;
				
                // reset zoom
                var st = GetScaleTransform(child);
                st.ScaleX = scale;
                st.ScaleY = scale;

				ScaleLimitMin = scaleMin;

                // reset pan
                var tt = GetTranslateTransform(child);
                tt.X = sx;
                tt.Y = sy;
            }
        }

        #region Child Events

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child != null)
            {
                var st = GetScaleTransform(child);
                var tt = GetTranslateTransform(child);

                double zoom = e.Delta > 0 ? 2.0 : 0.5;                               //                 double zoom = e.Delta > 0 ? .5 : -.5;
                if (!(e.Delta > 0) && (st.ScaleX <= ScaleLimitMin || st.ScaleY <= ScaleLimitMin))     // if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                    return;

                Point relative = e.GetPosition(child);
                double absoluteX;
                double absoluteY;

                absoluteX = relative.X * st.ScaleX + tt.X;
                absoluteY = relative.Y * st.ScaleY + tt.Y;

                //                 st.ScaleX += zoom;
                //                 st.ScaleY += zoom;
                st.ScaleX = System.Math.Max(ScaleLimitMin, st.ScaleX * zoom);
                st.ScaleY = System.Math.Max(ScaleLimitMin, st.ScaleY * zoom);

				tt.X = absoluteX - relative.X * st.ScaleX;
                tt.Y = absoluteY - relative.Y * st.ScaleY;
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                var tt = GetTranslateTransform(child);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.Hand;
                child.CaptureMouse();
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
// 			this.Reset(0.5, );
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child != null)
            {
                if (child.IsMouseCaptured)
                {
                    var tt = GetTranslateTransform(child);
                    Vector v = start - e.GetPosition(this);
                    tt.X = origin.X - v.X;
                    tt.Y = origin.Y - v.Y;
                    //                     if (0 <= tt.X) tt.X = 0;
                    //                     if (0 <= tt.Y) tt.Y = 0;
                }
            }
        }

        #endregion
    }
}
