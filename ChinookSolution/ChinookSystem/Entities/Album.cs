﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChinookSystem.Entities
{
    [Table("Album")]
    public class Album
    {
        private string _ReleaseLabel;

        [Key]
        public int AlbumId { get; set; }

        [StringLength(160, ErrorMessage = "Title is limited to 160 characters.")]
        public string Title { get; set; }

        public int ArtistId { get; set; }

        public int ReleaseYear { get; set; }

        [StringLength(50, ErrorMessage = "Release label is limited to 50 characters.")]
        public string ReleaseLabel
        {
            get { return _ReleaseLabel}
            set { _ReleaseLabel = string.IsNullOrEmpty(value) ? null : value; }
        }
    }
}
