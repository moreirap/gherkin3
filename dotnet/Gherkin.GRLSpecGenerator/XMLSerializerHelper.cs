using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gherkin.GRLSpecGenerator
{
    public class XMLSerializerHelper
    {
        public static String PrintXML(object obj)
        {
            String Result = "";

            var serializer = new XmlSerializer(obj.GetType());
            
            MemoryStream mStream = new MemoryStream();
            var writer = new XmlTextWriter(mStream, Encoding.UTF8);
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
                StreamReader sReader = new StreamReader(mStream);

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
    }


}
