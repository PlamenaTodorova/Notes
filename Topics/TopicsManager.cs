using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Topics
{
    public class TopicsManager
    {
        private List<TopicViewModel> directories;
        private const string MainFolder = @"\Notes";
        private readonly string StartingPoint;

        public TopicsManager()
        {
            directories = new List<TopicViewModel>();

            StartingPoint = Directory.GetCurrentDirectory();

            LoadDirectories();
        }

        public List<TopicViewModel> Topics()
        {
            return directories
                .Where(e => e.ParentPath == (StartingPoint))
                .ToList();
        }

        public string GetDirectoryLocation(string title)
        {
            TopicViewModel current = directories
                .FirstOrDefault(e => e.Name == title);

            if (current == null)
                throw new Exception("No topic matches this name");

            return current.ParentPath + "\\" + title;
        }

        #region LoadFolders
        private void LoadDirectories()
        {
            DirectoryInfo startDirectory = new DirectoryInfo(StartingPoint + MainFolder);

            if (startDirectory.Exists)
            {
                WalkDirectoryTree(startDirectory);
            }
            else
            {
                Directory.CreateDirectory(StartingPoint + MainFolder);

                File.Create(StartingPoint + MainFolder + "\\Notes.json");

                TopicViewModel newFolder = new TopicViewModel("Notes", StartingPoint);

                directories.Add(newFolder);
            }
        }

        private TopicViewModel WalkDirectoryTree(DirectoryInfo root)
        {
            TopicViewModel current = new TopicViewModel(root.Name, root.Parent.FullName);
            directories.Add(current);

            DirectoryInfo[] dirs = root.GetDirectories();

            foreach (DirectoryInfo directory in dirs)
            {
                current.SubDirectories.Add(WalkDirectoryTree(directory));
            }

            return current;
        }

        #endregion

        #region CreateFolder
        public void CreateFolder(string Name, string parentPath)
        {
            TopicViewModel parent = GetFolder(parentPath);

            string newFolderName = parentPath + "\\" + Name;
            Directory.CreateDirectory(newFolderName);

            string filePath = newFolderName + "\\" + Name + ".json";

            File.Open(filePath, FileMode.CreateNew);
            Thread.Sleep(1000);

            TopicViewModel newFolder = new TopicViewModel(Name, parentPath);

            directories.Add(newFolder);

            parent.SubDirectories.Add(newFolder);
        }

        public void CreateFolder(string Name)
        {
            this.CreateFolder(Name, StartingPoint + MainFolder);
        }

        private TopicViewModel GetFolder(string path)
        {
            TopicViewModel folder = directories.FirstOrDefault(e => e.ParentPath + "\\" + e.Name == path);
            
            if (folder == null)
            {
                throw new Exception("No directory with this name is loaded");
            }

            return folder;
        }
        #endregion

        #region DeleteFolder
        //TO DO
        #endregion

        #region MoveFolder
        //TO DO
        #endregion
    }
}

