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
    // Last Modified: October 2020
    // ARGE Software® is a Trademark of ARGE Technologies LLC

    /*
         USB Testing device Application as Command Console
         list_devices for All Devices
         list_removable for Only Removable Disk Drives
         count_removable to display how many removable devices are attached to PC
                
     */

    class Program
    {

        private static string usb2_label, usb3_label;

        static void Main(string[] args)
        {
            // bool exit = false;
            string option;
            testing_usb TestUSB = new testing_usb();
            reloadOptions();

            /*
            Console.WriteLine("USB Testing device Application \n " +
                "1. 'list_devices' for All Devices \n " +
                "2. list_removable for Only Removable Disk Drives \n " +
                "3. Help to show menu: "); */

            try
            {
                option = args[0];
                option = option.ToLower();

                switch (option)
                {
                    case "list_devices": // List All the Drives in PC
                        TestUSB.list_devices();
                        break;
                    case "list_removable": // List all the Removable Drives
                        TestUSB.list_removable();
                        break;
                    case "list_removable_parsable": // List All Removable Devices in Parsable Format
                        int returncode = TestUSB.list_removable_parsable();
                        Console.WriteLine("Total Listed Devices: {0}",returncode);
                        break;
                    case "count_removable": // Return the number of attached devices
                        Console.WriteLine((TestUSB.count_removable()).ToString()); 
                        break;

                    case "set_newlabel_gui":
                        SetFixLabel FLG = new SetFixLabel();
                        FLG.ShowDialog();
                        break;
                    case "set_newlabel_cmd":
                        Settings1.Default.FIX_LABEL = args[1];
                        break;

                    case "list_fixtures":
                        TestUSB.list_fixtures(Settings1.Default.FIX_LABEL);
                        break;

                    case "copy_read_test":
                        string filename_source;
                        // Console.WriteLine("Enter filename: ");
                        try
                        {
                            filename_source = args[1];
                            int random_l, random_h;
                            if (args[1] != null)
                            {
                                if (args[2] == "group3")
                                {
                                    random_l = 0;
                                    random_h = 1;
                                }
                                else
                                {
                                    random_l = 0;
                                    random_h = 4;
                                }
                                int result = TestUSB.Test_Drive_RW(filename_source, usb2_label, "USB2.0", random_l, random_h);
                                if (result != 0)
                                    Console.WriteLine("Operation Finished with ERROR code: {0}", result);
                                else
                                {
                                    result = TestUSB.Test_Drive_RW(filename_source, usb3_label, "USB3.0", random_l, random_h);
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
                        break;
                    case "read_test":
                        break;
                    case "help":
                        printHelp();
                        break;
                    case "about":
                        AboutBox1 AB = new AboutBox1();
                        AB.ShowDialog();
                        break;
                    case "savelabel_settings":
                        SetConfigLabels FM = new SetConfigLabels();
                        FM.ShowDialog();
                        reloadOptions();
                        break;
                    default:
                        printHelp();
                        break;
                }
              
            }
            catch (IndexOutOfRangeException)
            {
                printHelp();
            }

            }

        private static void reloadOptions()
        {
            // Read Expected Labels from Settings File
            usb2_label = Settings1.Default.USB_2_LABEL;
            usb3_label = Settings1.Default.USB_3_LABEL;
        }

        private static void printHelp()
        {
            Console.WriteLine(" - USB Test Console App - ");
            Console.WriteLine("Use: USB_Testing [option1] [option2] to Execute RemovableDevices Tests: ");
            Console.WriteLine("Options Available: ");
            Console.WriteLine("1. [list_devices] \t to List All Drives Available in PC");
            Console.WriteLine("2. [list_removable] \t to display All Removable Drives available in PC");
            Console.WriteLine("3. [list_removable_parsable] \t to display All Removable Drives in Minimal Detail");
            Console.WriteLine("4. [count_removable] \t to return the number of Removable Drives in PC");
            Console.WriteLine("5. [copy_read_test] \t [filename.extension] to Execute a Copy/Read Test in one of the Attached Devices");
            Console.WriteLine("6. [list_fixtures] \t It will return a list with the Removable Drives but only those which contain a pre-defined Fixture Identifier Tag");
            Console.WriteLine("7. [set_newlabel_gui] \t Use a GUI to change the Fixture Identifier Tag");
            Console.WriteLine("8. [set_newlabel_cmd] \t [NEWTAG] set a new Fixture Tag by using commands");
            Console.WriteLine("6. [help] \t to display this Help Menu");
            Console.WriteLine("7. [About] \t this tool");

        }
           

    }
}


