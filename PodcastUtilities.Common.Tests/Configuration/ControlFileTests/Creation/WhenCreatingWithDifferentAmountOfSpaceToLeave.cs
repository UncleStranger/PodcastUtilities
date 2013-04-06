﻿using System;
using System.Xml;
using NUnit.Framework;
using PodcastUtilities.Common.Configuration;

namespace PodcastUtilities.Common.Tests.Configuration.ControlFileTests.Creation
{
    public abstract class WhenCreatingAControlFileWithDifferentAmountOfSpaceToLeave : WhenTestingAControlFile
    {
        protected string ControlFileFreeSpaceText { get; set; }
        protected long FreeSpaceToLeave { get; set; }
        protected Exception ThrownException { get; set; }

        protected override void GivenThat()
        {
            base.GivenThat();

            XmlNode n = ControlFileXmlDocument.SelectSingleNode("podcasts/global/freeSpaceToLeaveOnDestinationMB");
            n.InnerText = ControlFileFreeSpaceText;

            ControlFile = new ReadOnlyControlFile(ControlFileXmlDocument);
        }

        protected override void When()
        {
            ThrownException = null;
            try
            {
                FreeSpaceToLeave = ControlFile.GetFreeSpaceToLeaveOnDestination();
            }
            catch (Exception exception)
            {
                ThrownException = exception;
            }
        }
    }

    public class WhenCreatingAControlFileWithAnAmountOfSpaceToLeave : WhenCreatingAControlFileWithDifferentAmountOfSpaceToLeave
    {
        protected override void GivenThat()
        {
            ControlFileFreeSpaceText = "1234";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadFreeSpaceToLeave()
        {
            Assert.That(FreeSpaceToLeave, Is.EqualTo(1234));
        }
    }

    public class WhenCreatingAControlFileWithAnIllegalAmountOfSpaceToLeave : WhenCreatingAControlFileWithDifferentAmountOfSpaceToLeave
    {
        protected override void GivenThat()
        {
            ControlFileFreeSpaceText = "XYZ";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadFreeSpaceToLeave()
        {
            Assert.That(FreeSpaceToLeave, Is.EqualTo(0));
        }
    }

}