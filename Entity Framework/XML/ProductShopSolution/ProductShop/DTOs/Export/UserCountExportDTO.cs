using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Users")]
    public class UserCountExportDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UsersData[] Users { get; set; }

    }

    [XmlType("User")]
    public class UsersData
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        public SoldProductsCount SoldProducts { get; set; }    
    }

    [XmlType("SoldProducts")]
    public class SoldProductsCount
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public SoldProductsData[] Products { get; set; }
    }

    [XmlType("Product")]
    public class SoldProductsData
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }

    }
}
