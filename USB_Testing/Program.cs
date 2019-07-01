using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace USB_Testing
{

    // USB Testing Program in C#
    // Author: EARG 
    // ARGE Software® is a Trademark of ARGE Technologies LLC

    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            string option;
            testing_usb TestUSB = new testing_usb();
            Console.WriteLine("USB Testing device Application \n " +
                "1. 'list_devices' for All Devices \n " +
                "2. list_removable for Only Removable Disk Drives \n " +
                "3. Help to show menu: ");

            while (exit == false)
            {
                Console.WriteLine("Enter Option: ");
                option = Console.ReadLine();
               
                if(option.ToLower() == "list_devices")
                {
                    TestUSB.list_devices();
                }
                if (option.ToLower() == "list_removable")
                {
                    TestUSB.list_removable();
                }
                if (option.ToLower() == "list_removable_parsable")
                {
                    TestUSB.list_removable_parsable();
                }
                if (option.ToLower() == "count_removable")
                {
                    Console.WriteLine((TestUSB.count_removable()).ToString()); //Return the number of attached devices
                }
                if (option.ToLower() == "copy_read_test") //Will pick a random device and will try to copy and read back a file
                {
                    string filename_source, filename_dest;
                    Console.WriteLine("Enter filename: ");
                    filename_source = Console.ReadLine();
                    Console.WriteLine("Enter filename: ");
                    filename_dest = Console.ReadLine();
                    TestUSB.File_CopyRead_Test(filename_source, filename_dest);
                }
                if (option.ToLower() == "read_test")
                {
                    //TestUSB.list_removable();
                }

                if (option == "exit")
                {
                    exit = true;
                
                }

            }
            }

    }
}


