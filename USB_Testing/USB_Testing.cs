using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                        Console.WriteLine("  File system: {0}", d.DriveFormat);
                        Console.WriteLine(
                            "  Total size of drive:            {0, 15} bytes ",
                            d.TotalSize);
                    }
                }
            }
        }

        public void File_CopyRead_Test()
        {

        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}
