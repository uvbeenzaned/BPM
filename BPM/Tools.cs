using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BPM
{
    class Tools
    {
        public static void showHelp()
        {
            Console.WriteLine("Author: uvbeenzaned of Networkery.co Build: " + Assembly.GetEntryAssembly().GetName().Version.ToString());
            Console.WriteLine("<------------------>");
            Console.WriteLine("bpm help - shows this help.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm get-install <-x, -e> [nameofplugin] <nameofplugin2> - looks up latest project file of the specified plugin and then downloads it to plugins/.  Arg -x extracts plugins only, arg -e extracts all files.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm get-install-craftbukkit [rb, beta, dev] - downloads one of the specified latest builds of craftbukkit.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm get-install-bukkit [rb, beta, dev] - downloads one of the specified latest builds of bukkit.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm generate-index <\"filename\"> <\"optional external curse url\"> - indexes all of dev.bukkit.org/server-mods (may take quite awhile!) and outputs them all in a csv under indexes/. (filename is optional) (url is optional)");
            Console.WriteLine("<------------------>");
        }

        public static void checkDirs()
        {
            if (!Directory.Exists("pages/"))
            {
                Directory.CreateDirectory("pages/");
            }
            if (!Directory.Exists("indexes/"))
            {
                Directory.CreateDirectory("indexes/");
            }
            if (!Directory.Exists("plugins/"))
            {
                Directory.CreateDirectory("plugins/");
            }
        }

        public static string cut(string s, int a)
        {
            string ns = "";
            char[] ca = s.ToCharArray();
            if (s.Length > a)
            {
                ns = s.Remove(a - 1);
            }
            else
            {
                if (s.Length < a)
                {
                    int max = a - s.Length;
                    for (int i = max; i >= max; i--)
                    {
                        ns = ns + " ";
                    }
                    ns = ns + s;
                }
            }
            return ns;
        }

        public static void smartExtractPluginsOnly(string zippath)
        {
            FileInfo fi = new FileInfo(zippath);
            if (fi.Extension == ".zip")
            {
                ZipArchive archive = ZipFile.OpenRead(zippath);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Extracting > " + entry.Name + " > to plugins/...");
                        entry.ExtractToFile(Path.Combine("plugins/", entry.FullName), true);
                    }
                }
                archive.Dispose();
                fi.Delete();
                Console.WriteLine("Finished extracting plugins!");
            }
        }

        public static void extractAll(string zippath)
        {
            FileInfo fi = new FileInfo(zippath);
            if (fi.Extension == ".zip")
            {
                ZipArchive archive = ZipFile.OpenRead(zippath);
                Console.WriteLine("Extracting all files from >" + fi.Name + " > to plugins/...");
                archive.ExtractToDirectory("plugins/");
                archive.Dispose();
                fi.Delete();
                Console.WriteLine("Finished extracting plugins!");
            }
        }
    }
}
