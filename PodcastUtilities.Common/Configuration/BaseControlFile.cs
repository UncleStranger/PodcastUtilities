﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using PodcastUtilities.Common.Exceptions;
using PodcastUtilities.Common.Playlists;

namespace PodcastUtilities.Common.Configuration
{
    /// <summary>
    /// base operations to work with controlfiles
    /// </summary>
    public abstract class BaseControlFile : IControlFileGlobalDefaults
    {
        private const string DefaultSortFieldToken = "Name";
        private const string DefaultSortDirectionToken = "ascending";
        private const string DefaultFeedFormatToken = "rss";
        private const string DefaultFeedNamingStyleToken = "pubdate_url";
        private const string DefaultFeedEpisodeDownloadStrategyToken = "all";

        /// <summary>
        /// the field we are using to sort the podcasts on
        /// </summary>
        private string _sortField;
        /// <summary>
        /// direction to sort in
        /// </summary>
        private string _sortDirection;
        /// <summary>
        /// global default maximum days old for feed download
        /// </summary>
        private string _feedMaximumDaysOld;
        /// <summary>
        /// global default number of days before deleteing a download
        /// </summary>
        private string _feedDeleteDownloadsDaysOld;
        /// <summary>
        /// global default feed format 
        /// </summary>
        private string _feedFormat;
        /// <summary>
        /// global default for naming downloaded episodes
        /// </summary>
        private string _feedEpisodeNamingStyle;
        /// <summary>
        /// global default for naming downloaded episodes
        /// </summary>
        private string _feedEpisodeDownloadStrategy;

        /// <summary>
        /// setup the hard coded defaults
        /// </summary>
        protected BaseControlFile()
        {
            FreeSpaceToLeaveOnDestination = 0;
            FreeSpaceToLeaveOnDownload = 0;
            MaximumNumberOfConcurrentDownloads = 5;
            RetryWaitInSeconds = 10;
        }

