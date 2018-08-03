using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class Notes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Pinned { get; set; }
        public List<CheckedList> CheckedList { get; set; }
        public List<Labels> Label { get; set; }
    }

    public class CheckedList
    {
        public int ID { get; set; }
        //public int CheckedListKey { get; set; }
        public string ListItem { get; set; }

        //public Notes notes { get; set; }
    }

    public class Labels
    {
        public int ID { get; set; }
        //public int LabelKey { get; set; }
        public string Label { get; set; }

        //public Notes notes { get; set; }
    }
}
