using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes
{
    public class Note
    {
        public Note(int id, int number)
        {
            this.Id = id;

            this.Title = "New note";

            if (number != 0)
            {
                this.Title += "(" + number + ")";
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }
    }
}
