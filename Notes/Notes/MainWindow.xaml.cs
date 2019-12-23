using Notes.DataAccess;
using Notes.Domain;
using System;
using System.Collections.Generic;
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

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Note> notes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            using (var context = new NotesContext())
            {
                notes = context.Notes.Where(x => x.DeletedDate == null).ToList();
            }
            notesDG.ItemsSource = notes;
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            var selectedNote = notesDG.SelectedItem as Note;
            notes.Remove(selectedNote);
            Task.Run(() => DeleteNote(selectedNote));
            notesDG.ItemsSource = null;
            notesDG.ItemsSource = notes;
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Save());
        }

        private Task DeleteNote(Note note)
        {
            using (var context = new NotesContext())
            {
                var noteDb = context.Notes.FirstOrDefault(x => x.Id == note.Id);
                noteDb.DeletedDate = DateTime.Now;
                context.Update(noteDb);
                context.SaveChanges();
            }
            return Task.CompletedTask;
        }

        private Task Save()
        {
            using (var context = new NotesContext())
            {
                foreach (var note in notes)
                {
                    var noteDb = context.Notes.FirstOrDefault(x => x.Id == note.Id && x.DeletedDate == null);
                    if (noteDb is null)
                    {
                        context.Add(note);
                    }
                    else
                    {
                        noteDb.Text = note.Text;
                        noteDb.IsCompleted = note.IsCompleted;
                        context.Update(noteDb);
                    }
                }
                context.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}
