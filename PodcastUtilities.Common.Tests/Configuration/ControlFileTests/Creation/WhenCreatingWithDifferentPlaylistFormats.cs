﻿using System;
using System.Xml;
using NUnit.Framework;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Exceptions;
using PodcastUtilities.Common.Playlists;

namespace PodcastUtilities.Common.Tests.Configuration.ControlFileTests.Creation
{
    abstract class WhenCreatingAControlFileWithDifferentPlaylistFormats : WhenTestingAControlFile
    {
        protected string ControlFileFormatText { get; set; }
        protected PlaylistFormat Format { get; set; }
        protected Exception ThrownException { get; set; }

        protected override void GivenThat()
        {
            base.GivenThat();

            XmlNode n = ControlFileXmlDocument.SelectSingleNode("podcasts/global/playlistFormat");
            n.InnerText = ControlFileFormatText;

        }

        protected override void When()
        {
            ThrownException = null;
            try
            {
                ControlFile = new ReadOnlyControlFile(ControlFileXmlDocument);
                Format = ControlFile.GetPlaylistFormat();
            }
            catch (Exception exception)
            {
                ThrownException = exception;
            }
        }
    }

    class WhenCreatingAControlFileWithWplPlaylistFormat : WhenCreatingAControlFileWithDifferentPlaylistFormats
    {
        protected override void GivenThat()
        {
            ControlFileFormatText = "wpl";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadPlaylistFormat()
        {
            Assert.That(Format, Is.EqualTo(PlaylistFormat.WPL));
        }
    }

    class WhenCreatingAControlFileWithAsxPlaylistFormat : WhenCreatingAControlFileWithDifferentPlaylistFormats
    {
        protected override void GivenThat()
        {
            ControlFileFormatText = "ASX";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadPlaylistFormat()
        {
            Assert.That(Format, Is.EqualTo(PlaylistFormat.ASX));
        }
    }

    class WhenCreatingAControlFileWithAnUnknownPlaylistFormat : WhenCreatingAControlFileWithDifferentPlaylistFormats
    {
        protected override void GivenThat()
        {
            ControlFileFormatText = "UNKNOWN";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadPlaylistFormat()
        {
            Assert.That(Format, Is.EqualTo(PlaylistFormat.Unknown));
        }
    }

    class WhenCreatingAControlFileWithAnInvalidPlaylistFormat : WhenCreatingAControlFileWithDifferentPlaylistFormats
    {
        protected override void GivenThat()
        {
            ControlFileFormatText = "INVALID";
            base.GivenThat();
        }

        [Test]
        public void ItShouldThorw()
        {
            Assert.That(ThrownException, Is.InstanceOf<NotSupportedException>());
        }
    }
}