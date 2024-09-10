﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class ExportCarsWithPartsDTO
    {
        [XmlAttribute("make")]
        public string Make { get; set; }
        
        [XmlAttribute("model")]
        public string Model { get; set; }
        
        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartsOfCarExportDTO[] Parts { get; set; }   
    }

    [XmlType("part")]
    public class PartsOfCarExportDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
