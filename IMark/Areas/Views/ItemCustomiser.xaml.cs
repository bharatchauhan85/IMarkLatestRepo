using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using IMark.Areas.Views.Common;
using Plugin.Media;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;

namespace IMark.Areas.Views
{
    public partial class ItemCustomiser : ContentPage
    {
        SKBitmap bitmap;
        SKMatrix currentMatrix = SKMatrix.CreateIdentity();
        SKMatrix currentMatrixForTxt = SKMatrix.CreateIdentity();
        // Information for translating and scaling
        long? touchId = null;
        SKPoint pressedLocation;
        SKMatrix pressedMatrix;

        // Information for scaling
        bool isScaling;
        SKPoint pivotPoint;
        private SKBitmap txtBitMap;
        private string str;
        private SKBitmap monkeyBitmap;
        private PinchToZoomContainer _currentItem;

        public ItemCustomiser()
        {
            InitializeComponent();
        }

        void Slider_DragStarted(System.Object sender, System.EventArgs e)
        {
            
        }

        async void Slider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if(_currentItem!=null)
                await _currentItem.ScaleTo(e.NewValue, 60);
        }
        //  void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        //  {
        //      SKImageInfo info = args.Info;
        //      SKSurface surface = args.Surface;
        //      SKCanvas canvas = surface.Canvas;
        //      canvas.Clear();
        //      if (bitmap!=null)
        //      {
        //          canvas.SetMatrix(currentMatrix);
        //          canvas.DrawBitmap(bitmap, 0, 0);
        //      }
        //      if(str!=null)
        //      {
        //          // Create an SKPaint object to display the text
        //          SKPaint textPaint = new SKPaint
        //          {
        //              Color = SKColors.Chocolate
        //          };

        //          // Adjust TextSize property so text is 90% of screen width
        //          float textWidth = textPaint.MeasureText(str);
        //          textPaint.TextSize = 0.9f * info.Width * textPaint.TextSize / textWidth;

        //          // Find the text bounds
        //          SKRect textBounds = new SKRect();
        //          textPaint.MeasureText(str, ref textBounds);

        //          // Calculate offsets to center the text on the screen
        //          float xText = info.Width / 2 - textBounds.MidX;
        //          float yText = info.Height / 2 - textBounds.MidY;

        //          // And draw the text
        //          canvas.DrawText(str, xText, yText, textPaint);

        //          //Create a new SKRect object for the frame around the text

        //         //SKRect frameRect = textBounds;
        //         // frameRect.Offset(xText, yText);
        //         // frameRect.Inflate(10, 10);

        //         // // Create an SKPaint object to display the frame
        //         // SKPaint framePaint = new SKPaint
        //         // {
        //         //     Style = SKPaintStyle.Stroke,
        //         //     StrokeWidth = 5,
        //         //     Color = SKColors.Blue
        //         // };

        //         // // Draw one frame
        //         // canvas.DrawRoundRect(frameRect, 20, 20, framePaint);

        //         // // Inflate the frameRect and draw another
        //         // frameRect.Inflate(10, 10);
        //         // framePaint.Color = SKColors.DarkBlue;
        //         // canvas.DrawRoundRect(frameRect, 30, 30, framePaint);
        //      }


        //      if(monkeyBitmap!=null)
        //      {
        //          canvas.SetMatrix(currentMatrix);
        //          canvas.DrawBitmap(monkeyBitmap, 0, 0);
        //      }
        //  }

        //  void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        //  {
        //      // Convert Xamarin.Forms point to pixels
        //      Point pt = args.Location;
        //      SKPoint point =
        //          new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
        //                      (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

        //     if(bitmap!=null)
        //      {
        //          switch (args.Type)
        //          {
        //              case TouchActionType.Pressed:
        //                  // Track only one finger
        //                  if (touchId.HasValue)
        //                      return;

        //                  // Check if the finger is within the boundaries of the bitmap
        //                  SKRect rect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
        //                  rect = currentMatrix.MapRect(rect);
        //                  if (!rect.Contains(point))
        //                      return;

        //                  // First assume there will be no scaling
        //                  isScaling = false;

        //                  // If touch is outside interior ellipse, make this a scaling operation
        //                  if (Math.Pow((point.X - rect.MidX) / (rect.Width / 2), 2) +
        //                      Math.Pow((point.Y - rect.MidY) / (rect.Height / 2), 2) > 1)
        //                  {
        //                      isScaling = true;
        //                      float xPivot = point.X < rect.MidX ? rect.Right : rect.Left;
        //                      float yPivot = point.Y < rect.MidY ? rect.Bottom : rect.Top;
        //                      pivotPoint = new SKPoint(xPivot, yPivot);
        //                  }

