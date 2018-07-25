using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;

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
            string path = GetNotesCollectionPath();
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

        private void WriteNoteList()
        {
            File.WriteAllText(GetNotesCollectionPath(), JsonConvert.SerializeObject(notesInfo));
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
            int countOfStandarNamedNotes = notesInfo.Where(e => e.Title.StartsWith(StandartTitle)).Count();
            int count = notesInfo.Count();

            //Create new note
            Note newNote = new Note(count, countOfStandarNamedNotes);

            string newNoteLocation = location + "\\" + newNote.Title + NotesFileExtension;
            newNote.Path = newNoteLocation;

            NoteViewModel newNoteModel = new NoteViewModel()
            {
                Id = newNote.Id,
                Title = newNote.Title,
                NoteText = ""
            };

            //Create a file for the note
            File.Create(newNote.Path);

            //Add it to the collections
            notesInfo.Add(newNote);
            notes.Add(newNoteModel);

            //Update the file
            WriteNoteList();
        }
        #endregion

        #region UpdateNotes
        //TO DO

        #region RenemeNote
        public void RenameNote(int id)
        {
            NoteViewModel currentNote = notes.FirstOrDefault(e => e.Id == id);
            Note currentNoteInfo = notesInfo.FirstOrDefault(e => e.Id == id);

            if (currentNote == null)
                throw new Exception("No such note found");

            string newName = GetNoteFilePath(currentNote);
            
            File.Move(currentNoteInfo.Path, newName);
            Thread.Sleep(1000);

            File.Delete(currentNoteInfo.Path);
            currentNoteInfo.Path = newName;
            currentNoteInfo.Title = currentNote.Title;
            WriteNoteList();
        }
        #endregion

        #region ChangeText
        public void UpdateText(int id)
        {
            NoteViewModel currentNote = notes.FirstOrDefault(e => e.Id == id);
            Note currentNoteInfo = notesInfo.FirstOrDefault(e => e.Id == id);
            
            if (currentNote == null)
                throw new Exception("No such note found");

            File.WriteAllText(currentNoteInfo.Path, currentNote.NoteText);
        }
        #endregion

        #region DeleteNote
        public void RemoveNote(int id)
        {
            NoteViewModel currentNote = notes.FirstOrDefault(e => e.Id == id);
            Note currentNoteInfo = notesInfo.FirstOrDefault(e => e.Id == id);

            if (currentNote == null)
                throw new Exception("No such note found");

            File.Delete(currentNoteInfo.Path);

            notes.Remove(currentNote);
            notesInfo.Remove(currentNoteInfo);

            //Safe things after work is done
            WriteNoteList();
        }
        #endregion

        #region MoveNote
        //TO DO
        #endregion

        #endregion

        private string GetNotesCollectionPath()
        {
            string name = location.Substring(location.LastIndexOf('\\'));

            return location + name + NotesInfoFileExtension;
        }

        private string GetNoteFilePath(NoteViewModel note)
        {
            return location + '\\' + note.Title + NotesFileExtension;
        }
    }
}
