using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Entities
{
    public class LocationData
    {
        public long ID { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [MaxLength(4)]
        public string LatitudeMark { get; set; }

        [MaxLength(4)]
        public string LongitudeMark { get; set; }

        [Required]
        public double Speed { get; set; }

        [Required]
        public double Direction { get; set; }

        public double Altitude { get; set; }

        public double GsmSignalStrengh { get; set; }

        [Required]
        public double Power { get; set; }

        [MaxLength(16)]
        public string TerminalStateHex { get; set; }

        [Required]
        public double Accuracy { get; set; }
    }
}
