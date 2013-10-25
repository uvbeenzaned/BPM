using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPM
{
    class Functions
    {
        static List<string> pagelines = new List<string>();

        public static void getInstall(string pluginname)
        {
            Console.WriteLine("Parsing BukkitDev pages....");
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
                Tools.checkDirs();
                string newpname = currline.Split(',')[0].Split('/')[currline.Split(',')[0].Split('/').Length - 1];
                if (!string.IsNullOrWhiteSpace(newpname))
                {
                    Console.WriteLine("Downloading " + newpname + "....");
                    DownloadFile(currline.Split(',')[0], "plugins\\" + newpname);
                    Console.WriteLine("Finished downloading " + newpname + "!");
                }
                else
                {
                    Console.WriteLine("The name of the plugin you specified does not exist on dev.bukkit.org or the site is down.");
                }
            }
        }

        public static void indexBukkitDev()
        {
            int pgcnt = 1;
            int actualplugincnt = 0;
            Tools.checkDirs();
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
            Tools.checkDirs();
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
                string currline = "";
                foreach (var line in pagelines)
                {
                    //<td class="col-project"><h2><a href="http://dev.bukkit.org/server-mods/quests/">Quests</a></h2></td>
                    if (line.Contains("<td class=\"col-project\">"))
                    {
                        foreach (var item in line.Split('>'))
                        {
                            if (item.EndsWith("</a", StringComparison.InvariantCultureIgnoreCase))
                            {
                                currline = "\"" + item.Replace("</a", "") + "\",";
                                break;
                            }
                        }
                        foreach (var item in line.Split('\"'))
                        {
                            if (item.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                            {
                                currline = currline + "http://dev.bukkit.org" + item;
                                break;
                            }
                        }
                    }
                    if (line.Contains("<td class=\"col-date\">"))
                    {
                        bool stop = false;
                        foreach (var item in line.Split('\"'))
                        {
                            if (stop == true)
                            {
                                currline = currline + "," + "\"" + item + "\"";
                                break;
                            }
                            if (item.Contains("title"))
                            {
                                stop = true;
                            }
                        }
                    }
                    if (line.Contains("<ul class=\"comma-separated-list\">"))
                    {
                        string users = "\"";
                        foreach (string user in line.Split('>'))
                        {
                            if (user.Contains("</a"))
                            {
                                users = users + user.Replace("</a", "") + ", ";
                            }
                        }
                        users = users.TrimEnd(',', ' ');
                        users = users + "\"";
                        currline = currline + "," + users;
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

        private static void DownloadFile(string url, string filename)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.DownloadFile(new Uri(url), filename);
            }
            catch (Exception)
            {
            }
        }
    }
}
