using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoSaver
{
	public partial class MainWindow : Window
	{
		public const uint WS_CHILD = 0x40000000;
		public const uint WS_VISIBLE = 0x10000000;
		public const uint WS_CLIPCHILDREN = 0x02000000;

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left, top, right, bottom;
		}

		[DllImport("user32.dll")]
		static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		private bool _previewMode = false;
		private HwndSource _winWPFContent;

		public MainWindow()
		{
			InitializeComponent();

			mediaElement.Source = new Uri(App.GetVideoFilePath());
			mediaElement.Stretch = Stretch.Fill;
		}

		public MainWindow(IntPtr previewWndHandle) : this()
		{
			Top = Left = -9999;

			try
			{
				RECT lpRect;
				GetClientRect(previewWndHandle, out lpRect);

				//dockPanel.Width = 228; // lpRect.right - lpRect.left;
				//dockPanel.Height = 168; // lpRect.bottom - lpRect.top;

				HwndSourceParameters sourceParams = new HwndSourceParameters("sourceParams")
				{
					PositionX = 0,
					PositionY = 0,
					Height = lpRect.bottom - lpRect.top,
					Width = lpRect.right - lpRect.left,
					ParentWindow = previewWndHandle,
					WindowStyle = (int)(WS_VISIBLE | WS_CHILD | WS_CLIPCHILDREN)
				};

				_winWPFContent = new HwndSource(sourceParams);
				_winWPFContent.Disposed += new EventHandler(_winWPFContent_Disposed);
				_winWPFContent.RootVisual = dockPanel;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			_previewMode = true;
		}

		void _winWPFContent_Disposed(object sender, EventArgs e)
		{
			this.Close();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!_previewMode)
				this.Close();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!_previewMode)
				this.Close();
		}

		private Point mouseLocation = new Point(double.MinValue, double.MinValue);
        private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (_previewMode)
				return;

			var position = e.GetPosition(null);
			if (mouseLocation.X != double.MinValue)
			{
				// Terminate if mouse is moved a significant distance
				if (Math.Abs(mouseLocation.X - position.X) > 5 ||
					Math.Abs(mouseLocation.Y - position.Y) > 5)
					this.Close();
			}

			// Update current mouse location
			mouseLocation = position;
		}

		private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			mediaElement.Position = TimeSpan.FromSeconds(0);
		}
	}
}
