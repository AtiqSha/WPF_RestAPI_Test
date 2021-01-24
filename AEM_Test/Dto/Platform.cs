using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AEM_Test.Dto
{
    public class Platform
    {

        public int id { get; set; }
        public string uniqueName { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
       // [DefaultValue(null)]
        public string createdAt { get; set; }
       // [DefaultValue(null)]
        public string updatedAt { get; set; }
        public Wells[] well { get; set; }
    }

    public class Wells
    {
        public int id { get; set; }
        public int? platformId { get; set; }
        public string uniqueName { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        [DefaultValue(null)]
        public string createdAt { get; set; }
        [DefaultValue(null)]
        public string updatedAt { get; set; }
    }

}