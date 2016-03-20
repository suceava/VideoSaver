using System;
using System.IO;
using System.Windows;

namespace VideoSaver
{
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			PrepareVideo();

			if (e.Args.Length > 0)
			{
				/*
					/c - Show the screensaver configuration dialog box
					/p - Show the screensaver in the screensaver selection dialog box
					/s - Show the screensaver full-screen
				 */

				var firstArg = e.Args[0].Trim();
				var secondArg = e.Args.Length > 1 ? e.Args[1] : null;

				// Handle cases where arguments are separated by colon. 
				// Examples: /c:1234567 or /p:1234567
				if (firstArg.Length > 2)
				{
					secondArg = firstArg.Substring(3);
					firstArg = firstArg.Substring(0, 2);
				}

				switch (firstArg)
				{
					case "/c":
						var configWindow = new ConfigWindow();
						configWindow.Show();
						return;

					case "/p": // preview
					   // second argument needs to be a window handle
						if (secondArg == null)
						{
							MessageBox.Show("Preview window handle was not provided", "Video Saver", MessageBoxButton.OK, MessageBoxImage.Exclamation);
							this.Shutdown();
							return;
						}
						IntPtr previewWndHandle = new IntPtr(long.Parse(secondArg));

						var previewWindow = new MainWindow(previewWndHandle);
						previewWindow.Show();
						break;

					case "/s": // full screen
						var mainWindow = new MainWindow();
						mainWindow.WindowState = WindowState.Maximized;
						mainWindow.Topmost = true;
						mainWindow.Show();
						break;

					default:
						this.Shutdown();
						return;
				}
			}
#if DEBUG
			else
			{
				var mainWindow = new MainWindow();
				mainWindow.Show();
			}
#endif
		}

		private void PrepareVideo()
		{
			var file = GetVideoFilePath();
			var path = Path.GetDirectoryName(file);
            if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (!File.Exists(file))
			{
				// save the video from resource to disk
				using (var fs = File.OpenWrite(file))
				{
					fs.Write(VideoSaver.Properties.Resources.video, 0, VideoSaver.Properties.Resources.video.Length);
				}
			}
		}

		internal static string GetVideoFilePath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VideoSaver", "SaverVideo.mp4");
		}
	}
}
