using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BidSoftware.Shared.Export
{
    public class Info
    {
        [XmlElement(ElementName = "minimumBid")]
        public string MinimumBid { get; set; }
        [XmlElement(ElementName = "reservationPrice")]
        public string ReservationPrice { get; set; }
        [XmlElement(ElementName = "initialDate")]
        public string InitialDate { get; set; }
    }

    public class Bid
    {
        [XmlAttribute(AttributeName = "userid")]
        public string UserId { get; set; }
        [XmlAttribute(AttributeName = "datetime")]
        public string Datetime { get; set; }
    }

    public class Bids
    {
        [XmlElement(ElementName = "bid")]
        public Bid[] ArrayBid { get; set; }
        [XmlAttribute(AttributeName = "num")]
        public string Num { get; set; }
    }

    [XmlRoot(ElementName = "auction")]
    public class Auction
    {
        [XmlElement(ElementName = "info")]
        public Info Info { get; set; }
        [XmlElement(ElementName = "bids")]
        public Bids Bids { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    
    public class Exporter
    {
        public static void Export(Auction auction, string path)
        {
            var serializer = new XmlSerializer(typeof(Auction));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, auction);
            }
        }
    }
}
