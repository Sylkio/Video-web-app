using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoWebAppApi.Models
{
    public class Video
    {
        public int? Id { get; set; }
        public string? VideoName { get; set; }
        public string? VideoDescription { get; set; }
        public string? Url { get; set; }

    }
}