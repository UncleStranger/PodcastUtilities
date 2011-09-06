﻿using System.Collections.Generic;
using NUnit.Framework;
using PodcastUtilities.Common.Feeds;

namespace PodcastUtilities.Common.Tests.Feeds.PodcastFeedInRssFormatTests
{
    public class WhenLoadingTheEpisodesFromExampleXml : WhenTestingTheFeed
    {
        private IList<IPodcastFeedItem> _episodes;
        private bool _statusUpdate;
        private bool _statusError;
        private bool _statusWarning;

        protected override void GivenThat()
        {
            base.GivenThat();
            Feed = new PodcastFeedInRssFormat(FeedXmlStream);
            Feed.StatusUpdate += new System.EventHandler<StatusUpdateEventArgs>(Feed_StatusUpdate);
            _statusError = false;
            _statusWarning = false;
            _statusUpdate = false;
        }

        void Feed_StatusUpdate(object sender, StatusUpdateEventArgs e)
        {
            switch (e.MessageLevel)
            {
                case StatusUpdateLevel.Warning:
                    _statusWarning = true;
                    break;
                case StatusUpdateLevel.Error:
                    _statusError = true;
                    break;
                case StatusUpdateLevel.Verbose:
                    _statusUpdate = true;
                    break;
            }
        }

        protected override void When()
        {
            _episodes = Feed.Episodes;
        }

        [Test]
        public void ItShouldLoadTheEpisodes()
        {
            Assert.That(_episodes.Count, Is.EqualTo(14));
        }

        [Test]
        public void ItShouldFireTheStatusEvent()
        {
            Assert.That(_statusError, Is.False);
            Assert.That(_statusWarning, Is.True);
            Assert.That(_statusUpdate, Is.True);
        }

        [Test]
        public void ItShouldExcludeIllegalFilenames()
        {
            Assert.That(_episodes[0].FileName, Is.EqualTo("15-_Revolt_.mp3"));
            Assert.That(_episodes[2].FileName, Is.EqualTo("___"));
        }

        [Test]
        public void ItShouldCopeWithSimpleUrls()
        {
            Assert.That(_episodes[1].FileName, Is.EqualTo("114_Obsession.mp3"));
        }

        [Test]
        public void ItShouldEliminateEmptyUrl()
        {
            Assert.That(_episodes[3].FileName, Is.EqualTo("11-Scars.mp3"));
        }

        [Test]
        public void ItShouldGetThePublishDate()
        {
            Assert.That(_episodes[0].Published.Year, Is.EqualTo(2011));
            Assert.That(_episodes[0].Published.Month, Is.EqualTo(3));
            Assert.That(_episodes[0].Published.Day, Is.EqualTo(22));
            Assert.That(_episodes[0].Published.Hour, Is.EqualTo(17));
            Assert.That(_episodes[0].Published.Minute, Is.EqualTo(17));
            Assert.That(_episodes[0].Published.Second, Is.EqualTo(29));
        }
    }
}