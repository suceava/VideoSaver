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

namespace VideoSaver
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			mediaElement.Source = new Uri(App.GetVideoFilePath());
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			this.Close();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Close();
		}

		private Point mouseLocation = new Point(double.MinValue, double.MinValue);
        private void Window_MouseMove(object sender, MouseEventArgs e)
		{
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
