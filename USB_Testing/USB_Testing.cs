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

        public void list_removable_parsable() {

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

        public int count_removable(){
            int drives_found = 0;
            foreach (DriveInfo d in available_drives)
            {
                if ((d.DriveType.ToString()).ToLower() == "removable") { 
                    if (d.IsReady == true) {
                    drives_found++;
                    }
                }
            }
            return drives_found;
        }


        public void File_CopyRead_Test(string source_filename, string dest_filename)
        {
            double data_transfer = 0;
            //Get a random USB Drive
            try
            {
                
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                File.Copy(source_filename, dest_filename); //Copy the file
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                FileInfo TS_File = new FileInfo(source_filename); // Load transferred file to get data
                
                    data_transfer = TS_File.Length / ts.TotalSeconds; //Transferred bytes by second
                Console.WriteLine("Transfer Speed: {0}", data_transfer);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine("Drive was not found!");
                Console.WriteLine(dnfe.Message);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR: File not found");
            }
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}
