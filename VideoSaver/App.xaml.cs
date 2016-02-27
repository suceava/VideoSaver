using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

				switch (e.Args[0])
				{
					case "/c":
						var configWindow = new ConfigWindow();
						configWindow.Show();
						return;

					case "/p": // preview
						// second argument needs to be a window handle
						if (e.Args.Length < 2)
						{
							MessageBox.Show("Preview window handle was not provided", "Video Saver", MessageBoxButton.OK, MessageBoxImage.Exclamation);
							this.Shutdown();
							return;
						}
						IntPtr previewWndHandle = new IntPtr(long.Parse(e.Args[1]));

						break;

					case "/s": // full screen
						var mainWindow = new MainWindow();
						mainWindow.Show();
						break;
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
