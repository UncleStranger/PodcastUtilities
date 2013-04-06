﻿using NUnit.Framework;
using PodcastUtilities.Common.Feeds;

namespace PodcastUtilities.Common.Tests.Feeds.PodcastFeedInRssFormatTests
{
    public class WhenConstructingAFeedFromExampleXml : WhenTestingTheFeed
    {
        protected override void When()
        {
            Feed = new PodcastFeedInRssFormat(FeedXmlStream,null);
        }

        [Test]
        public void ItShouldGetTheCorrectTitle()
        {
            Assert.That(Feed.Title, Is.EqualTo("This Developer's Life"));
        }
    }
}