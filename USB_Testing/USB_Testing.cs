using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace USB_Testing
{
    public class testing_usb
    {
        private DriveInfo[] available_drives;

        public string dest_filename;
        public string source_filename;
        public string[] USB3_Drives = new string[10];
        public string[] USB2_Drives = new string[10];

        public testing_usb()
        {
            //Constructor code
            available_drives = DriveInfo.GetDrives();
        }



        public string[] listed_devices;
        public void list_devices()
        {

            int drives_found = 0;

            foreach (DriveInfo d in available_drives)
            {
                drives_found++;
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }

            Console.WriteLine("Found: " + drives_found.ToString() + "Drives Connected");
        }

        public void list_removable() //Will display only removable drives
        {
            int drives_found = 0;

            foreach (DriveInfo d in available_drives)
            {

                if ((d.DriveType.ToString()).ToLower() == "removable")
                {
                    drives_found++;
                    if (d.IsReady == true)
                    {

                        Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                        Console.WriteLine("  Root Directory: {0}", d.RootDirectory);
                        Console.WriteLine("  File system: {0}", d.DriveFormat);
                        Console.WriteLine(
                            "  Total size of drive:            {0, 15} bytes ",
                            d.TotalSize);
                    }
                }
            }
        }

        public void list_removable_parsable()
        {

            foreach (DriveInfo d in available_drives)
            {

                if ((d.DriveType.ToString()).ToLower() == "removable")
                {

                    Console.WriteLine("Label \t Root \t FileSystem");
                    if (d.IsReady == true)
                    {

                        Console.WriteLine("{0} \t {1} \t {2}", d.VolumeLabel, d.RootDirectory, d.DriveFormat);
                    }
                }
            }


        }

        public int count_removable()
        {
            int drives_found = 0;
            foreach (DriveInfo d in available_drives)
            {
                if ((d.DriveType.ToString()).ToLower() == "removable")
                {
                    if (d.IsReady == true)
                    {
                        drives_found++;
                    }
                }
            }
            return drives_found;
        }


        public int File_WR_Test(string source_filename)
        {
            string dest_filename_USB2 = "E:\\testfile.bin";
            string dest_filename_USB3 = "F:\\testfile.bin";
            double data_transfer = 0;
            int i = 0;
            int j = 0;
            //Get a random USB Drive

            // Pick a random USB 3.0 Drive
            int usb2_drive = 0;
            // Pick a random USB 2.0 drive
            int usb3_drive = 0;

            foreach (DriveInfo d in available_drives)
            {
                if (d.VolumeLabel.ToString().IndexOf("MOTOUS") != -1)
                {
                    this.USB3_Drives[i] = d.Name;
                    i++;
                }
                if (d.VolumeLabel.ToString().IndexOf("_USB2") != -1)
                {
                    this.USB2_Drives[j] = d.Name;
                    j = j + 1;
                }

            }

            try //Write a file in the USB 2.0 drive and log the speed
            {
                //int RandomDrive = RandomNumber(0, 11);
                dest_filename_USB2 = Path.Combine(this.USB2_Drives[usb2_drive], "TestFile.bin");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                File.Copy(source_filename, dest_filename_USB2); //Copy the file to USB2.0 drive
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                FileInfo TS_File = new FileInfo(source_filename); // Load transferred file to get data

                data_transfer = (TS_File.Length / ts.TotalSeconds) / 1000000; //Transferred MB/s
                Console.WriteLine("Estimated Transfer Speed MB/s USB2.0: {0}", data_transfer);
            }

            catch (ArgumentNullException dnfe)
            {
                if (USB2_Drives[usb2_drive] == null) // Argument null exception triggered due to no drives with label USB3 found
                    Console.WriteLine("ERROR: No USB2.0 Drives were found");
                else
                    Console.WriteLine("ERROR: Unexpected Error, see message");
                return dnfe.HResult;
            }

            catch (IOException dnfe)
            {
                Console.WriteLine("ERROR: IO Error Exception, the file might be already copied to drive or non existent");
                Console.WriteLine(dnfe.Message);
                return dnfe.HResult;
            }

            catch (IndexOutOfRangeException dnfe)
            {
                Console.WriteLine("ERROR: No USB2.0 Drive was found");
                Console.WriteLine(dnfe.Message);
                return dnfe.HResult;
            }

            catch (UnauthorizedAccessException dnfe)
            {
                DriveInfo d = new DriveInfo(USB2_Drives[usb2_drive]);
                Console.WriteLine("ERROR: Cannot Write into this USB Drive: {0}", d.VolumeLabel);
                Console.WriteLine(dnfe.Message); //File already copied
                return dnfe.HResult;
            }


            try //Write a sample file to USB 3.0 and log the speed
            {

                dest_filename_USB3 = Path.Combine(USB3_Drives[usb3_drive], "TestFile.bin");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                File.Copy(source_filename, dest_filename_USB3); //Copy the file
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                FileInfo TS_File = new FileInfo(source_filename); // Load transferred file to get data

                data_transfer = (TS_File.Length / ts.TotalSeconds) / 1000000; //Transferred MB/s
                Console.WriteLine("Estimated Transfer Speed USB3.0: {0}", data_transfer);
            }

            catch (ArgumentNullException dnfe)
            {

                if (USB3_Drives[usb3_drive] == null) // Argument null exception triggered due to no drives with label USB3 found
                    Console.WriteLine("ERROR: No USB3.0 Drives were found");
                else
                    Console.WriteLine("ERROR: Unexpected Error, see message");
                return dnfe.HResult;
            }


            catch (IndexOutOfRangeException dnfe) // Triggered if USB3_Drives array is empty (No drives were detected)
            {
                Console.WriteLine("ERROR: No USB3.0 Drive was found");
                Console.WriteLine(dnfe.Message);
            }


            catch (UnauthorizedAccessException dnfe)
            {
                DriveInfo d = new DriveInfo(USB3_Drives[usb3_drive]);
                Console.WriteLine("ERROR: Cannot Write into this USB Drive: {0}", d.VolumeLabel);
                Console.WriteLine(dnfe.Message); //File already copied
                return dnfe.HResult;
            }

            catch (IOException dnfe)
            {
                Console.WriteLine("ERROR: File might be already copied or Non Existent, see message");
                Console.WriteLine(dnfe.Message);
                return dnfe.HResult;
            }


            // ReadingTest

            FileInfo USB2_File = new FileInfo(dest_filename_USB2); // Load transferred file to get data

            Console.WriteLine("Read file {0} from 2.0 Drive with Lenght {1}", USB2_File.Name, USB2_File.Length);


            FileInfo USB3_File = new FileInfo(dest_filename_USB3);
            Console.WriteLine("Read file {0} from 3.0 Drive with Lenght {1}", USB3_File.Name, USB3_File.Length);

            //Cleanup and delete files from drive USB
            File.Delete(dest_filename_USB2);
            File.Delete(dest_filename_USB3);

            return 0;

        }



        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}
