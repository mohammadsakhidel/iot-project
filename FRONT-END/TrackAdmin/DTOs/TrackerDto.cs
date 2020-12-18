using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class TrackerDto {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Manufacturer { get; set; }
        public string RawID { get; set; }
        public string CommandSet { get; set; }
        public string ProductType { get; set; }
        public string ProductModel { get; set; }
        public string Explanation { get; set; }
        public string DisplayName { get; set; }
        public string IconImageId { get; set; }
        public string SerialNumber { get; set; }
        public string CreationTime { get; set; }
        public string LastConnection { get; set; }
        public string LastConnectedServer { get; set; }

        // Computed Props:
        public string Desc => $"Tracker ID: {Id}\tCommand Set: {CommandSet}\t" +
            $"Last Connection: {LastConnection} to the server: {LastConnectedServer}";
    }
}
