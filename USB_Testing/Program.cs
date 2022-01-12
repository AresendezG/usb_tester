using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;

namespace USB_Testing
{

    // USB Utilities with Drive Labels and Mount points.
    // Author: Esli Alejandro Resendez 
    // Last Modified: Jan 2022
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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

        [DllImport("kernel32.dll")]
        static extern bool DeleteVolumeMountPoint(string lpszVolumeMountPoint);

        [DllImport("kernel32.dll")]
        static extern bool SetVolumeMountPoint(string lpszVolumeMountPoint, string lpszVolumeName);

        const int MAX_PATH = 260;


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
                        // validate the Argument character is actually a Letter:
                        char[] ByteVal = MountingLetter.ToUpper().ToCharArray();
                        if (ByteVal[0] > 0x5A || ByteVal[0] < 0x41)
                        {
                            ArgumentException Ax = new ArgumentException();
                            throw Ax;
                        }

                        // Check if letter is available:
                        if (TestUSB.IsLetterInUse(MountingLetter.Substring(0,1)))
                        {
                            // Drive letter selected by user is busy
                            // Find if our drive is the one mounted there:
                            string WhereIsOurDrive = TestUSB.GetDriveLetter(DriveLabel);
                            if (WhereIsOurDrive.Contains(MountingLetter))
                            {
                                Console.WriteLine("Remount Completed");
                                break;
                            }
                            else // There's something mounted in user's letter, but not the desired label
                            {
                                Console.WriteLine("ERROR:\tThere is a Drive already mounted in Letter: " + MountingLetter.Substring(0, 1));
                                break;
                            }
                        }
                        else
                        {
                            if(TestUSB.IsDriveConnected(DriveLabel)) // Check if this Label is connected to Computer
                            {
                                // Remount code using Winapi
                                string CurrentLetter = TestUSB.GetDriveLetter(DriveLabel);
                                string ExpectedDrive = MountingLetter.ToUpper() + ":\\";

                                if (CurrentLetter != ExpectedDrive) {
                                    try
                                    {
                                        ChangeDriveLetter(CurrentLetter, ExpectedDrive);
                                        Console.WriteLine("Remount Completed");
                                    }
                                    catch
                                    {
                                        Console.WriteLine("ERROR: Remount Error");
                                    }
                                }
                                else // Drive is already mounted there
                                {
                                    Console.WriteLine("Remount Completed");
                                }

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

                    case "find_comport":
                        string vid = args[1];
                        string pid = args[2];

                        List<string> DetectedComPorts = ComPortNames(vid, pid);

                        if (DetectedComPorts.Count == 1)
                        {
                            Console.WriteLine("ComPort is: " + DetectedComPorts[0]);
                        }
                        else if (DetectedComPorts.Count == 0)
                        {
                            Console.WriteLine("Error: No USB Device find");
                        }
                        else
                        {
                            Console.WriteLine("Error: More than 1 Device with the given descriptor is connected");
                        }

                        break;
                    case "test_tcp_port":
                        string IPAddress = args[1];
                        string TCP_Port = args[2];

                        TcpClient TestClient = new TcpClient();
                        try
                        {
                            TestClient.Connect(IPAddress, Convert.ToInt32(TCP_Port));
                            if (TestClient.Connected)
                            {
                                Console.WriteLine("TCP Port: " + TCP_Port + " At Host: " + IPAddress + " is Active");
                            }
                            else
                            {
                                Console.WriteLine("Cannot Connect to selected TCP Port");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failure while trying to Connect to the Port: " + TCP_Port);
                        }
                        finally
                        {
                            TestClient.Close();
                        }

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

        private static void ChangeDriveLetter(string CurrentDrive, string ExpectedDrive)
        {
            StringBuilder volume = new StringBuilder(MAX_PATH);
            if (!GetVolumeNameForVolumeMountPoint(@""+CurrentDrive+"", volume, (uint)MAX_PATH))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            if (!DeleteVolumeMountPoint(@"" + CurrentDrive + ""))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            if (!SetVolumeMountPoint(@"" + ExpectedDrive + "", volume.ToString()))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        private static void PrintHelp()
        {
            Console.WriteLine(" - USB Test Console App - ");
            Console.WriteLine("Use: usb_tester.exe [option1] [option2] to Execute RemovableDevices Tests: ");
            Console.WriteLine("Options Available: ");
            Console.WriteLine("1. [list_devices]\tto List All Drives Available in PC");
            Console.WriteLine("2. [list_removable]\tto display All Removable Drives available in PC");
            Console.WriteLine("3. [list_removable_parsable] \tto display All Removable Drives in Minimal Detail");
            Console.WriteLine("4. [count_removable]\t to return the number of Removable Drives in PC");
            Console.WriteLine("5. [copy_read_test] [filename] \tto Execute a Copy/Read Test in one of the Attached Devices");
            Console.WriteLine("6. [list_fixtures]\tIt will return a list with the Removable Drives but only those which contain a pre-defined Fixture Identifier Tag");
            Console.WriteLine("7. [set_newlabel_gui] \tUse a GUI to change the Fixture Identifier Tag");
            Console.WriteLine("8. [set_newlabel_cmd] \t[NEWTAG] set a new Fixture Tag by using commands");
            Console.WriteLine("9. [remount] [USBLABEL] [DRIVE] \tto mount the drive with USBLABEL to the [Drive] letter");
            Console.WriteLine("10. [find_comport] [vid] [pid] \tto find the Com Port using a given vid and pid values");
            Console.WriteLine("11. [help]\tto display this Help Menu");
            Console.WriteLine("12. [About]\t this tool");

        }


        private static List<string> ComPortNames(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();

            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

            foreach (String s3 in rk2.GetSubKeyNames())
            {

                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            string location = (string)rk5.GetValue("LocationInformation");
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            string portName = (string)rk6.GetValue("PortName");
                            if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                                comports.Add((string)rk6.GetValue("PortName"));
                            //RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            //comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }


    }
}


