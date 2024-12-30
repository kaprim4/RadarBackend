using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class ImageHelper 
    {
        private static ImageHelper _instance;
        private readonly ResourceSettings _resourceSettings;

        // Constructor for Dependency Injection
        public ImageHelper(IOptions<ResourceSettings> resourceSettings)
        {
            _resourceSettings = resourceSettings.Value;
            _instance = this;
        }

        // Static property to access the singleton instance
        public static ImageHelper Instance => _instance ?? throw new InvalidOperationException("ImageHelper is not initialized.");

        // Instance method for DI usage
        public string GetResourcePath(bool isVideo = false)
        {
            CheckPath();
            if (isVideo)
            {

                return _resourceSettings.RecourceDirectory_Video;
            }
            else
            {
                return _resourceSettings.RecourceDirectory_Image;
            }
        }

        public void CheckPath()
        {
            if (!Directory.Exists(_resourceSettings.RecourceDirectory_Image))
                Directory.CreateDirectory(_resourceSettings.RecourceDirectory_Image);

            if (!Directory.Exists(_resourceSettings.RecourceDirectory_Video))
                Directory.CreateDirectory(_resourceSettings.RecourceDirectory_Video);
        }

        // Static method that uses the singleton instance
        public static string GetResourcePathStatic(bool isVideo = false)
        {
            return Instance.GetResourcePath(isVideo);
        }

    }
}
