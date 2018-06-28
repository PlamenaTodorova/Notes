using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace Notes
{
    public class NotesGenerator
    {
        private const string NotesInfoFileExtension = ".json";
        private const string NotesFileExtension = ".note";
        private const string StandartTitle = "New note";

        private string location { get; set; }
        private ObservableCollection<NoteViewModel> notes;
        private List<Note> notesInfo;

        public NotesGenerator(string location)
        {
            ChangeLocation(location);
        }

        public void ChangeLocation(string location)
        {
            this.location = location;

            notesInfo = GetNotesList();
            Render();
        }

        #region ReadingAndWritingTheNotes
        private List<Note> GetNotesList()
        {
            List<Note> list = new List<Note>();
            string path = GetNotesFilePath();
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                list = JsonConvert.
                    DeserializeObject<List<Note>>(json);

            }

            if (list == null)
            {
                list = new List<Note>();
            }

            return list;
        }

        private void WriteNoteList(List<Note> notes)
        {
            File.WriteAllText(GetNotesFilePath(), JsonConvert.SerializeObject(notes));
        }
        #endregion

        #region RenderNotes
        public ObservableCollection<NoteViewModel> GetNotes()
        {
            return notes;
        }

        private void Render()
        {
            notes = new ObservableCollection<NoteViewModel>();

            foreach (Note noteInfo in notesInfo)
            {
                notes.Add(new NoteViewModel()
                {
                    Title = noteInfo.Title,
                    NoteText = GetText(noteInfo)
                });
            }
        }

        private string GetText(Note noteInfo)
        {
            string filePath = noteInfo.Path;
            return File.ReadAllText(filePath);
        }

        #endregion

        #region AddNote
        public void AddNote()
        {
            //Find how many notes there are with the title "New note"(Standart title name)
            int count = notesInfo.Where(e => e.Title.StartsWith(StandartTitle)).Count();

            //Create new note
            Note newNote = new Note(count);

            string newNoteLocation = location + "\\" + newNote.Title + NotesFileExtension;
            newNote.Path = newNoteLocation;

            NoteViewModel newNoteModel = new NoteViewModel()
            {
                Title = newNote.Title,
                NoteText = ""
            };

            //Create a file for the note
            File.Create(newNote.Path);

            //Add it to the collections
            notesInfo.Add(newNote);
            notes.Add(newNoteModel);

            //Update the file
            WriteNoteList(notesInfo);
        }
        #endregion

        #region UpdateNotes
        //TO DO

        #region RenemeNote
        //TO DO
        #endregion

        #region ChangeText
        public void UpdateText(string title)
        {
            NoteViewModel currentNote = notes.FirstOrDefault(e => e.Title == title);
            Note currentNoteInfo = notesInfo.FirstOrDefault(e => e.Title == title);

            if (currentNote == null)
                throw new Exception("No such note found");

            File.WriteAllText(currentNoteInfo.Path, currentNote.NoteText);
        }
        #endregion

        #region DeleteNote
        //TO DO
        #endregion

        #region MoveNote
        //TO DO
        #endregion

        #endregion

        private string GetNotesFilePath()
        {
            string name = location.Substring(location.LastIndexOf('\\'));

            return location + name + NotesInfoFileExtension;
        }
    }
}
