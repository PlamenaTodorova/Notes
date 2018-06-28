using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TopicViewModel
    {
        public TopicViewModel(string name, string path)
        {
            this.Name = name;
            this.ParentPath = path;

            this.SubDirectories = new HashSet<TopicViewModel>();
        }

        public string Name { get; set; }

        public string ParentPath { get; set; }

        public ICollection<TopicViewModel> SubDirectories { get; set; }
    }
}
