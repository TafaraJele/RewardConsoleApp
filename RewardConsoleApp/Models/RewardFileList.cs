using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RewardConsoleApp.Models
{
    [XmlRoot("Root")]
    public class RewardFileList
    {        
        [XmlElement(ElementName = "Record")]
        public List<RewardCards> rewardCardList;
        public RewardFileList()
        {
            rewardCardList = new List<RewardCards>();
        }
        public void AddRecord (RewardCards rewardCards )
        {

            rewardCardList.Add(rewardCards);

        }

    }
}
