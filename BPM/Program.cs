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
                            if (args.Length > 1)
                            {
                                bool skip = true;
                                foreach (string plugin in args)
                                {
                                    if (!skip)
                                    {
                                        Functions.getInstall(plugin);
                                    }
                                    skip = false;
                                }
                            }
                            else
                            {
                                Tools.showHelp();
                            }
                            break;
                        case "get-install-craftbukkit":
                            if(args.Length > 1)
                            {
                                switch(args[1])
                                {
                                    case "rb":
                                        Functions.getInstallBukkit(BukkitProjectTypes.CRAFTBUKKIT, BukkitDownloadTypes.RB);
                                        break;
                                    case "beta":
                                        Functions.getInstallBukkit(BukkitProjectTypes.CRAFTBUKKIT, BukkitDownloadTypes.BETA);
                                        break;
                                    case "dev":
                                        Functions.getInstallBukkit(BukkitProjectTypes.CRAFTBUKKIT, BukkitDownloadTypes.DEV);
                                        break;
                                    default:
                                        Tools.showHelp();
                                        break;
                                }
                            }
                            break;
                        case "get-install-bukkit":
                            if (args.Length > 1)
                            {
                                switch (args[1])
                                {
                                    case "rb":
                                        Functions.getInstallBukkit(BukkitProjectTypes.BUKKIT, BukkitDownloadTypes.RB);
                                        break;
                                    case "beta":
                                        Functions.getInstallBukkit(BukkitProjectTypes.BUKKIT, BukkitDownloadTypes.BETA);
                                        break;
                                    case "dev":
                                        Functions.getInstallBukkit(BukkitProjectTypes.BUKKIT, BukkitDownloadTypes.DEV);
                                        break;
                                    default:
                                        Tools.showHelp();
                                        break;
                                }
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
    }
}
