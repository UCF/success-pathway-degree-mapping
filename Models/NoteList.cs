using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class NoteList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NoteType { get ;set; }

        public NoteList()
        { 
        
        }

        public NoteList(Note n)
        {
            Id = n.Id;
            Title = Note.GetNoteTypeValue(n.NoteType);
            Content = n.Value;
            NoteType = n.NoteType;
        }

        public static List<NoteList> List(int degreeId)
        {
            List<Note> list_n = Note.List(degreeId, null);
            List<NoteList> list_nl = new List<NoteList>();
            foreach (Note n in list_n.Where(x=>x.Active))
            {
                NoteList nl = new NoteList(n);
                list_nl.Add(nl);
            }
            return list_nl;
        }
    }
}