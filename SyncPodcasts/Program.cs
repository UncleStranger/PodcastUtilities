﻿using System;
using System.Reflection;
using PodcastUtilities.Common;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Files;
using PodcastUtilities.Common.Platform;
using PodcastUtilities.Common.Playlists;
using PodcastUtilities.Ioc;

namespace SyncPodcasts
{
	class Program
	{
		static private void DisplayBanner()
		{
			// do not move the GetExecutingAssembly call from here into a supporting DLL
			Assembly me = System.Reflection.Assembly.GetExecutingAssembly();
			AssemblyName name = me.GetName();
			Console.WriteLine("SyncPodcasts v{0}", name.Version);
		}

		static private void DisplayHelp()
		{
			Console.WriteLine("Usage: SyncPodcasts <controlfile>");
			Console.WriteLine("Where");
			Console.WriteLine("  <controlfile> = XML control file eg. podcasts.xml");
		}

		static void Main(string[] args)
		{
			DisplayBanner();
			if (args.Length < 1)
			{
				DisplayHelp();
				return;
			}

			LinFuIocContainer iocContainer = InitializeIocContainer();

			var control = new ReadOnlyControlFile(args[0]);
			var finder = iocContainer.Resolve<IFinder>();
			var copier = iocContainer.Resolve<ICopier>();
			var remover = iocContainer.Resolve<IUnwantedFileRemover>();
			var fileUtilities = iocContainer.Resolve<IFileUtilities>();
			var playlistFactory = iocContainer.Resolve<IPlaylistFactory>();

            var generator = new Generator(finder, fileUtilities, playlistFactory);
            generator.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(StatusUpdate);

			var synchronizer = new Synchronizer(finder, copier, remover);
			synchronizer.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(StatusUpdate);

			synchronizer.Synchronize(control, false);

			if (!string.IsNullOrEmpty(control.GetPlaylistFileName()))
				generator.GeneratePlaylist(control, true);
		}

		private static LinFuIocContainer InitializeIocContainer()
		{
			var container =  new LinFuIocContainer();

            IocRegistration.RegisterFileServices(container);
            IocRegistration.RegisterPlaylistServices(container);

			return container;
		}

		static void StatusUpdate(object sender, StatusUpdateEventArgs e)
        {
            // maybe we want to optionally filter verbose message
            Console.WriteLine(e.Message);
        }
	}
}