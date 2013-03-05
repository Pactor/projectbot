using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace ProjectBot.Managers
{
    public class XMLManager
    {
        public static void Load()
        {
            try
            {
                XmlDocument Doc = new XmlDocument();
                Doc.Load(Globals.ConfigXML);
            }
            catch
            {
                CreateConfig();
            }
        }
        static void CreateConfig()
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("GoogleSVNBot XMLConfig"),
                new XElement("ROOT",
                    new XElement("MySqlConfig",
                        new XElement("Server", "localhost"),
                        new XElement("DataBase", "dbName"),
                        new XElement("UserName", "root"),
                        new XElement("Password", "password")),
                        new XComment("Feeds List"),
                        new XElement("Feeds")));
            // Save it
            doc.Save(Globals.ConfigXML);
        }
        public static bool AddFeed(string feedName)
        {
            string feedLink = "http://code.google.com/feeds/p/" + feedName + "/svnchanges/basic";

            try
            {
                if (!GetFeedByName(feedName))
                {
                    XDocument doc = XDocument.Load(Globals.ConfigXML);
                    var feed = doc.Root.Element("Feeds");
                    feed.Add(new XElement("Feed",
                        new XElement("FeedName", feedName),
                        new XElement("FeedUrl", feedLink)));
                    doc.Save(Globals.ConfigXML);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool RemoveFeed(string feedName)
        {
            string feedLink = "http://code.google.com/feeds/p/" + feedName + "/svnchanges/basic";
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                XElement toRemove = doc.Descendants("Feeds")
                 .Descendants("Feed")
                 .Where(x => x.Element("FeedName").Value == feedName)
                 .First<XElement>();
                toRemove.Remove();
                doc.Save(Globals.ConfigXML);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool GetFeedByName(string feedName)
        {
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                XElement toRemove = doc.Descendants("Feeds")
                 .Descendants("Feed")
                 .Where(x => x.Element("FeedName").Value == feedName)
                 .First<XElement>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static List<string> GetAllFeedNames()
        {
            List<string> feeds = new List<string>();
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                foreach (XElement desc in doc.Descendants("Feeds").Descendants("Feed"))
                {
                    feeds.Add(desc.Descendants("FeedName").DescendantsAndSelf().FirstOrDefault().Value);
                }
            }
            catch
            {
                Console.WriteLine("Error gathering Feednames!");
            }
            return feeds;
        }
        public static bool ClearAllFeeds()
        {
            try
            {
                foreach (string feed in GetAllFeedNames())
                {
                    RemoveFeed(feed);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region Mysql
        public static string GetMySqlConnection()
        {
            string conn = string.Empty;
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                conn = "SERVER=" + doc.Descendants("ROOT")
                 .Descendants("MySqlConfig").Elements("Server").FirstOrDefault().Value + ";";
                conn += "DATABASE=" + doc.Descendants("ROOT")
                    .Descendants("MySqlConfig").Elements("DataBase").FirstOrDefault().Value + ";";
                conn += "UID=" + doc.Descendants("ROOT")
                    .Descendants("MySqlConfig").Elements("UserName").FirstOrDefault().Value + ";";
                conn += "Password=" + doc.Descendants("ROOT")
                    .Descendants("MySqlConfig").Elements("Password").FirstOrDefault().Value + ";";
            }
            catch
            {
                Console.WriteLine("Error in MySql Connection String!");
            }
            return conn;
        }
        public static bool SetMySqlServerName(string servername)
        {
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                doc.Descendants("ROOT")
                 .Descendants("MySqlConfig").Elements("Server").FirstOrDefault().Value = servername;
                doc.Save(Globals.ConfigXML);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SetMySqlDataBase(string database)
        {
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                doc.Descendants("ROOT")
                 .Descendants("MySqlConfig").Elements("DataBase").FirstOrDefault().Value = database;
                doc.Save(Globals.ConfigXML);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SetMySqlUserName(string username)
        {
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                doc.Descendants("ROOT")
                 .Descendants("MySqlConfig").Elements("Username").FirstOrDefault().Value = username;
                doc.Save(Globals.ConfigXML);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SetMySqlPassword(string password)
        {
            try
            {
                XDocument doc = XDocument.Load(Globals.ConfigXML);
                doc.Descendants("ROOT")
                 .Descendants("MySqlConfig").Elements("Password").FirstOrDefault().Value = password;
                doc.Save(Globals.ConfigXML);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
