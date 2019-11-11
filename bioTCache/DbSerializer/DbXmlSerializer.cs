using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace bioTCache
{
    public class DbXmlSerializer : ISerializer
    {
        public string SerializeSimple<T>(T i_Obj)
        {
            StringBuilder str = new StringBuilder();
            foreach (var prop in i_Obj.GetType().GetProperties())
            {

                str.Append("<" + prop.Name + ">");
                    str.Append(prop.GetValue(i_Obj,null));
                str.Append("</" + prop.Name + ">");
            }

            return str.ToString();
        }

        public string SerializePrettyPrintSimple<T>(T i_Obj)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<xml>" +Environment.NewLine);
            foreach (var prop in i_Obj.GetType().GetProperties())
            {
                str.Append("\t<" + prop.Name + "> ");
                str.Append(prop.GetValue(i_Obj, null) );
                str.Append(" </" + prop.Name + ">" + Environment.NewLine);
            }
            str.Append("</xml>" + Environment.NewLine);

            return str.ToString();
        }

        public string Serialize<T>(T i_Value)
        {
            if (i_Value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, i_Value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public T Deserialize<T>(string i_Xml) 
        {
            if (string.IsNullOrEmpty(i_Xml))
            {
                return default(T);
            }
            try
            {
                using (var stringReader = new StringReader(i_Xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

    }
}
