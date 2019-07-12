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
            string option, usb2_label, usb3_label;
            testing_usb TestUSB = new testing_usb();

            /*
            Console.WriteLine("USB Testing device Application \n " +
                "1. 'list_devices' for All Devices \n " +
                "2. list_removable for Only Removable Disk Drives \n " +
                "3. Help to show menu: "); */

            //while (exit == false) //Removing Continuous Loop
            // Console.WriteLine("Enter Option: ");

            //Loading Init Options

                // ReadInit_File Init_Options = new ReadInit_File("c:\\mtp\\site_ne_sch\\projects\\pdh_be\\hardwareio\\USBTester\\init.xml");
                ReadInit_File Init_Options = new ReadInit_File(@"init.xml");

            if (Init_Options.load_correct)
            {
                usb2_label = Init_Options.get_property("usb2_expected_label");
                usb3_label = Init_Options.get_property("usb3_expected_label");
            }
            else
            {
                Init_Options = null; // Retry with default init file
                Console.WriteLine("Trying to load Init File from [C:\\mtp\\site_ne_sch\\projects\\PDH_BE\\HardwareIO\\USB_Test\\init.xml]");
                Init_Options = new ReadInit_File("c:\\mtp\\site_ne_sch\\projects\\PDH_BE\\HardwareIO\\USBTester\\init.xml");
                if (Init_Options.load_correct)
                {
                    usb2_label = Init_Options.get_property("usb2_expected_label");
                    usb3_label = Init_Options.get_property("usb3_expected_label");
                }
                else {
                    Console.WriteLine("Unable to Load Init File, will use default Options");
                    usb2_label = "_USB2";
                    usb3_label = "_USB3";

                }
            }

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
                    int returncode = TestUSB.list_removable_parsable();
                    if (returncode == 0)
                        Console.WriteLine("Listed Devices");
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
                            int result = TestUSB.Test_Drive_RW(filename_source, usb2_label, "USB2.0");
                            if (result != 0)
                                Console.WriteLine("Operation Finished with ERROR code: {0}", result);
                            else
                            {
                                result = TestUSB.Test_Drive_RW(filename_source, usb3_label, "USB3.0");
                                if (result != 0)
                                    Console.WriteLine("Operation Finished with ERROR code: {0}", result);
                            }

                        }
                        else
                            Console.WriteLine("ERROR: Enter Test filename [EX]: USBTest.exe copy_read_test c:\\image\\testimage.jpg");
                    }
                    catch (IndexOutOfRangeException no_arg)
                    {
                        Console.WriteLine("ERROR: No Test filename [Example]: USBTest.exe copy_read_test c:\\image\\testimage.jpg");
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
            catch (IndexOutOfRangeException)
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


