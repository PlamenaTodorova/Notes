using Instruments;
using Topics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models;

namespace NotesCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Engin
        Engin engin;
        
        public MainWindow()
        {
            InitializeComponent();

            engin = new Engin();

            SetTopics();

            RenderNotes();
        }

        private void RenderNotes()
        {
            notesContainer.ItemsSource = engin.GetNotes();
        }

        private void SetTopics()
        {
            CategoryTree.ItemsSource = engin.GetTopics();
            

        }

        #region New Commands
        private void NewNote_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!engin.AddNote())
            {
                MessageBox.Show("There was a problem creating a note. Please try again later.");
            }
        }

        private void NewTopic_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewTopic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewTopic newTopic = new NewTopic();

            string name;

            if(newTopic.ShowDialog() == true)
            {
                name = newTopic.TopicsName;

                TopicViewModel selected = CategoryTree.SelectedItem as TopicViewModel;

                string selectedPath;

                if (selected == null)
                    selectedPath = "";
                else
                    selectedPath = selected.ParentPath + "\\" + selected.Name;
                
                if (!engin.AddTopic(name, selectedPath))
                {
                    MessageBox.Show("An error ocurred while creating the topic");
                }
                SetTopics();
            }
        }

        #endregion

        private void NoteEdited(object sender, RoutedEventArgs e)
        {
            int noteId = int.Parse(((TextBlock)((StackPanel)
                ((TextBox)sender).Parent).Children[0]).Text);

            if (!engin.ModifyNoteText(noteId))
            {
                MessageBox.Show("A problem occured while saving the note. There might be problem with the note's file");
            }
            
        }

        private void RenameNote(object sender, RoutedEventArgs e)
        {
            int noteId = int.Parse(((TextBlock)((StackPanel)((Border)((Grid)
                ((TextBox)sender).Parent).Parent).Parent).Children[0]).Text);

            if (!engin.RenameNote(noteId))
            {
                MessageBox.Show("A problem occured while saving the note. There might be problem with the note's file");
            }

        }
        
        private void DeleteNote(object sender, RoutedEventArgs e)
        {
            int noteId = int.Parse(((TextBlock)((StackPanel)((Border)((Grid)
                ((Menu)((MenuItem)((MenuItem)sender).Parent).Parent).Parent)
                .Parent).Parent).Children[0]).Text);

            
            if (!engin.DeleteNote(noteId))
            {
                MessageBox.Show("The note couldn't be delete or has already been remove.");
            }

        }

        private void TopicPicked(object sender, RoutedEventArgs e)
        {
            string topic = ((Button)sender).Content.ToString();

            if (!engin.ChangeTopic(topic))
            {
                MessageBox.Show("Topic not found.");
            }

            RenderNotes();
        }
    }


}
