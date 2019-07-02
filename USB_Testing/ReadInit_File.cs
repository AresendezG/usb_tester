using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace USB_Testing
{
    class ReadInit_File
    {
        private string filename_load;
        private XmlDocument document;
        //public string property;


        public ReadInit_File(string filename) // Pass the filename
        {
            filename_load = filename;
            loadfile(); //Only make sure the document exists
            
        }

        private void loadfile()
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename_load);    
        }

        public string get_property(string nodename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename_load);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name == nodename)
                {
                    string text = node.InnerText; //or loop through its children as well 
                    return text;
                }
            }
            return "";
        }

    }
}
