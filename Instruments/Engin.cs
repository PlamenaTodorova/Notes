using Topics;
using System;
using System.Collections.Generic;
using Models;
using Notes;
using System.Collections.ObjectModel;

namespace Instruments
{
    public class Engin
    {
        private TopicsManager loadNotes;
        private NotesGenerator notesGenerator;

        public Engin()
        {
            loadNotes = new TopicsManager();

            notesGenerator = new NotesGenerator(loadNotes.GetDirectoryLocation("Notes"));
        }

        public List<TopicViewModel> GetTopics()
        {
            return loadNotes.Topics();
        }

        public ObservableCollection<NoteViewModel> GetNotes()
        {
            return notesGenerator.GetNotes();
        }

        public bool ChangeTopic(string topic)
        {
            try
            {
                string path = loadNotes.GetDirectoryLocation(topic);

                notesGenerator.ChangeLocation(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTopic(string name, string place)
        {
            try
            {
                if(place == "")
                    loadNotes.CreateFolder(name);
                else
                    loadNotes.CreateFolder(name, place);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNote()
        {
            try
            {
                notesGenerator.AddNote();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModifyNoteText(string title)
        {
            try
            {
                notesGenerator.UpdateText(title);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
