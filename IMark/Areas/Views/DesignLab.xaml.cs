using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IMark.Areas.PopUp;
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
        private SKImage skImage;
        private bool isCapturing;
        public TentacledNode _node;
        private int selectedSide = 0;
        private ImageSource _frontSaveImage;

        public ImageSource FrontSaveImage
        {
            get { return _frontSaveImage; }
            set { _frontSaveImage = value;OnPropertyChanged(); }
        }

        private ImageSource _backImageSource;

        public ImageSource BackImageSource
        {
            get { return _backImageSource; }
            set { _backImageSource = value; OnPropertyChanged(); }
        }

        public DesignLab(TentacledNode node,Uri uri=null)
        {
            InitializeComponent();
            BindingContext = this;
            MySlider.Minimum = 40d;
            BindingContext = App.Locator.DesignLab;
            _node = node;
            addToCart.IsVisible = false;
            // Load in all the available bitmaps
            SetBackground(uri);
            AddEmojiMessage();
            AddTextMessage();
        }

        private void  SetBackground(Uri uri)
        {
            if (uri != null)
            {
                var url = "https://assets.ajio.com/medias/sys_master/root/ha7/he7/15216784474142/-288Wx360H-461085141-blue-MODEL.jpg";
                using (HttpClient client = new HttpClient())
                {
                    BackgroundBitMap = SKBitmap.Decode((client.GetAsync(url)).Result.Content.ReadAsStreamAsync().Result);
                }
                return;
            }
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (var strm = assembly.GetManifestResourceStream("IMark.Resources.Back_tShirt.png"))
            {
                BackgroundBitMap = SKBitmap.Decode(strm);
            }
        }


        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            try
            {
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
                if (bitmapCollection.Count == 0)
                    return;
                if (string.IsNullOrEmpty(bitmapCollection[bitmapCollection.Count - 1].Text.Text))
                {
                    TextSizeSlider.IsVisible = false;
                }
                else
                {
                    TextSizeSlider.IsVisible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;

            canvas.Clear();
            using(SKPaint paint = new SKPaint())
            {
                canvas.DrawBitmap(BackgroundBitMap, new SKRect(0, 0, args.Info.Width, args.Info.Height), BitmapStretch.Uniform, BitmapAlignment.Center, BitmapAlignment.Center);
            }
            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
            if (isCapturing)
                skImage = args.Surface.Snapshot();
        }

        async  void AddTextBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new TextPicker());
            //var txt = r.Text;
            //if (string.IsNullOrEmpty(r.Text))
            //{
            //    return;
            //}
            //bitmapCollection.Add(GetTextBitMap(txt));
            //canvasView.InvalidateSurface();
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
                if(selectedSide==0)
                {
                    MyFrontImage.Source = ImageSource.FromStream(() => strm);
                }
                else
                {
                    MyBackImage.Source = ImageSource.FromStream(() => strm);
                }
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
                    var success = await DependencyService.Get<IPhotoLibrary>().SavePhotoAsyncWithImageSource(data, "MyFolder", "DesignLab" + fileName + ".png");
                    if (success==null)
                    {
                        UserDialogs.Instance.Alert("SavePhotoAsync return false");
                    }
                    else
                    {
                        addToCart.IsVisible = true;
                        FrontSaveImage = success;
                        UserDialogs.Instance.Alert("Success!");
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        }

        private float _currentTextSize=80;
        private TouchManipulationBitmap GetTextBitMap(string text,bool isEmoji=false)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new System.Exception("Null or empty string");
            }
            SKBitmap sKBitmap = null;
            var emojiChar = 0x1F680;

            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter(emojiChar);
            using (SKPaint textPaint = new SKPaint { TextSize = _currentTextSize })
            {
                SKRect bounds = new SKRect();
                textPaint.MeasureText(text, ref bounds);
                if(isEmoji)
                {
                    textPaint.Typeface = emojiTypeface;
                }
                sKBitmap = new SKBitmap((int)bounds.Right,
                                           (int)bounds.Height);
                using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                {
                    bitmapCanvas.Clear();
                    bitmapCanvas.DrawText(text, 0, -bounds.Top, textPaint);
                }
            }
            var bt = new TouchManipulationBitmap(sKBitmap);
            bt.Text = new TextInfo { Text = text };
            bt.IsEmoji = isEmoji;
            return bt;
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

        private void AddEmojiMessage()
        {
            MessagingCenter.Subscribe<EmojiPicker, string>(this, "Emoji", (a, b) => {
                SKBitmap sKBitmap = null;
                var emojiChar = 0x1F680;

                var fontManager = SKFontManager.Default;
                var emojiTypeface = fontManager.MatchCharacter(emojiChar);
                using (SKPaint textPaint = new SKPaint { TextSize = _currentTextSize, Typeface = emojiTypeface })
                {

                    // ask the font manager for a font with that character

                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(b, ref bounds);

                    sKBitmap = new SKBitmap((int)bounds.Right,
                                               (int)bounds.Height);

                    using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(b, 0, -bounds.Top, textPaint);
                    }
                }
                var bitmap = new TouchManipulationBitmap(sKBitmap);
                bitmap.Text = new TextInfo { Text = b };
                bitmap.IsEmoji = true;
                bitmapCollection.Add(bitmap);
                canvasView.InvalidateSurface();
            });
        }
         
        private void AddTextMessage()
        {
            MessagingCenter.Subscribe<TextPicker, TextInfo>(this, "TextAdd", (a, b) => {
                SKBitmap sKBitmap = null;
                
                using (SKPaint textPaint = new SKPaint { TextSize = _currentTextSize,Color=b.TextColor.ToSKColor(),Typeface=b.Typeface })
                {

                    // ask the font manager for a font with that character

                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(b.Text, ref bounds);

                    sKBitmap = new SKBitmap((int)bounds.Right,
                                               (int)bounds.Height);

                    using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(b.Text, 0, -bounds.Top, textPaint);
                    }
                }
                var bitmap = new TouchManipulationBitmap(sKBitmap);
                bitmap.Text = b;
                bitmapCollection.Add(bitmap);
                canvasView.InvalidateSurface();
            });
        }

        void EmojiBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Areas.PopUp.EmojiPicker());
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

        private void TextSize_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                _currentTextSize = (float)e.NewValue;
                // bitmapCollection[bitmapCollection.Count - 1] = GetTextBitMap("Test");
                bitmapCollection[bitmapCollection.Count - 1]=ChangeTextSize(bitmapCollection[bitmapCollection.Count - 1]);
                canvasView.InvalidateSurface();
            }
            catch (Exception ex)
            {

            }
        }

        private TouchManipulationBitmap ChangeTextSize(TouchManipulationBitmap touchManipulationBitmap)
        {
            SKBitmap sKBitmap = touchManipulationBitmap.GetBitmap(); 
           
            if (touchManipulationBitmap.IsEmoji)
            {
                var emojiChar = 0x1F680;
                var fontManager = SKFontManager.Default;
                var emojiTypeface = fontManager.MatchCharacter(emojiChar);
                using (SKPaint textPaint = new SKPaint
                {
                    TextSize = _currentTextSize,
                    Typeface = emojiTypeface
                })
                {

                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(touchManipulationBitmap.Text.Text, ref bounds);
                    sKBitmap = sKBitmap.Resize(new SKImageInfo((int)bounds.Right, (int)bounds.Height), SKFilterQuality.High);
                    using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(touchManipulationBitmap.Text.Text, 0, -bounds.Top, textPaint);
                    }
                    touchManipulationBitmap.SetbitMap(sKBitmap);
                    return touchManipulationBitmap;
                }
            }
            else
            {
                using (SKPaint textPaint = new SKPaint
                {
                    TextSize = _currentTextSize,
                    Color = touchManipulationBitmap.Text.TextColor.ToSKColor(),
                    Typeface = touchManipulationBitmap.Text.Typeface
                })
                {

                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(touchManipulationBitmap.Text.Text, ref bounds);
                    sKBitmap = sKBitmap.Resize(new SKImageInfo((int)bounds.Right, (int)bounds.Height), SKFilterQuality.High);
                    using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(touchManipulationBitmap.Text.Text, 0, -bounds.Top, textPaint);
                    }
                    touchManipulationBitmap.SetbitMap(sKBitmap);
                    return touchManipulationBitmap;
                }
            }
           
        }

        private void Front_Clicked(object sender, EventArgs e)
        {
            selectedSide = 0;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (var strm = assembly.GetManifestResourceStream("IMark.Resources.back_tshir.png"))
            {
                BackgroundBitMap = SKBitmap.Decode(strm);
            }
            canvasView.InvalidateSurface();
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            selectedSide = 1;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (var strm = assembly.GetManifestResourceStream("IMark.Resources.Back_tShirt.png"))
            {
                BackgroundBitMap = SKBitmap.Decode(strm);
            }
            canvasView.InvalidateSurface();
        }
    }
}
