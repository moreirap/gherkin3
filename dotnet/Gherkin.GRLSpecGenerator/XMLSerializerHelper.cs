using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gherkin.GRLCatalogueGenerator
{
    public class XMLSerializerHelper
    {
        public static String SerializeObject(object obj)
        {
            String Result = "";

            var serializer = new XmlSerializer(obj.GetType());
            
            MemoryStream mStream = new MemoryStream();
            var writer = new XmlTextWriter(mStream, new UTF8Encoding(false));
            writer.Formatting = Formatting.Indented;

            try
            {
                // Write the XML into a formatting XmlTextWriter
                serializer.Serialize(writer, obj);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream, new UTF8Encoding(false));

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return Result;
        }

        public static string RemoveBOM(string text)
        {
            byte[] withBom = { 0xef, 0xbb, 0xbf, 0x41 };
            string viaEncoding = Encoding.UTF8.GetString(withBom);
            return !String.IsNullOrEmpty(text) && text.Length >= 3 && text.StartsWith(viaEncoding) ? text.Substring(3) : text; 
        }
    }


}
