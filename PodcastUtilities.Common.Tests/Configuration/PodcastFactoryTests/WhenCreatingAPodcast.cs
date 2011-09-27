using NUnit.Framework;
using PodcastUtilities.Common.Configuration;
using Rhino.Mocks;

namespace PodcastUtilities.Common.Tests.Configuration.PodcastFactoryTests
{
    public class WhenCreatingAPodcast
        : WhenTestingBehaviour
    {
        protected IReadOnlyControlFile _controlFile;
        protected IPodcastDefaultsProvider DefaultsProvider { get; set; }

        protected PodcastFactory PodcastFactory { get; set; }

        protected IPodcastInfo CreatedPodcast { get; set; }

        protected override void GivenThat()
        {
            base.GivenThat();

            _controlFile = TestControlFileFactory.CreateControlFile();

            DefaultsProvider = GenerateMock<IPodcastDefaultsProvider>();

            DefaultsProvider.Stub(p => p.Pattern).Return("*.blah");
            DefaultsProvider.Stub(p => p.SortField).Return(PodcastFileSortField.FileName);
            DefaultsProvider.Stub(p => p.AscendingSort).Return(true);
            DefaultsProvider.Stub(p => p.FeedFormat).Return(PodcastFeedFormat.ATOM);
            DefaultsProvider.Stub(p => p.EpisodeNamingStyle).Return(PodcastEpisodeNamingStyle.UrlFileNameAndPublishDateTime);
            DefaultsProvider.Stub(p => p.EpisodeDownloadStrategy).Return(PodcastEpisodeDownloadStrategy.HighTide);
            DefaultsProvider.Stub(p => p.MaximumDaysOld).Return(33);

            PodcastFactory = new PodcastFactory(DefaultsProvider);
        }

        protected override void When()
        {
            CreatedPodcast = PodcastFactory.CreatePodcast(_controlFile);
        }

        [Test]
        public void ItShouldCreateAPodcastObject()
        {
            Assert.That(CreatedPodcast, Is.Not.Null);
        }

        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsPattern()
        {
            Assert.That(CreatedPodcast.Pattern, Is.EqualTo("*.blah"));
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsSortField()
        {
            Assert.That(CreatedPodcast.SortField.Value, Is.EqualTo(PodcastFileSortField.FileName));
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsAscendingSort()
        {
            Assert.That(CreatedPodcast.AscendingSort.Value, Is.True);
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsFeedFormat()
        {
            Assert.That(CreatedPodcast.Feed.Format.Value, Is.EqualTo(PodcastFeedFormat.ATOM));
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsFeedNamingStyle()
        {
            Assert.That(CreatedPodcast.Feed.NamingStyle.Value, Is.EqualTo(PodcastEpisodeNamingStyle.UrlFileNameAndPublishDateTime));
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsFeedDownloadStrategy()
        {
            Assert.That(CreatedPodcast.Feed.DownloadStrategy.Value, Is.EqualTo(PodcastEpisodeDownloadStrategy.HighTide));
        }
        [Test]
        public void ItShouldInitializeThePodcastUsingDefaultsFeedMaximumDaysOld()
        {
            Assert.That(CreatedPodcast.Feed.MaximumDaysOld.Value, Is.EqualTo(33));
        }
    }
}