        //                  // Common for either pan or scale
        //                  touchId = args.Id;
        //                  pressedLocation = point;
        //                  pressedMatrix = currentMatrix;
        //                  break;

        //              case TouchActionType.Moved:
        //                  if (!touchId.HasValue || args.Id != touchId.Value)
        //                      return;

        //                  SKMatrix matrix = SKMatrix.CreateIdentity();

        //                  // Translating
        //                  if (!isScaling)
        //                  {
        //                      SKPoint delta = point - pressedLocation;
        //                      matrix = SKMatrix.CreateTranslation(delta.X, delta.Y);
        //                  }
        //                  // Scaling
        //                  else
        //                  {
        //                      float scaleX = (point.X - pivotPoint.X) / (pressedLocation.X - pivotPoint.X);
        //                      float scaleY = (point.Y - pivotPoint.Y) / (pressedLocation.Y - pivotPoint.Y);
        //                      matrix = SKMatrix.CreateScale(scaleX, scaleY, pivotPoint.X, pivotPoint.Y);
        //                  }

        //                  // Concatenate the matrices
        //                  SKMatrix.PreConcat(ref matrix, pressedMatrix);
        //                  currentMatrix = matrix;
        //                  canvasView.InvalidateSurface();
        //                  break;

        //              case TouchActionType.Released:
        //              case TouchActionType.Cancelled:
        //                  touchId = null;
        //                  break;
        //          }
        //      }
        //  }

        void Front_Clicked(System.Object sender, System.EventArgs e)
        {
            BackImage.Source = "FrontPage.jpg";
        }

        void Back_Clicked(System.Object sender, System.EventArgs e)
        {
            BackImage.Source = "Back_tShirt.png";
        }

        async void AddImage_Clicked(System.Object sender, System.EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
            });

            if (file == null)
                return;
            // UserDialogs.Instance.Toast(file.Path);
            //bitmap = SKBitmap.Decode(file.GetStream());
            //canvasView.InvalidateSurface();
            var pn = new Views.Common.PinchToZoomContainer();
            pn.Content = new Image { Source = ImageSource.FromStream(()=>file.GetStream()) };
            var tp = new TapGestureRecognizer() { NumberOfTapsRequired=2 };
            tp.Tapped += Tp_Tapped;
            ///pn.Content.GestureRecognizers.Add(tp);
            MyContainerGrid.Children.Add(pn);
            _currentItem = pn;
        }

        private void Tp_Tapped(object sender, EventArgs e)
        {
            _currentItem = sender as PinchToZoomContainer;
            if(MyContainerGrid.Children.Count>1)
            {
                var ct = MyContainerGrid.Children[MyContainerGrid.Children.Count - 1];
                MyContainerGrid.Children[MyContainerGrid.Children.Count - 1] =_currentItem;
                MyContainerGrid.Children[MyContainerGrid.Children.Count - 2] = ct;
            }
        }

        async void AddText_Clicked(System.Object sender, System.EventArgs e)
        {
            var res = await UserDialogs.Instance.PromptAsync("Enter Text");
            str = res.Text;

            var pn = new Views.Common.PinchToZoomContainer();
            pn.VerticalOptions = LayoutOptions.Center;
            pn.HorizontalOptions = LayoutOptions.Center;
            pn.Content = new Label { Text = str ,FontSize=22};
            var tp = new TapGestureRecognizer();
            tp.Tapped += Tp_Tapped;
           // pn.Content.GestureRecognizers.Add(tp);
            _currentItem = pn;
            MyContainerGrid.Children.Add(pn);
            //monkeyBitmap = new SKBitmap();
            //using (SKCanvas canvas = new SKCanvas(monkeyBitmap))
            //{
            //    using (SKPaint paint = new SKPaint())
            //    {
            //        paint.Style = SKPaintStyle.Stroke;
            //        paint.Color = SKColors.Black;
            //        paint.StrokeWidth = 24;
            //        paint.StrokeCap = SKStrokeCap.Round;

            //        using (SKPath path = new SKPath())
            //        {
            //            path.MoveTo(380, 390);
            //            path.CubicTo(560, 390, 560, 280, 500, 280);

            //            path.MoveTo(320, 390);
            //            path.CubicTo(140, 390, 140, 280, 200, 280);

            //            canvas.DrawPath(path, paint);
            //        }
            //    }
            //}

            //canvasView.InvalidateSurface();
        }

        void SaveImg_Clicked(System.Object sender, System.EventArgs e)
        {
        }
      
    }
}
