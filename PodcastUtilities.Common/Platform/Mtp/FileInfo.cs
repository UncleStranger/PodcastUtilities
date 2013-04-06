using System;
using PodcastUtilities.PortableDevices;

namespace PodcastUtilities.Common.Platform.Mtp
{
    ///<summary>
    /// Implementation of abstract file info for MTP "files" (child objects)
    ///</summary>
    public class FileInfo
        : IFileInfo
    {
        private readonly IDevice _device;
        private IDeviceObject _deviceObject;
        private readonly string _path;

        internal FileInfo(IDevice device, string path)
            : this(device, null, path)
        {
        }

        internal FileInfo(IDevice device, IDeviceObject deviceObject, string path)
        {
            _device = device;
            _deviceObject = deviceObject;
            _path = path;
        }

        /// <summary>
        /// the name of the file eg. file.ext
        /// </summary>
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// the full pathname of the object eg. c:\media\file.ext
        /// </summary>
        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// date time the file was created
        /// </summary>
        public DateTime CreationTime
        {
            get { throw new NotImplementedException(); }
        }

        private IDeviceObject DeviceObject
        {
            get
            {
                return _deviceObject ?? (_deviceObject = _device.GetObjectFromPath(_path));
            }
        }
    }
}