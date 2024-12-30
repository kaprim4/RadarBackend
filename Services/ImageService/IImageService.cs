using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageService
{
    public interface IImageService
    {
        string GetImage(string name);
        Task<string> AddImage(string? imagePath, string? videoPath);
    }
}
