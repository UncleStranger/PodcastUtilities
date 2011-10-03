﻿using System;
using NUnit.Framework;
using PodcastUtilities.Common.Feeds;

namespace PodcastUtilities.Common.Tests.Feeds.SyncItemToEpisodeDownloaderTaskConverterTests
{
    public class WhenTestingTheConverterConvertsItems : WhenTestingTheConverter
    {
        protected override void SetupData()
        {
            base.SetupData();
            _downloadItems.Add(new SyncItem()
                                   {
                                       DestinationPath = "destination1",
                                       EpisodeTitle = "item1",
                                       EpisodeUrl = new Uri("http://test1")
                                   });
            _downloadItems.Add(new SyncItem()
                                   {
                                       DestinationPath = "destination2",
                                       EpisodeTitle = "item2",
                                       EpisodeUrl = new Uri("http://test2")
                                   });
        }

        protected override void When()
        {
            _tasks = _converter.ConvertItemsToTasks(_downloadItems, null, null);
        }

        [Test]
        public void ItShouldReturnTheCorrectNumberOfTasks()
        {
            Assert.That(_tasks.Length, Is.EqualTo(2));
        }

        [Test]
        public void ItShouldReturnTheCorrectTypes()
        {
            Assert.IsInstanceOf(typeof(ITask), _tasks[0]);
            Assert.IsInstanceOf(typeof(ITask), _tasks[1]);
        }

        [Test]
        public void ItShouldReturnTasks0()
        {
            Assert.That(_tasks[0].SyncItem.DestinationPath, Is.EqualTo("destination1"));
            Assert.That(_tasks[0].SyncItem.EpisodeTitle, Is.EqualTo("item1"));
            Assert.That(_tasks[0].SyncItem.EpisodeUrl.ToString(), Is.EqualTo("http://test1/"));
        }

        [Test]
        public void ItShouldReturnTasks1()
        {
            Assert.That(_tasks[1].SyncItem.DestinationPath, Is.EqualTo("destination2"));
            Assert.That(_tasks[1].SyncItem.EpisodeTitle, Is.EqualTo("item2"));
            Assert.That(_tasks[1].SyncItem.EpisodeUrl.ToString(), Is.EqualTo("http://test2/"));
        }
    }
}