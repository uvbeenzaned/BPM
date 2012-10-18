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

            checkDirs();
            if (runfromargs)
            {
                if (args.Length < 1)
                {
                    showHelp();
                }
                else
                {
                    switch (args[0])
                    {
                        case "help":
                            showHelp();
                            break;
                        case "get-install":
                            if (args[1] != "" || args[1] != null)
                            {
                                getInstall(args[1]);
                            }
                            else
                            {
                                showHelp();
                            }
                            break;
                        case "generate-index":
                            indexBukkitDev();
                            break;
                        default:
                            showHelp();
                            break;
                    }
                }
            }
            else
            {
                //do custom debugging and coding here
                getInstall("glassback");
            }
        }

        static void showHelp()
        {
            Console.WriteLine("<------------------>");
            Console.WriteLine("bpm.exe help - shows this help.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm.exe get-install <nameofplugin> - looks up latest project file of the specified plugin and then downloads it as a .jar to plugins\\.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm.exe generate-index - indexes all of dev.bukkit.org/server-mods (may take quite awhile!) and outputs them all in a csv under indexes\\main.csv");
            Console.WriteLine("<------------------>");
        }

        static void getInstall(string pluginname)
        {
            string tmpfilename = "pages\\" + pluginname + "tmp.htm";
            DownloadFile("http://dev.bukkit.org/server-mods/" + pluginname + "/", tmpfilename);
            if (File.Exists(tmpfilename))
            {
                pagelines.Clear();
                pagelines = File.ReadAllLines(tmpfilename).ToList<string>();
                string downloadpageaddress = "";
                foreach (var line in pagelines)
                {
                    if (line.Contains("<a href=\"") && line.Contains("\">Download</a>"))
                    {
                        foreach (var item in line.Split('"'))
                        {
                            if (item.StartsWith("/"))
                            {
                                downloadpageaddress = "http://dev.bukkit.org" + item;
                                break;
                            }
                        }
                    }
                }
                pagelines.Clear();
                DownloadFile(downloadpageaddress, tmpfilename);
                pagelines = File.ReadAllLines(tmpfilename).ToList<string>();
                string currline = "";
                foreach (var line in pagelines)
                {
                    if (line.Contains("<dd><a href=\"") && line.Contains("</a></dd>"))
                    {
                        foreach (var item in line.Split('\"'))
                        {
                            if (item.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                            {
                                currline = item;
                                break;
                            }
                        }
                    }
                }
                pagelines.Clear();
                File.Delete(tmpfilename);
                checkDirs();
                Console.WriteLine("Downloading " + pluginname + "....");
                DownloadFile(currline.Split(',')[0], "plugins\\" + pluginname + ".jar");
                Console.WriteLine("Finished downloading " + pluginname + "!");
            }
        }

        static void indexBukkitDev()
        {
            int pgcnt = 1;
            int actualplugincnt = 0;
            checkDirs();
            DownloadFile("http://dev.bukkit.org/server-mods/", "pages\\page" + pgcnt + ".htm");
            if (File.Exists("pages\\page" + pgcnt + ".htm"))
            {
                pagelines = File.ReadAllLines("pages\\page" + pgcnt + ".htm").ToList<string>();
                foreach (var line in pagelines)
                {
                    //<li class="listing-pagination-pages-total">6443 server mods found</li>
                    if (line.Contains("listing-pagination-pages-total"))
                    {
                        //Console.WriteLine(line);
                        string[] splitline = line.Split('>');
                        string singleline = splitline[splitline.Length - 2].Replace("</li", "").Split(' ')[0];
                        actualplugincnt = Convert.ToInt32(singleline);
                        break;
                    }
                }
            }
            checkDirs();
            List<string> newlines = new List<string>();
            for (int i = 0; i < actualplugincnt; )
            {
                DownloadFile("http://dev.bukkit.org/server-mods/?page=" + pgcnt.ToString(), "pages\\page" + pgcnt + ".htm");
                if (File.Exists("pages\\page" + pgcnt + ".htm"))
                {
                    pagelines = File.ReadAllLines("pages\\page" + pgcnt + ".htm").ToList<string>();
                }
                else
                {
                    break;
                }
                foreach (var line in pagelines)
                {
                    string currline = "";
                    //<td class="col-project"><h2><a href="http://dev.bukkit.org/server-mods/quests/">Quests</a></h2></td>
                    if (line.Contains("<td class=\"col-project\">"))
                    {
                        foreach (var item in line.Split('>'))
                        {
                            if (item.EndsWith("</a", StringComparison.InvariantCultureIgnoreCase))
                            {
                                currline = item.Replace("</a", "");
                                break;
                            }
                        }
                        foreach (var item in line.Split('\"'))
                        {
                            if (item.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                            {
                                currline = currline + "," + "http://dev.bukkit.org" + item;
                                break;
                            }
                        }
                        newlines.Add(currline);
                        i++;
                    }
                }
                using (StreamWriter sw = new StreamWriter("indexes\\main.csv"))
                {
                    foreach (var line in newlines)
                    {
                        sw.WriteLine(line);
                        Console.WriteLine(line);
                    }
                }
                pagelines.Clear();
                //File.Delete("pages\\page" + pgcnt + ".htm");
                pgcnt++;
            }
        }

        static void checkDirs()
        {
            if (!Directory.Exists("pages\\"))
            {
                Directory.CreateDirectory("pages\\");
            }
            if (!Directory.Exists("indexes\\"))
            {
                Directory.CreateDirectory("indexes\\");
            }
            if (!Directory.Exists("plugins\\"))
            {
                Directory.CreateDirectory("plugins\\");
            }
        }

        public static void DownloadFile(string url, string filename)
        {
            WebClient wc = new WebClient();
            //wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            //wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
            try
            {
                wc.DownloadFile(new Uri(url), filename);
            }
            catch (Exception)
            {
            }
        }

        static void pause()
        {
            Console.ReadLine();
            Environment.Exit(0);
        }

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
