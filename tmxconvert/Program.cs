using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace tmxconvert
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(new StreamReader(File.Open(args[0], FileMode.Open)));

            XmlNode data = xDoc.GetElementsByTagName("layer")[0].FirstChild;
            byte[] decodedData = Convert.FromBase64String(data.InnerText);

            int[] tileData = new int[decodedData.Length / 4];

            for (int i = 0; i < tileData.Length; i++)
            {
                tileData[i] = (int)BitConverter.ToUInt32(decodedData, 4 * i) - 1;
            }



        }
    }
}
