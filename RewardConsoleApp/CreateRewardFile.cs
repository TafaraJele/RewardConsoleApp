using Microsoft.Extensions.Configuration;
using RewardConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RewardConsoleApp
{
    public class CreateRewardFile
    {
        private IConfiguration _iconfiguration;
        public CreateRewardFile(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        /// <summary>
        /// Gets Card Details from the Database
        /// </summary>
        /// <returns></returns>
        public RewardFileList GetRewardCards(List<RewardCustomerDetails> customers)
        {
            //Get Reward cards from Database
            //var rewardFileDbContext = new RewardFileDbContext(_iconfiguration);

            var list = new RewardFileList();

            // var rewardfiles = rewardFileDbContext.GetList();           

            var rewardfiles = GetCards(customers);

            foreach (var rewardcard in rewardfiles)
            {

                list.AddRecord(rewardcard);

            }


            return list;

        }
        public void GenerateRewardFile(RewardFileList list, string[] fileNameParameters)
        {
            //Get File path from settings           
            var path = (_iconfiguration.GetSection("DirectorySettings:FilePath")).Value;

            //Create File Name
            Random random = new Random();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string number = random.Next(0, 9999).ToString("D4");
            string reference = $"{fileNameParameters[0]}_{fileNameParameters[1]}_Open_Contract{number}_{date}";
            string filename = $"{reference}.xml";

            // Create and save Reward XML File  
            string filePath = Path.Combine(path, filename);
            var xmlString = "";
            using (var stream = new StringWriter())
            {
                var opts = new XmlWriterSettings { OmitXmlDeclaration = true };
                using (var writer = XmlWriter.Create(stream, opts))
                {
                    var xml = new XmlSerializer(list.GetType());
                    Directory.CreateDirectory(path);
                    xml.Serialize(writer, list);
                    writer.Flush();
                }
                xmlString = stream.ToString();
            }
            var doc = XDocument.Parse(xmlString);
            doc.Root.RemoveAttributes();
            doc.Save(filePath);
        }
        private List<RewardCards> GetCards(List<RewardCustomerDetails> customers)
        {
            string fio = "";
            string customerSex = "";
            var rewardCards = new List<RewardCards>();
            foreach (var rewardCard in customers)
            {
                fio = $"{rewardCard.CustomerFirstName} {rewardCard.CustomerMiddleName} {rewardCard.CustomerLastName}";
                if (rewardCard.CustomerTitleId == "0")
                {
                    customerSex = "M";
                }
                else if ((rewardCard.CustomerTitleId == "1") || (rewardCard.CustomerTitleId == "2") || (rewardCard.CustomerTitleId == "3"))
                {

                    customerSex = "F";
                }
                else
                {
                    customerSex = "Unknown";
                }
                rewardCards.Add(new RewardCards
                {
                    Fio = fio,
                    Sex = customerSex,
                    Pasnom = rewardCard.CustomerId,
                    Schparam = $"{rewardCard.CardNumber}-0-1",
                    Contype = "NA"

                });
                
            };
            return rewardCards;
        }

    }
}