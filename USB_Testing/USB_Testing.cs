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
        public string[] USB_Drives = new string[15];
        //public string[] USB2_Drives = new string[15];

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

        public bool IsDriveConnected(string DriveLabel)
        {

            foreach (DriveInfo d in available_drives)
            {

                if ((d.DriveType.ToString()).ToLower() == "removable")
                {
                    if (d.IsReady == true)
                    {
                        if (d.VolumeLabel == DriveLabel)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

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

        public int list_removable_parsable()
        {
            int drives_found = 0;
            Console.WriteLine("- Drive Label - \t - Windows Root - \t - FileSystem -");
            foreach (DriveInfo d in available_drives)
            {

                if ((d.DriveType.ToString()).ToLower() == "removable")
                {
                    if (d.IsReady == true)
                    {
                        Console.WriteLine("{0} \t {1} \t {2}", d.VolumeLabel, d.RootDirectory, d.DriveFormat);
                        drives_found++;
                    }
                }
            }

            return drives_found;

        }

        public bool IsLetterInUse(string DriveLetter)
        {

            if (DriveLetter.Length > 0)
            {
                DriveLetter = DriveLetter.ToUpper()+":";
                foreach (DriveInfo d in available_drives)
                {
                    if (d.Name.ToString().Contains(DriveLetter))
                     return true;
                    
                }
                return false;
            }
           else 
             return false;
        }


        public int list_fixtures(string FixPrefix)
        {
            int FixtureFound = 0;
           // Console.WriteLine("- Drive Label - \t - Windows Root - \t - FileSystem -");
            foreach (DriveInfo d in available_drives)
            {

                if ((d.DriveType.ToString()).ToLower() == "removable")
                {
                    if (d.IsReady == true)
                    {
                        if (d.VolumeLabel.Contains(FixPrefix))
                        {
                            Console.WriteLine("{0} \t {1} \t {2}", d.VolumeLabel, d.RootDirectory, d.DriveFormat);
                            FixtureFound++;
                        }
                    }
                }
            }

            return FixtureFound;
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



        public int Test_Drive_RW(string source_filename, string Volume_Label, string USB_Drive_Type, int random_l, int random_h) 
        // Pick the filename to be copied, pick the USB drive type, pick a range for select a random device out of the list 
        {

            string dest_filename = "E:\\testfile.bin";
            double data_transfer = 0;
            int i = 0;
            //Get a random USB Drive

            // Pick a random port having the desired label
            int usb_drive = 0;
                usb_drive = RandomNumber(random_l, random_h);
            //Find all drives that have the desired volume label

            foreach (DriveInfo d in available_drives)
            {
                if (d.VolumeLabel.ToString().IndexOf(Volume_Label) != -1)
                {
                    this.USB_Drives[i] = d.Name;
                    i++;
                }

            }

            try //Write a file Test
            {
               
                dest_filename = Path.Combine(this.USB_Drives[usb_drive], "TestFile.bin");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                File.Copy(source_filename, dest_filename); //Copy the file to USB2.0 drive
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                FileInfo TS_File = new FileInfo(source_filename); // Load transferred file to get data

                data_transfer = (TS_File.Length / ts.TotalSeconds) / 1000000; //Transferred MB/s
                Console.WriteLine("Estimated Transfer Speed MB/s {0}: {1}", USB_Drive_Type, data_transfer);
            }

            catch (ArgumentNullException dnfe)
            {
                if (USB_Drives[usb_drive] == null) // Argument null exception triggered due to no drives with label USB3 found
                    Console.WriteLine("ERROR: No {0} Drives were found", USB_Drive_Type);
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
                Console.WriteLine("ERROR: No {0} Drive was found", USB_Drive_Type);
                Console.WriteLine(dnfe.Message);
                return dnfe.HResult;
            }

            catch (UnauthorizedAccessException dnfe)
            {
                DriveInfo d = new DriveInfo(USB_Drives[usb_drive]);
                Console.WriteLine("ERROR: Cannot Write into this USB Drive: {0}", d.VolumeLabel);
                Console.WriteLine(dnfe.Message); //File already copied
                return dnfe.HResult;
            }


            // ReadingTest
            FileInfo USB_File = new FileInfo(dest_filename); // Load transferred file to get data
            Console.WriteLine("Read file {0} from {1} Drive with Lenght {2}", USB_File.Name, USB_Drive_Type, USB_File.Length);
            //Cleanup and delete files from drive USB
            File.Delete(dest_filename);

            return 0;



        }




        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}
