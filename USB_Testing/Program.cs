using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace USB_Testing
{

    //Including some changes to do stuff

        // Incluiding some more changes
   class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            string option;
            testing_usb TestUSB = new testing_usb();
            Console.WriteLine("USB Testing device Application, Enter option or help: ");
            while (exit == false)
            {
                Console.WriteLine("Enter Option: ");
                option = Console.ReadLine();
               
                if(option == "List_Devices")
                {
                    TestUSB.list_devices();
                }
                if (option == "exit")
                {
                    exit = true;
                
                }

            }
            }

    }
}

public class testing_usb
{

    public string[] listed_devices;
    public void list_devices()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        int drives_found = 0;

        foreach (DriveInfo d in allDrives)
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

        Console.WriteLine("Found: " + drives_found.ToString()+ "Drives Connected");
    }
}
