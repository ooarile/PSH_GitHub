using System.Windows.Media;

namespace ControlExtensions
{
	public class ThumbnailItem
	{
		public static int LoadedCount = 0;
		public static ThumbnailItem Create(ImageSource imageSource, string imageLabel, string imagePath, Brush labelBrush, double fontSize)
		{
			return new ThumbnailItem()
			{
				ImageSource = imageSource,
				ImageLabel = imageLabel,
				ImagePath = imagePath,
				LabelBrush = labelBrush,
				FontSize = fontSize
			};
		}

		public static ThumbnailItem Clone(ThumbnailItem item)
		{
			return new ThumbnailItem()
			{
				ImageSource = item.ImageSource,
				ImageLabel = item.ImageLabel,
				ImagePath = item.ImagePath,
				LabelBrush = item.LabelBrush,
				FontSize = item.FontSize,
				IsLoaded = item.IsLoaded
			};
		}

		private bool _isLoaded = false;

		public ImageSource ImageSource { get; set; }
		public string ImageLabel { get; set; }
		public string ImagePath { get; set; }
		public Brush LabelBrush { get; set; }
		public double FontSize { get; set; }
		public bool IsLoaded
		{
			get => _isLoaded;
			set { if (_isLoaded != value) { _isLoaded = value; LoadedCount += (value) ? 1 : 0; } }
		}
	}
}

