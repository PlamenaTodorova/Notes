using Topics;
using System;
using Notes;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TopicsManager directory = new TopicsManager();

            NotesGenerator notes = new NotesGenerator(directory.GetDirectoryLocation("Notes"));
            notes.ChangeLocation(directory.GetDirectoryLocation("Main"));

            notes.AddNote();
            Console.Read();
        }
    }
}
