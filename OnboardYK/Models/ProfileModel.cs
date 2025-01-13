
using System.Xml.Serialization;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Yubico.YubiKey.Piv;

namespace OnboardYK.Models
{
    // Must be public due to XML serialization, otherwise 0x80131509 / System.InvalidOperationException
    [XmlRoot(ElementName = "OnboardYK")]
    public class ProfileModel
    {
        [XmlElement(ElementName = "ShowAllProfiles")]
        public bool ShowAllProfiles { get; set; } = false;
        [XmlElement(ElementName = "Profile")]
        public List<Profile>? Profiles { get; set; }
        [XmlElement(ElementName = "RequireComplexPIN")]
        public bool RequireComplexPIN { get; set; } = false;
        [XmlElement(ElementName = "RequireBlockedPUK")]
        public bool RequireBlockedPUK { get; set; } = false;
        [XmlElement(ElementName = "RetriesPIN")]
        public int RetriesPIN { get; set; } = 8;

        public class Profile
        {
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; } = String.Empty;
            [XmlElement(ElementName = "Slot")]
            public byte Slot { get; set; } = 0x9A;
            [XmlElement(ElementName = "Algorithm")]
            public PivAlgorithm Algorithm { get; set; } = PivAlgorithm.Rsa2048;
            [XmlElement(ElementName = "PinPolicy")]
            public PivPinPolicy PinPolicy { get; set; } = PivPinPolicy.Default;
            [XmlElement(ElementName = "TouchPolicy")]
            public PivTouchPolicy TouchPolicy { get; set; } = PivTouchPolicy.Default;
            [XmlElement(ElementName = "Template")]
            public string Template { get; set; } = string.Empty;
            [XmlElement(ElementName = "CA")]
            public string CA { get; set; } = String.Empty;
            [XmlElement(ElementName = "AlwaysVisible")]
            public bool AlwaysVisible { get; set; } = true;
        }

        private static string ConvertToHumanReadableXml(string inputString)
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true
            };

            var stringBuilder = new StringBuilder();

            var xElement = XElement.Parse(inputString);

            using (var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
            {
                xElement.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        public void SaveToFile(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(ProfileModel));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, this);
                    var xmlData = stringWriter.ToString();

                    File.WriteAllText(path, ConvertToHumanReadableXml(xmlData));
                }
            }
        }
        public string SaveToString()
        {
            var xmlSerializer = new XmlSerializer(typeof(ProfileModel));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, this);
                    var xmlData = stringWriter.ToString();

                    return ConvertToHumanReadableXml(xmlData);
                }
            }
        }

        public static ProfileModel LoadFromFile(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProfileModel));
            //xmlSerializer.UnknownElement += new XmlElementEventHandler(UnknownElementHandler);
            //xmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(UnknownAttributeHandler);

            using (StreamReader reader = new StreamReader(path))
            {
                var xmlData = xmlSerializer.Deserialize(reader.BaseStream);
                if (xmlData is not null)
                {
                    return (ProfileModel)xmlData;
                }
                return new ProfileModel();
            }
        }
    }
}