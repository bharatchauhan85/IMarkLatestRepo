// using System;
// using System.IO;
using System.Threading.Tasks;

using Android.Content;
using Android.Media;
using Android.OS;
using Java.IO;

using Xamarin.Forms;

using SkiaSharpFormsDemos.Droid;
using Plugin.Permissions;
using System;
using Plugin.Permissions.Abstractions;
using System.IO;
using IMark.Droid;
using Acr.UserDialogs;

[assembly: Dependency(typeof(PhotoLibrary))]

namespace SkiaSharpFormsDemos.Droid
{
    public class PhotoLibrary : IPhotoLibrary
    {
        public Task<System.IO.Stream> PickPhotoAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<System.IO.Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }

        // Saving photos requires android.permission.WRITE_EXTERNAL_STORAGE in AndroidManifest.xml

        public async Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            try
            {
                Java.IO.File picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                Java.IO.File folderDirectory = picturesDirectory;

                if (!string.IsNullOrEmpty(folder))
                {
                    folderDirectory = new Java.IO.File(picturesDirectory, folder);
                    folderDirectory.Mkdirs();
                }

                using (Java.IO.File bitmapFile = new Java.IO.File(folderDirectory, filename))
                {
                    bitmapFile.CreateNewFile();

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(data);
                    }

                    // Make sure it shows up in the Photos gallery promptly.
                    MediaScannerConnection.ScanFile(MainActivity.Instance,
                                                    new string[] { bitmapFile.Path },
                                                    new string[] { "image/png", "image/jpeg" }, null);
                    UserDialogs.Instance.Toast("File save in :" + bitmapFile.Path);
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }
        public async Task<ImageSource> SavePhotoAsyncWithImageSource(byte[] data, string folder, string filename)
        {
            try
            {
                Java.IO.File picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                Java.IO.File folderDirectory = picturesDirectory;

                if (!string.IsNullOrEmpty(folder))
                {
                    folderDirectory = new Java.IO.File(picturesDirectory, folder);
                    folderDirectory.Mkdirs();
                }

                using (Java.IO.File bitmapFile = new Java.IO.File(folderDirectory, filename))
                {
                    bitmapFile.CreateNewFile();

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(data);
                    }

                    // Make sure it shows up in the Photos gallery promptly.
                    MediaScannerConnection.ScanFile(MainActivity.Instance,
                                                    new string[] { bitmapFile.Path },
                                                    new string[] { "image/png", "image/jpeg" }, null);
                    UserDialogs.Instance.Toast("File save in :" + bitmapFile.Path);
                    var file = System.IO.File.OpenRead(bitmapFile.Path);
                    //var mStr = new MemoryStream();
                    //file.CopyTo(mStr);
                    return ImageSource.FromStream(() => file);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> SavePhotoAsync1(byte[] data)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //location denied
                }

                var filename = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).ToString(), "NewFolder");
                Directory.CreateDirectory(filename);
                filename = System.IO.Path.Combine(filename, "filename.png");
                using (var fileOutputStream = new FileOutputStream(filename))
                {
                    await fileOutputStream.WriteAsync(data);
                }
                //File picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                //File folderDirectory = picturesDirectory;

                //if (!string.IsNullOrEmpty(folder))
                //{
                //    folderDirectory = new File(picturesDirectory, folder);
                //    folderDirectory.Mkdirs();
                //}

                //using (File bitmapFile = new File(folderDirectory, filename))
                //{
                //    bitmapFile.CreateNewFile();

                //    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                //    {
                //        await outputStream.WriteAsync(data);
                //    }

                //    // Make sure it shows up in the Photos gallery promptly.
                //    MediaScannerConnection.ScanFile(MainActivity.Instance,
                //                                    new string[] { bitmapFile.Path },
                //                                    new string[] { "image/png", "image/jpeg" }, null);
                //}
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}



