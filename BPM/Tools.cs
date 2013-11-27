using System;
using System.Collections.Generic;
using System.IO;
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
            Console.WriteLine("bpm get-install [nameofplugin] <nameofplugin2> - looks up latest project file of the specified plugin and then downloads it to plugins/.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm get-install-craftbukkit [rb, beta, dev] - downloads one of the specified latest builds of craftbukkit.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm get-install-bukkit [rb, beta, dev] - downloads one of the specified latest builds of bukkit.");
            Console.WriteLine("--------");
            Console.WriteLine("bpm generate-index - indexes all of dev.bukkit.org/server-mods (may take quite awhile!) and outputs them all in a csv under indexes/main.csv");
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
    }
}
