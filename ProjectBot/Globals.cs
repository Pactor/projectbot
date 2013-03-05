using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjectBot
{
    public class Globals
    {
        public static string AppDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjectBot");
        public static string ConfigXML = Path.Combine(AppDir, "Config.xml");
        public static string GetProjectLink(string project)
        {
            return "http://code.google.com/feeds/p/" + project + "/svnchanges/basic";
        }
    }
}
