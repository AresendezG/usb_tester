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


    /*
         USB Testing device Application as Command Console
         list_devices for All Devices
         list_removable for Only Removable Disk Drives
         count_removable to display how many removable devices are attached to PC
        
         
     */


    class Program
    {
        static void Main(string[] args)
        {
            // bool exit = false;
            string option;
            testing_usb TestUSB = new testing_usb();

            /*
            Console.WriteLine("USB Testing device Application \n " +
                "1. 'list_devices' for All Devices \n " +
                "2. list_removable for Only Removable Disk Drives \n " +
                "3. Help to show menu: "); */

            //while (exit == false) //Removing Continuous Loop

            // Console.WriteLine("Enter Option: ");
            try
            {
                option = args[0];

                if (option.ToLower() == "list_devices")
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
                    string filename_source;

                    // Console.WriteLine("Enter filename: ");
                    try
                    {
                        filename_source = args[1];
                        if (args[1] != null)
                        {
                            int result = TestUSB.File_WR_Test(filename_source);
                            if (result != 0)
                                Console.WriteLine("Operation Finished with ERROR code: {0}", result);
                        }
                        else
                            Console.WriteLine("Please enter the Test filename [EX]: USBTest.exe copy_read_test c:\\image\\testimage.jpg");
                    }
                    catch (IndexOutOfRangeException no_arg)
                    {
                        Console.WriteLine("Please enter the Test filename [Example]: USBTest.exe copy_read_test c:\\image\\testimage.jpg");
                        Console.WriteLine(no_arg.Message);
                    }
                }
                if (option.ToLower() == "read_test")
                {
                    //TestUSB.list_removable();
                }

                if (option == "help" || option == null)
                {
                    Console.Write("Enter argument option USBTesting [option] [option] to execute USB Automated Test: " +
                        "1. list_devices To list All drives" +
                        "2. list_removable_parsable to display info of removable devices" +
                        "3. copy_read_test [filename.extension] to copy, read and delete file test" +
                        "4. help to display this help message");
                    // exit = true;

                }


            }
            catch (IndexOutOfRangeException e)
            {
               
                    Console.Write("Enter argument option USBTesting [option] [option] to execute USB Automated Test: \n" +
                        "1. list_devices To list All drives \n" +
                        "2. list_removable_parsable to display info of removable devices \n" +
                        "3. copy_read_test [filename.extension] to copy, read and delete file test \n" +
                        "4. help to display this help message \n\n");
                    // exit = true;
            }

            }
           

    }
}


