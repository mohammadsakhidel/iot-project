using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;

namespace TrackAPI.Services {
    public class ImageService : IImageService {

        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public ImageService(IImageRepository imageRepository, IMapper mapper) {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<(bool, string)> CreateAsync(ImageModel model) {

            var image = _mapper.Map<ImageModel, Image>(model);
            image.Id = Guid.NewGuid().ToString();
            image.CreationTime = DateTime.UtcNow;

            _imageRepository.Add(image);
            await _imageRepository.SaveAsync();
            return (true, image.Id);

        }

        public async Task<ImageModel> GetAsync(string id) {

            var image = await _imageRepository.GetAsync(id);
            if (image == null)
                return null;

            var model = _mapper.Map<Image, ImageModel>(image);
            return model;

        }

        public async Task<(bool, string)> RemoveAsync(string id) {

            var image = await _imageRepository.GetAsync(id);
            if (image == null)
                return (false, "Image not found!");

            _imageRepository.Remove(image);
            await _imageRepository.SaveAsync();
            return (true, string.Empty);

        }
    }
}
