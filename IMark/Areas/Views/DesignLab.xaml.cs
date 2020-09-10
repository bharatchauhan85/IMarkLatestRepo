using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IMark.Data.Models.Response;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpFormsDemos;
using SkiaSharpFormsDemos.Transforms;
using TouchTracking;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace IMark.Areas.Views
{
    public partial class DesignLab : ContentPage
    {
        SKBitmap BackgroundBitMap;
        List<TouchManipulationBitmap> bitmapCollection =
            new List<TouchManipulationBitmap>();

        Dictionary<long, TouchManipulationBitmap> bitmapDictionary =
            new Dictionary<long, TouchManipulationBitmap>();
        private SKBitmap helloBitmap;
        private SKImage skImage;
        private bool isCapturing;
        public TentacledNode _node;
        public DesignLab(TentacledNode node)
        {
            InitializeComponent();
            BindingContext = App.Locator.DesignLab;
            _node = node;
            addToCart.IsVisible = false;
            // Load in all the available bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            //string[] resourceIDs = assembly.GetManifestResourceNames();
            //SKPoint position = new SKPoint();
            //int count = 0;
            //foreach (string resourceID in resourceIDs)
            //{
            //    count++;
            //    if (resourceID.EndsWith(".png") ||
            //        resourceID.EndsWith(".jpg"))
            //    {
            //        using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            //        {
            //            SKBitmap bitmap = SKBitmap.Decode(stream);
            //            bitmapCollection.Add(new TouchManipulationBitmap(bitmap)
            //            {
            //                Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
            //            });
            //            position.X += 100;
            //            position.Y += 100;
            //        }
            //    }
            //    if (count > 3)
            //        break;
            //}

            using (var strm = assembly.GetManifestResourceStream("IMark.Resources.Back_tShirt.png"))
            {
                BackgroundBitMap = SKBitmap.Decode(strm);
            }
        }




        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    for (int i = bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
                            // Move bitmap to end of collection
                            bitmapCollection.Remove(bitmap);
                            bitmapCollection.Add(bitmap);

                            // Do the touch processing
                            bitmapDictionary.Add(args.Id, bitmap);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            canvasView.InvalidateSurface();
                            break;
                        }
                    }
                    break;

                case TouchActionType.Moved:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        bitmapDictionary.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(BackgroundBitMap, new SKRect(0, 0, args.Info.Width, args.Info.Height), BitmapStretch.AspectFill, BitmapAlignment.Center, BitmapAlignment.Center);
            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
            if (isCapturing)
                skImage = args.Surface.Snapshot();
        }

        async void AddTextBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            var r = await UserDialogs.Instance.PromptAsync(new PromptConfig { });
            var txt = r.Text;
            if (string.IsNullOrEmpty(r.Text))
            {
                return;
            }
            bitmapCollection.Add(GetTextBitMap(txt));
            canvasView.InvalidateSurface();
        }

        void RemoveBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            if (bitmapCollection.Count > 0)
            {
                bitmapCollection.RemoveAt(bitmapCollection.Count - 1);
                canvasView.InvalidateSurface();
            }
        }

        async void SaveBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var status = await CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
                if (status != PermissionStatus.Granted)
                {
                    UserDialogs.Instance.Alert("Must give permission");
                    return;
                }
                isCapturing = true;
                UserDialogs.Instance.ShowLoading();
                canvasView.InvalidateSurface();
                await Task.Delay(4000);
                UserDialogs.Instance.HideLoading();
                isCapturing = false;
                var strm = skImage.Encode(SKEncodedImageFormat.Png, 100).AsStream();
                byte[] data = ReadToEnd(strm);

                if (data == null)
                {
                    UserDialogs.Instance.Alert("Encode returned null");
                }
                else if (data.Length == 0)
                {
                    UserDialogs.Instance.Alert("Encode returned empty array");
                }
                else
                {
                    var fileName= await UserDialogs.Instance.PromptAsync(new PromptConfig {Title="Enter file Name" });
                    if(string.IsNullOrEmpty(fileName.Text))
                    {

                    }
                    bool success = await DependencyService.Get<IPhotoLibrary>().SavePhotoAsync(data, "MyFolder", "DesignLab" + fileName + ".png");
                    if (!success)
                    {
                        UserDialogs.Instance.Alert("SavePhotoAsync return false");
                    }
                    else
                    {
                        addToCart.IsVisible = true;
                        UserDialogs.Instance.Alert("Success!");
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        }


        private TouchManipulationBitmap GetTextBitMap(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new System.Exception("Null or empty string");
            }
            SKBitmap sKBitmap = null;
            using (SKPaint textPaint = new SKPaint { TextSize = 80 })
            {
                SKRect bounds = new SKRect();
                textPaint.MeasureText(text, ref bounds);

                sKBitmap = new SKBitmap((int)bounds.Right,
                                           (int)bounds.Height);

                using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                {
                    bitmapCanvas.Clear();
                    bitmapCanvas.DrawText(text, 0, -bounds.Top, textPaint);
                }
            }
            return new TouchManipulationBitmap(sKBitmap);
        }


        private TouchManipulationBitmap GetImageBitmap(Stream stream)
        {
            SKBitmap bitmap = SKBitmap.Decode(stream);
            var m = new TouchManipulationBitmap(bitmap)
            {
                Matrix = SKMatrix.MakeTranslation(100, 100),
            };
            return m;
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        void EmojiBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            SKBitmap sKBitmap = null;
            var emojiChar = 0x1F680;

            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter(emojiChar);
            using (SKPaint textPaint = new SKPaint { TextSize = 80, Typeface = emojiTypeface })
            {

                // ask the font manager for a font with that character

                SKRect bounds = new SKRect();
                textPaint.MeasureText("😀 😃 😄", ref bounds);

                sKBitmap = new SKBitmap((int)bounds.Right,
                                           (int)bounds.Height);

                using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                {
                    bitmapCanvas.Clear();
                    bitmapCanvas.DrawText("😀 😃 😄", 0, -bounds.Top, textPaint);
                }
            }
            bitmapCollection.Add(new TouchManipulationBitmap(sKBitmap));
            canvasView.InvalidateSurface();

           // PopupNavigation.Instance.PushAsync(new Areas.PopUp.EmojiPicker());
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        async void AddImgBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            using (var strm = await DependencyService.Get<IPhotoLibrary>().PickPhotoAsync())
            {
                if (strm != null)
                {
                    bitmapCollection.Add(GetImageBitmap(strm));
                    canvasView.InvalidateSurface();
                }
            }
        }

        async void BackBtn_Clicked(System.Object sender, System.EventArgs e)
        {
           await App.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await App.Locator.DesignLab.SendImageToServer("https://lp2.hm.com/hmgoepprod?set=quality[79],source[/60/21/602109860c10a4663c82a5dbf0ac8e94bce214e3.jpg],origin[dam],category[kids_babyboy_topstshirts],type[DESCRIPTIVESTILLLIFE],res[m],hmver[2]&call=url[file:/product/fullscreen]", _node);
        }
    }
}
