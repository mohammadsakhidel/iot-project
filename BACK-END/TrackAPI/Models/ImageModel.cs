using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class ImageModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
