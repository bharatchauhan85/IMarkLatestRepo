using System;
using System.Linq;
using IMark.Core.Models.Authentication.Response;
using IMark.Core.Models.Enums;
using IMark.Core.Models.Media;
using IMark.Core.Models.Notes;
using Xamarin.Forms;
using AddNotesData = IMark.Core.Models.Notes.AddNotesData;

namespace IMark.Core.Helpers
{
    public static class Misc
    {
        public static bool VerifyHasValue(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return true;
        }

        public static AddNotesData GetNotesDataFromFile(MediaFile file)
        {
            string filename = file.Path.Split('/').Last();
            AddNotesData data = new AddNotesData() { FileStream = file.FileStream, Path = file.Path, PreviewPath = file.PreviewPath, ImageSource = ImageSource.FromStream(()=>file.FileStream), FileName = filename, Type=  NoteType.Image };
            return data;
        }

        public static NoteType GetNoteTypeFromNoteModel(NotesModel noteModel)
        {
            NoteType type = NoteType.Image;
            if (noteModel!=null)
            {
                if(noteModel.notefiles?.Count>=1)
                {
                    if(noteModel.notefiles[0].fileName.Contains("pdf",true))
                    {
                        type = NoteType.Pdf;
                    }
                }
            }
            return type;
        }
    }
}
