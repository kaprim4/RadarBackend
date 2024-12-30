using Domain.Models;
using Infrastructure.Context;
using Services.ImageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IMageService
{
    public class ImageService : AService, IImageService
    {
        public ImageService(AppDbContext context) :base(context) { }


        public async Task<string> AddImage(string? imagePath, string? videoPath)
        {
            try
            {
                var image = new Images()
                {
                    Name = Guid.NewGuid().ToString(),
                    ImagePath = imagePath ?? "",
                    VideoPath = videoPath ?? "",
                };
                _context.Images.Add(image);

                await _context.SaveChangesAsync();
                return image.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
            

        }

        public string GetImage(string name)
        {
            throw new NotImplementedException();
        }
    }
}
