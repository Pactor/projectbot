using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBot.Managers;

namespace ProjectBot.Events
{
    public class ChangeArgs
    {
        public GoogleCodeProject Project { get; set; }
        public ChangeArgs(GoogleCodeProject project)
        {
            this.Project = project;
        }
    }
}
