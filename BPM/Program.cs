using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace BPM
{
    class Program
    {
        static List<string> pagelines = new List<string>();

        static void Main(string[] args)
        {
            //do we want to run from the arguments?
            bool runfromargs = true;

            Tools.checkDirs();
            if (runfromargs)
            {
                if (args.Length < 1)
                {
                    Tools.showHelp();
                }
                else
                {
                    switch (args[0])
                    {
                        case "help":
                            Tools.showHelp();
                            break;
                        case "get-install":
                            if (args[1] != "" || args[1] != null)
                            {
                                Functions.getInstall(args[1]);
                            }
                            else
                            {
                                Tools.showHelp();
                            }
                            break;
                        case "generate-index":
                            Functions.indexBukkitDev();
                            break;
                        default:
                            Tools.showHelp();
                            break;
                    }
                }
            }
            else
            {
                //do custom debugging and coding here
                Functions.getInstall("netfire");
            }
        }

        //debug pause
        static void pause()
        {
            Console.ReadLine();
            Environment.Exit(0);
        }

        //never used async download setup
        //public static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    Console.WriteLine("Downloaded {0} of {1} bytes. {2} % complete...",
        //        e.BytesReceived,
        //        e.TotalBytesToReceive,
        //        e.ProgressPercentage);
        //}

        //public static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        //{
            
        //}
    }
}
