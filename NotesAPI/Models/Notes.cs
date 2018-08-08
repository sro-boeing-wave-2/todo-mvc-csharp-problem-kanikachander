using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class Note
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Pinned { get; set; }
        public List<CheckedListItem> CheckedList { get; set; }
        public List<Label> Labels { get; set; }
    }

    public class CheckedListItem
    {
        public int ID { get; set; }
        public string ListItem { get; set; }
    }

    public class Label
    {
        public int ID { get; set; }
        public string LabelName { get; set; }
    }
}