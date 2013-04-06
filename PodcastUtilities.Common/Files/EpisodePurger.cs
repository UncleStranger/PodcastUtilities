﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Exceptions;
using PodcastUtilities.Common.Platform;

namespace PodcastUtilities.Common.Files
{
    /// <summary>
    /// purge old download files
    /// </summary>
    public class EpisodePurger : IEpisodePurger
    {
        private IDirectoryInfoProvider _directoryInfoProvider;
        private readonly ITimeProvider _timeProvider;

        /// <summary>
        /// create the purger
        /// </summary>
        public EpisodePurger(ITimeProvider timeProvider, IDirectoryInfoProvider directoryInfoProvider)
        {
            _directoryInfoProvider = directoryInfoProvider;
            _timeProvider = timeProvider;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static DateTime GetWhenDownloadWasPublished(PodcastInfo podcastInfo, IFileInfo file)
        {
            switch (podcastInfo.Feed.NamingStyle.Value)
            {
                case PodcastEpisodeNamingStyle.UrlFileNameAndPublishDateTime:
                case PodcastEpisodeNamingStyle.UrlFileNameFeedTitleAndPublishDateTime:
                case PodcastEpisodeNamingStyle.EpisodeTitleAndPublishDateTime:
                case PodcastEpisodeNamingStyle.UrlFileNameFeedTitleAndPublishDateTimeInfolder:
                    try
                    {
                        return ConvertFilenameToPublishedDate(Path.GetFileNameWithoutExtension(file.FullName));
                    }
                    catch (Exception)
                    {
                        return file.CreationTime;
                    }
                case PodcastEpisodeNamingStyle.EpisodeTitle:
                case PodcastEpisodeNamingStyle.UrlFileName:
                    return file.CreationTime;
                default:
                    throw new EnumOutOfRangeException("NamingStyle");
            }
        }

        private static DateTime ConvertFilenameToPublishedDate(string fileName)
        {
            return new DateTime(
                    Convert.ToInt32(fileName.Substring(0, 4), CultureInfo.InvariantCulture),
                    Convert.ToInt32(fileName.Substring(5, 2), CultureInfo.InvariantCulture),
                    Convert.ToInt32(fileName.Substring(8, 2), CultureInfo.InvariantCulture),
                    Convert.ToInt32(fileName.Substring(11, 2), CultureInfo.InvariantCulture),
                    Convert.ToInt32(fileName.Substring(13, 2), CultureInfo.InvariantCulture),
                    0
                );
        }


        /// <summary>
        /// find old downloads that can be deleted
        /// </summary>
        /// <param name="rootFolder">the root folder for all downloads</param>
        /// <param name="podcastInfo">info on the podcast to download</param>
        /// <returns>list of episodes to be deleted for the supplied podcastInfo</returns>
        public IList<IFileInfo> FindEpisodesToPurge(string rootFolder, PodcastInfo podcastInfo)
        {
            List<IFileInfo> episodesToDelete = new List<IFileInfo>(10);
            if (podcastInfo.Feed == null)
            {
                // it is optional to have a feed
                return episodesToDelete;
            }

            var feedDownloadsFolder = Path.Combine(rootFolder, podcastInfo.Folder);
            var oldestEpisodeToKeep = DateTime.MinValue;
            if (podcastInfo.Feed.DeleteDownloadsDaysOld.Value < int.MaxValue)
            {
                oldestEpisodeToKeep = _timeProvider.UtcNow.AddDays(-podcastInfo.Feed.DeleteDownloadsDaysOld.Value);
            }

            if (IsSubFolderBasedNaming(podcastInfo.Feed.NamingStyle.Value))
            {
                ScanSubFoldersForOldFiles(feedDownloadsFolder, oldestEpisodeToKeep, episodesToDelete, podcastInfo);
            }
            else
            {
                ScanFolderForOldFiles(feedDownloadsFolder,oldestEpisodeToKeep,episodesToDelete,podcastInfo);
            }

            return episodesToDelete;
        }

        private static bool IsSubFolderBasedNaming(PodcastEpisodeNamingStyle style)
        {
            return style == PodcastEpisodeNamingStyle.UrlFileNameFeedTitleAndPublishDateTimeInfolder;
        }

        private void ScanSubFoldersForOldFiles(string folderToScan, DateTime oldestEpisodeToKeep, List<IFileInfo> episodesToDelete, PodcastInfo podcastInfo)
        {
            var directoryInfo = _directoryInfoProvider.GetDirectoryInfo(folderToScan);

            IDirectoryInfo[] subFolders;
            try
            {
                subFolders = directoryInfo.GetDirectories("*.*");
            }
            catch (DirectoryNotFoundException)
            {
                // if the folder is not there then there is nothing to do
                return;
            }

            foreach (var subFolder in subFolders)
            {
                ScanFolderForOldFiles(subFolder.FullName,oldestEpisodeToKeep,episodesToDelete,podcastInfo);
            }
        }

        private void ScanFolderForOldFiles(string folderToScan, DateTime oldestEpisodeToKeep, List<IFileInfo> episodesToDelete, PodcastInfo podcastInfo)
        {
            var directoryInfo = _directoryInfoProvider.GetDirectoryInfo(folderToScan);

            IFileInfo[] files;
            try
            {
                files = directoryInfo.GetFiles(podcastInfo.Pattern.Value);
            }
            catch (DirectoryNotFoundException)
            {
                // if the folder is not there then there is nothing to do
                return;
            }

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FullName);
                if (extension != null && extension.ToUpperInvariant() == ".XML")
                {
                    // do not delete the state file
                    continue;
                }
                if (GetWhenDownloadWasPublished(podcastInfo, file) < oldestEpisodeToKeep)
                    episodesToDelete.Add(file);
            }
        }
    }
}