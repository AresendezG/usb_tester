using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace USB_Testing
{
    class ReadInit_File
    {
        private string filename_load;
        public bool load_correct;
        //public string property;


        public ReadInit_File(string filename) // Pass the filename
        {
            filename_load = filename;
            load_correct = loadfile(); //Only make sure the document exists

        }

        private bool loadfile()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filename_load);
                return true;
            }
        catch(FileNotFoundException e)
            {
                //File not found, unable to load 
                Console.WriteLine(e.Message);
                return false;
            }
        catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

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
