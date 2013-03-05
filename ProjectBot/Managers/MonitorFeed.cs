using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBot.Events;
using System.Threading;
using System.Drawing;
using System.Xml;

namespace ProjectBot.Managers
{
    public class MonitorFeed
    {
        public delegate void ChangeDetected(object sender, ChangeArgs e);
        public event ChangeDetected changeDetected;
        private GoogleCodeProject curentProject;

        private RssFeedReader Reader;
        private Thread ReadRss { get; set; }
        public MonitorFeed(string projectname)
        {
            ReadRss = new Thread(delegate()
                {
                    // get the project name turns into a googlecode in a thread
                    if (curentProject == null) { curentProject = GetNewProject(Globals.GetProjectLink(projectname)); }
                    // If the curent project isnt null, check it again
                    GoogleCodeProject project = GetNewProject(Globals.GetProjectLink(projectname));
                    if (project != null)
                    {
                        // check to see if they are the same

                        if (!CompareUpdates(curentProject, project))
                        {
                            // Diferent Raise event
                            ChangeArgs args = new ChangeArgs(project);
                            changeDetected(this, args);

                        }
                    }
                });
            ReadRss.IsBackground = true;
            ReadRss.Start();
        }
        /// <summary>
        /// A http://code.google.com/feeds/p/ link
        /// </summary>
        /// <param name="projectLink">A http://code.google.com/feeds/p/ link</param>
        /// <returns></returns>
        private GoogleCodeProject GetNewProject(string projectLink)
        {
            Reader = new RssFeedReader();
            GoogleCodeProject project = null;
            XmlDocument doc = null;

            if (!projectLink.StartsWith("http://code.google.com/feeds/p/"))
            {
                // Its a bad link
                ProjectBot.WriteLine("Error in Project Link", Color.Red);
                return null;
            }
            else
            {
                if (Reader.TryRead(projectLink, out doc))
                {
                    project = Reader.ConvertToProject(doc);
                }
                
            }
            return project;
        }
        /// <summary>
        /// Compares 2 Goggle projects returns true if the are the same, false if they are different
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>true if the are the same, false if they are different</returns>
        private bool CompareUpdates(GoogleCodeProject one, GoogleCodeProject two)
        {
                if (one.Updated != two.Updated)
                {
                    return false;
                }
            return true;
        }
    }
}
