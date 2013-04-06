using System.Collections.Generic;
using NUnit.Framework;
using PodcastUtilities.Common;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Presentation.ViewModels;
using Rhino.Mocks;

namespace PodcastUtilities.Presentation.Tests.ViewModels.ConfigurePodcastsViewModelTests
{
	public class WhenExecutingOpenFileCommand
		: WhenTestingConfigurePodcastsViewModel
	{
		public List<PodcastInfo> Podcasts { get; set; }

		protected override void GivenThat()
		{
			base.GivenThat();

			ViewModel.Podcasts.Add(new PodcastViewModel(null));

			BrowseForFileService.Stub(s => s.BrowseForFileToOpen("Control Files|*.xml"))
				.Return(@"C:\blah\test.xml");

			ControlFileFactory.Stub(f => f.OpenControlFile(@"C:\blah\test.xml"))
				.Return(ControlFile);

			Podcasts = new List<PodcastInfo>
			           	{
			           		new PodcastInfo(ControlFile),
							new PodcastInfo(ControlFile),
							new PodcastInfo(ControlFile)
			           	};
			ControlFile.Stub(f => f.GetPodcasts())
				.Return(Podcasts);
		}

		protected override void When()
		{
			ViewModel.OpenFileCommand.Execute(null);
		}

		[Test]
		public void ItShouldBrowseAndOpenTheSelectedFile()
		{
			ControlFileFactory.AssertWasCalled(f => f.OpenControlFile(@"C:\blah\test.xml"));
		}

		[Test]
		public void ItShouldUpdateThePodcastsFromNewFile()
		{
			Assert.That(ViewModel.Podcasts.Count, Is.EqualTo(3));
			Assert.That(ViewModel.Podcasts[0].Podcast, Is.EqualTo(Podcasts[0]));
			Assert.That(ViewModel.Podcasts[1].Podcast, Is.EqualTo(Podcasts[1]));
			Assert.That(ViewModel.Podcasts[2].Podcast, Is.EqualTo(Podcasts[2]));
		}
	}
}