        /// <summary>
        /// the global default for feeds
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetDefaultDeleteDownloadsDaysOld()
        {
            return Convert.ToInt32(_feedDeleteDownloadsDaysOld, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// the global default for feeds
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public PodcastEpisodeDownloadStrategy GetDefaultDownloadStrategy()
        {
            return FeedInfo.ReadFeedEpisodeDownloadStrategy(_feedEpisodeDownloadStrategy);
        }

        /// <summary>
        /// the global default for feeds
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public PodcastFeedFormat GetDefaultFeedFormat()
        {
            return FeedInfo.ReadFeedFormat(_feedFormat);
        }

        /// <summary>
        /// the global default for feeds
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetDefaultMaximumDaysOld()
        {
            return Convert.ToInt32(_feedMaximumDaysOld, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// the global default for feeds
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public PodcastEpisodeNamingStyle GetDefaultNamingStyle()
        {
            return FeedInfo.ReadFeedEpisodeNamingStyle(_feedEpisodeNamingStyle);
        }

        /// <summary>
        /// the global default for podcasts
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool GetDefaultAscendingSort()
        {
            return PodcastInfo.ReadSortDirection(_sortDirection);
        }

        /// <summary>
        /// the global default for podcasts
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public PodcastFileSortField GetDefaultSortField()
        {
            return PodcastInfo.ReadSortField(_sortField);
        }

        /// <summary>
        /// pathname to the root folder to copy from when synchronising
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal string SourceRoot { get; set; }

        /// <summary>
        /// pathname to the destination root folder
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal string DestinationRoot { get; set; }

        /// <summary>
        /// filename and extension for the generated playlist
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal string PlaylistFileName { get; set; }

        /// <summary>
        /// the format for the generated playlist
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal PlaylistFormat PlaylistFormat { get; set; }

        /// <summary>
        /// free space in MB to leave on the destination device when syncing
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal long FreeSpaceToLeaveOnDestination { get; set; }

        /// <summary>
        /// free space in MB to leave on the download device - when downloading
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal long FreeSpaceToLeaveOnDownload { get; set; }

        /// <summary>
        /// the configuration for the individual podcasts
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal IList<PodcastInfo> Podcasts { get; private set; }

        /// <summary>
        /// maximum number of background downloads
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal int MaximumNumberOfConcurrentDownloads { get; set; }

        /// <summary>
        /// number of seconds to wait when trying a file conflict
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal int RetryWaitInSeconds { get; set; }

        /// <summary>
        /// pathname to the root folder to copy from when synchronising
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetSourceRoot()
        {
            return SourceRoot;
        }

        /// <summary>
        /// pathname to the destination root folder
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetDestinationRoot()
        {
            return DestinationRoot;
        }

        /// <summary>
        /// filename and extension for the generated playlist
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetPlaylistFileName()
        {
            return PlaylistFileName;
        }

        /// <summary>
        /// the format for the generated playlist
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public PlaylistFormat GetPlaylistFormat()
        {
            return PlaylistFormat;
        }

        /// <summary>
        /// free space in MB to leave on the destination device when syncing
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public long GetFreeSpaceToLeaveOnDestination()
        {
            return FreeSpaceToLeaveOnDestination;
        }

        /// <summary>
        /// free space in MB to leave on the download device - when downloading
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public long GetFreeSpaceToLeaveOnDownload()
        {
            return FreeSpaceToLeaveOnDownload;
        }

        /// <summary>
        /// the configuration for the individual podcasts
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<PodcastInfo> GetPodcasts()
        {
            return Podcasts;
        }

        /// <summary>
        /// maximum number of background downloads
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetMaximumNumberOfConcurrentDownloads()
        {
            return MaximumNumberOfConcurrentDownloads;
        }

        /// <summary>
        /// number of seconds to wait when trying a file conflict
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetRetryWaitInSeconds()
        {
            return RetryWaitInSeconds;
        }

        /// <summary>
        /// load the control file from an xml file on disk
        /// </summary>
        /// <param name="fileName">filename of the xml file</param>
        protected void LoadFromFile(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            ReadGlobalSection(xmlDocument);
            ReadPodcasts(xmlDocument);
        }

        /// <summary>
        /// load the control file from an in memory object
        /// </summary>
        /// <param name="document">doument to load from</param>
        protected void LoadFromXml(IXPathNavigable document)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.InnerXml = document.CreateNavigator().InnerXml;

            ReadGlobalSection(xmlDocument);
            ReadPodcasts(xmlDocument);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ReadGlobalSection(XmlDocument xmlDocument)
        {
            SourceRoot = GetNodeText(xmlDocument,"podcasts/global/sourceRoot");
            DestinationRoot = GetNodeText(xmlDocument,"podcasts/global/destinationRoot");
            PlaylistFileName = GetNodeText(xmlDocument,"podcasts/global/playlistFilename");

            string format = GetNodeText(xmlDocument,"podcasts/global/playlistFormat").ToUpperInvariant();
            switch (format)
            {
                case "WPL":
                    PlaylistFormat = PlaylistFormat.WPL;
                    break;
                case "ASX":
                    PlaylistFormat = PlaylistFormat.ASX;
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "{0} is not a valid value for the playlist format", format));
            }

            try
            {
                FreeSpaceToLeaveOnDestination = Convert.ToInt64(GetNodeText(xmlDocument,"podcasts/global/freeSpaceToLeaveOnDestinationMB"), CultureInfo.InvariantCulture);
            }
            catch {}

            try
            {
                FreeSpaceToLeaveOnDownload = Convert.ToInt64(GetNodeText(xmlDocument,"podcasts/global/freeSpaceToLeaveOnDownloadMB"), CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                MaximumNumberOfConcurrentDownloads = Convert.ToInt32(GetNodeText(xmlDocument, "podcasts/global/maximumNumberOfConcurrentDownloads"), CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                RetryWaitInSeconds = Convert.ToInt32(GetNodeText(xmlDocument,"podcasts/global/retryWaitInSeconds"), CultureInfo.InvariantCulture);
            }
            catch { }

            _sortField = GetNodeTextOrDefault(xmlDocument,"podcasts/global/sortfield", DefaultSortFieldToken);
            _sortDirection = GetNodeTextOrDefault(xmlDocument,"podcasts/global/sortdirection", DefaultSortDirectionToken);
            _feedMaximumDaysOld = GetNodeTextOrDefault(xmlDocument,"podcasts/global/feed/maximumDaysOld", int.MaxValue.ToString(CultureInfo.InvariantCulture));
            _feedDeleteDownloadsDaysOld = GetNodeTextOrDefault(xmlDocument,"podcasts/global/feed/deleteDownloadsDaysOld", int.MaxValue.ToString(CultureInfo.InvariantCulture));
            _feedFormat = GetNodeTextOrDefault(xmlDocument,"podcasts/global/feed/format", DefaultFeedFormatToken);
            _feedEpisodeNamingStyle = GetNodeTextOrDefault(xmlDocument, "podcasts/global/feed/namingStyle", DefaultFeedNamingStyleToken);
            _feedEpisodeDownloadStrategy = GetNodeTextOrDefault(xmlDocument,"podcasts/global/feed/downloadStrategy", DefaultFeedEpisodeDownloadStrategyToken);
        }

        private void ReadPodcasts(XmlDocument xmlDocument)
        {
            Podcasts = new List<PodcastInfo>();

            var podcastNodes = xmlDocument.SelectNodes("podcasts/podcast");

            if (podcastNodes != null)
            {
                foreach (XmlNode podcastNode in podcastNodes)
                {
                    var podcastInfo = new PodcastInfo(this)
                                          {
                                              Feed = ReadFeedInfo(podcastNode.SelectSingleNode("feed")),
                                              Folder = GetNodeText(podcastNode, "folder"),
                                              Pattern = GetNodeText(podcastNode, "pattern"),
                                              MaximumNumberOfFiles =
                                                  Convert.ToInt32(GetNodeTextOrDefault(podcastNode, "number", "-1"),
                                                                  CultureInfo.InvariantCulture)
                                          };

                    var nodeText = GetNodeTextOrDefault(podcastNode, "sortdirection", "");
                    if (!string.IsNullOrEmpty(nodeText))
                    {
                        podcastInfo.AscendingSort.Value =
                            !(nodeText.ToUpperInvariant().StartsWith("DESC", StringComparison.Ordinal));
                    }

                    nodeText = GetNodeTextOrDefault(podcastNode, "sortfield", "");
                    if (!string.IsNullOrEmpty(nodeText))
                    {
                        podcastInfo.SortField.Value = PodcastInfo.ReadSortField(nodeText);
                    }


                    Podcasts.Add(podcastInfo);
                }
            }
        }

        private IFeedInfo ReadFeedInfo(XmlNode feedNode)
        {
            if (feedNode == null)
            {
                return null;
            }
            var newFeed = new FeedInfo(this)
            {
                Address = new Uri(GetNodeText(feedNode, "url")),
            };

            var nodeText = GetNodeTextOrDefault(feedNode, "format", "");
            if (!string.IsNullOrEmpty(nodeText))
            {
                newFeed.Format.Value = FeedInfo.ReadFeedFormat(nodeText);
            }

            nodeText = GetNodeTextOrDefault(feedNode, "namingStyle", "");
            if (!string.IsNullOrEmpty(nodeText))
            {
                newFeed.NamingStyle.Value = FeedInfo.ReadFeedEpisodeNamingStyle(nodeText);
            }

            nodeText = GetNodeTextOrDefault(feedNode, "downloadStrategy", "");
            if (!string.IsNullOrEmpty(nodeText))
            {
                newFeed.DownloadStrategy.Value = FeedInfo.ReadFeedEpisodeDownloadStrategy(nodeText);
            }

            nodeText = GetNodeTextOrDefault(feedNode, "maximumDaysOld", "");
            if (!string.IsNullOrEmpty(nodeText))
            {
                newFeed.MaximumDaysOld.Value = ReadMaximumDaysOld(feedNode);
            }

            nodeText = GetNodeTextOrDefault(feedNode, "deleteDownloadsDaysOld", "");
            if (!string.IsNullOrEmpty(nodeText))
            {
                newFeed.DeleteDownloadsDaysOld.Value = ReadDeleteDownloadsDaysOld(feedNode);
            }

            return newFeed;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private int ReadMaximumDaysOld(XmlNode feedNode)
        {
            if (feedNode == null)
            {
                return int.MaxValue;
            }
            try
            {
                return Convert.ToInt32(GetNodeTextOrDefault(feedNode, "maximumDaysOld", _feedMaximumDaysOld), CultureInfo.InvariantCulture);
            }
            catch
            {
                return int.MaxValue;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private int ReadDeleteDownloadsDaysOld(XmlNode feedNode)
        {
            if (feedNode == null)
            {
                return int.MaxValue;
            }
            try
            {
                var days = Convert.ToInt32(GetNodeTextOrDefault(feedNode, "deleteDownloadsDaysOld", _feedDeleteDownloadsDaysOld), CultureInfo.InvariantCulture);
                if (days == 0)
                {
                    return int.MaxValue;
                }
                return days;
            }
            catch
            {
                return int.MaxValue;
            }
        }

        private static string GetNodeText(XmlNode root, string xpath)
        {
            XmlNode n = root.SelectSingleNode(xpath);
            if (n == null)
            {
                throw new XmlStructureException("GetNodeText : Node path '" + xpath + "' not found");
            }
            return n.InnerText;
        }

        private static string GetNodeTextOrDefault(XmlNode root, string xpath, string defaultText)
        {
            XmlNode n = root.SelectSingleNode(xpath);

            return ((n != null && !string.IsNullOrEmpty(n.InnerText)) ? n.InnerText : defaultText);
        }
    }
}