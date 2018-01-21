using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument xDoc;
            xDoc = new XDocument(new XElement("Logs"));
            // xDoc.Add(new XElement("Logs"));
            xDoc.Save("1.xml");
            Console.WriteLine(xDoc.Save);
            Console.Read();
        }
    }
}
