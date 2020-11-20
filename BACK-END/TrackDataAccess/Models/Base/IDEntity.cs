using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TrackDataAccess.Models.Base {
    public class IDEntity : Entity {
        
        [Column("id")]
        public string Id { get; set; }

    }
}
