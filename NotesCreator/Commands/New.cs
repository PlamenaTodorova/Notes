using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesCreator.Commands
{
    public class New
    {
        public static readonly RoutedUICommand NewNote = new RoutedUICommand
            (
                "NewNote",
                "NewNote",
                typeof(New),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewTopic = new RoutedUICommand
            (
                "NewTopic",
                "NewTopic",
                typeof(New),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.T, ModifierKeys.Control)
                }
            );

    }
}
