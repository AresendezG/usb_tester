using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace USB_Testing
{

    // USB Utilities with Drive Labels and Mount points.
    // Author: Esli Alejandro Resendez 
    // Last Modified: May 2021
    // ARGE Software® is a Trademark of ARGE Technologies, LLC

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
            string action;
            string parameter;
            testing_usb TestUSB = new testing_usb();
            ReloadOptions();

            /*
            Console.WriteLine("USB Testing device Application \n " +
                "1. 'list_devices' for All Devices \n " +
                "2. list_removable for Only Removable Disk Drives \n " +
                "3. Help to show menu: "); */

            try
            {
                action = args[0];
                action = action.ToLower();
                


                switch (action)
                {

                    case "remount":
                        // If user did not pass the right args, it will go into an exception
                        string DriveLabel = args[1];
                        string MountingLetter = args[2];
                        // User passed a correct 

                        if (MountingLetter.Length != 1)
                        {
                            IndexOutOfRangeException e = new IndexOutOfRangeException();
                            throw e;
                        }
                        // validate the character is a Letter:
                        char[] ByteVal = MountingLetter.ToUpper().ToCharArray();
                        if (ByteVal[0] > 0x5A || ByteVal[0] < 0x41)
                        {
                            ArgumentException Ax = new ArgumentException();
                            throw Ax;
                        }

                        // Check if letter is available:
                        if (TestUSB.IsLetterInUse(MountingLetter.Substring(0,1)))
                        {
                            Console.WriteLine("ERROR:\tDrive already mounted in Letter: "+MountingLetter.Substring(0,1));
                            break;
                        }
                        else
                        {
                            if(TestUSB.IsDriveConnected(DriveLabel)) // Label is valid
                            {
                                // Remount code using Winapi
                            }
                            else
                            {
                                Console.WriteLine("ERROR:\tThis Drive is not Connected to the System");
                            }

                        }


                        break;

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
                        int count = 0;
                        count = TestUSB.list_fixtures(Settings1.Default.FIX_LABEL);
                        Console.WriteLine("Fixtures found: {0}", count);
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
                        PrintHelp();
                        break;
                    case "about":
                        AboutBox1 AB = new AboutBox1();
                        AB.ShowDialog();
                        break;
                    case "savelabel_settings":
                        SetConfigLabels FM = new SetConfigLabels();
                        FM.ShowDialog();
                        ReloadOptions();
                        break;
                    default:
                        PrintHelp();
                        break;
                }
              
            }
            catch (IndexOutOfRangeException)
            {
                PrintHelp();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("ERROR:\tThe Argument is not a valid Letter");
            }

            }

        private static void ReloadOptions()
        {
            // Read Expected Labels from Settings File
            usb2_label = Settings1.Default.USB_2_LABEL;
            usb3_label = Settings1.Default.USB_3_LABEL;
        }

        private static void PrintHelp()
        {
            Console.WriteLine(" - USB Test Console App - ");
            Console.WriteLine("Use: usb_tester.exe [option1] [option2] to Execute RemovableDevices Tests: ");
            Console.WriteLine("Options Available: ");
            Console.WriteLine("1. [list_devices] \tto List All Drives Available in PC");
            Console.WriteLine("2. [list_removable] \tto display All Removable Drives available in PC");
            Console.WriteLine("3. [list_removable_parsable] \tto display All Removable Drives in Minimal Detail");
            Console.WriteLine("4. [count_removable] \t to return the number of Removable Drives in PC");
            Console.WriteLine("5. [copy_read_test] [filename] \tto Execute a Copy/Read Test in one of the Attached Devices");
            Console.WriteLine("6. [list_fixtures] \tIt will return a list with the Removable Drives but only those which contain a pre-defined Fixture Identifier Tag");
            Console.WriteLine("7. [set_newlabel_gui] \tUse a GUI to change the Fixture Identifier Tag");
            Console.WriteLine("8. [set_newlabel_cmd] \t[NEWTAG] set a new Fixture Tag by using commands");
            Console.WriteLine("9. [remount] [USBLABEL] [DRIVE] \tto mount the drive with USBLABEL to the [Drive] letter");
            Console.WriteLine("10. [help] \tto display this Help Menu");
            Console.WriteLine("11. [About] \t this tool");

        }
           

    }
}


