using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Drawing;

namespace ProjectBot.Managers
{
    class RssFeedReader
    {
        public XmlDocument Read(string rssUri)
        {
            Uri _rssFeed = null;
            XmlDocument rss = new XmlDocument();
            try
            {
                _rssFeed = new Uri(rssUri);

                HttpWebRequest request = WebRequest.Create(_rssFeed) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    rss.Load(reader);

                }
                // Just print it out for now
                //w.WriteRaw(rss.OuterXml);
                //Console.WriteLine(rss.OuterXml);
            }
            catch (Exception e)
            {
                ProjectBot.WriteLine(e.Message, Color.Red);
            }

            return rss;

        }
        public bool TryRead(string rssUri, out XmlDocument doc)
        {
            Uri _rssFeed = null;
            XmlDocument rss = new XmlDocument();
            doc = new XmlDocument();
            try
            {
                _rssFeed = new Uri(rssUri);

                HttpWebRequest request = WebRequest.Create(_rssFeed) as HttpWebRequest;
                //XmlWriter w = XmlWriter.Create(files + "_feed.xml");
                //files++;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    rss.Load(reader);
                    doc = rss;

                }
                // Just print it out for now
                //w.WriteRaw(rss.OuterXml);
                // Even a brand new project has text here, if it doesnt its not valid
                if (rss.OuterXml == string.Empty)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }

        }
        public GoogleCodeProject ConvertToProject(XmlDocument doc)
        {
            GoogleCodeProject project = new GoogleCodeProject();
            if (doc != null)
            {
                //TODO: Grab and parse the content block
                // K should be good, process the file.
                //Get the root elemnent of this xmldoc
                XmlElement root = doc.DocumentElement;
                // Grab the node we want
                XmlNodeList entrys = root.GetElementsByTagName("entry");
                // I just want the first entry node
                XmlNode entry = entrys[0];
                // Create our feed class
                project.Updated = entry["updated"].InnerText;
                // Fix the Description text
                foreach(string line in entry["content"].InnerText.Split('>').Last().Split('\n'))
                {
                    project.Description += line + "";
                }
                project.Link = entry["link"].GetAttribute("href");
                project.Author = entry["author"].InnerText;
                project.Version = entry["id"].InnerText.Split('/').Last();
                // Strip the first part of this message
                string buf = "http://code.google.com/feeds/p/";
                project.ProjectName = entry["id"].InnerText.Substring(buf.Length).Split('/').First();
            }
            return project;
        }
    }
}
