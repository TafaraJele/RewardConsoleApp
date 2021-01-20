using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RewardConsoleApp.Models
{    
    public class RewardCards
    {        
        [XmlElement(ElementName = "fio")]
        public string Fio { get; set; }

        [XmlElement(ElementName = "sex")]
        public string Sex { get; set; }

        [XmlElement(ElementName = "pasnom")]
        public string Pasnom { get; set; }

        [XmlElement(ElementName = "contype")]
        public string Contype { get; set; }

        [XmlElement(ElementName = "schparam")]
        public string Schparam { get; set; }
      
    }
}
