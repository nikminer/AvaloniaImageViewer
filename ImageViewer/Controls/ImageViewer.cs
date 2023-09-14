﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;

namespace BK.Controls
{
    public partial class ImageViewer : Control
    {
        private Point _cursorPoint;

        private IPen _pen;

        private bool _isPointerCaptured = false;


        public static readonly StyledProperty<double> ScaleProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(Scale), 1.0d);
        public double Scale
        {
            get => GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }


        public static readonly StyledProperty<double> MinScaleProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(MinScale), 0.5d);
        public double MinScale
        {
            get => GetValue(MinScaleProperty);
            set => SetValue(MinScaleProperty, value);
        }


        public static readonly StyledProperty<double> MaxScaleProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(MaxScale), 5d);
        public double MaxScale
        {
            get => GetValue(MaxScaleProperty);
            set => SetValue(MaxScaleProperty, value);
        }


        public static readonly DirectProperty<ImageViewer, Bitmap> ImageSourceProperty = AvaloniaProperty.RegisterDirect<ImageViewer, Bitmap>(
                nameof(ImageSource),
                 o => o.ImageSource,
                (o, v) => { o.ImageSource = v; o.FitImage(); }
            );
        private Bitmap image;
        public Bitmap ImageSource
        {
            get => image;
            set => SetAndRaise(ImageSourceProperty, ref image, value);
        }

        public void FitImage()
        {
            ViewportCenterX = 0;
            ViewportCenterY = 0;
            if (ImageSource != null && ImageSource.Size.Height > Bounds.Height)
            {
                Scale = Bounds.Height / ImageSource.Size.Height;
            }
        }


        public static readonly StyledProperty<double> RotationProperty = AvaloniaProperty.Register<ImageViewer, double>(
                nameof(Rotation),
                coerce: (_, val) => val % (Math.PI * 2)
            );
        public double Rotation
        {
            get => GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }


        public static readonly StyledProperty<double> ViewportCenterYProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(ViewportCenterY), 0.0d);
        public double ViewportCenterY
        {
            get => GetValue(ViewportCenterYProperty);
            set => SetValue(ViewportCenterYProperty, value);
        }

        public static readonly StyledProperty<double> ViewportCenterXProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(ViewportCenterX), 0.0d);
        public double ViewportCenterX
        {
            get => GetValue(ViewportCenterXProperty);
            set => SetValue(ViewportCenterXProperty, value);
        }


        public ImageViewer()
        {
            _pen = new Pen(new SolidColorBrush(Colors.Black), lineCap: PenLineCap.Round);
        }


        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            Point previousPoint = _cursorPoint;

            _cursorPoint = e.GetPosition(this);


            if (_isPointerCaptured)
            {
                Point oldWorldPoint = UIPointToWorldPoint(previousPoint, ViewportCenterX, ViewportCenterY, Scale, Rotation);
                Point newWorldPoint = UIPointToWorldPoint(_cursorPoint, ViewportCenterX, ViewportCenterY, Scale, Rotation);

                Vector diff = newWorldPoint - oldWorldPoint;

                bool canX = true;
                bool canY = true;

                if (image != null)
                {
                    canX = (diff.X > 0 || ViewportCenterX - diff.X < image.Size.Width / 2) && (diff.X < 0 || ViewportCenterX - diff.X > -image.Size.Width / 2);

                    canY = (diff.Y > 0 || ViewportCenterY - diff.Y < image.Size.Height / 2) && (diff.Y < 0 || ViewportCenterY - diff.Y > -image.Size.Height / 2);
                }

                if (canX)
                {
                    ViewportCenterX -= diff.X;
                }

                if (canY)
                {
                    ViewportCenterY -= diff.Y;
                }
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Handled = true;
            e.Pointer.Capture(this);
            _isPointerCaptured = true;
            base.OnPointerPressed(e);
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            e.Pointer.Capture(null);
            _isPointerCaptured = false;
            base.OnPointerReleased(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            var oldScale = Scale;

            var newScale = Scale * (1.0d + e.Delta.Y / 12.0d);

            if (newScale < MinScale || newScale > MaxScale)
            {
                return;
            }

            Scale = newScale;

            Point oldWorldPoint = UIPointToWorldPoint(_cursorPoint, ViewportCenterX, ViewportCenterY, oldScale, Rotation);
            Point newWorldPoint = UIPointToWorldPoint(_cursorPoint, ViewportCenterX, ViewportCenterY, Scale, Rotation);

            Vector diff = newWorldPoint - oldWorldPoint;

            ViewportCenterX -= diff.X;
            ViewportCenterY -= diff.Y;
        }

        public override void Render(DrawingContext context)
        {
            var localBounds = new Rect(new Size(this.Bounds.Width, this.Bounds.Height));
            var clip = context.PushClip(this.Bounds);
            context.DrawRectangle(Brushes.Black, _pen, localBounds, 1.0d);

            var halfMax = Math.Max(this.Bounds.Width / 2.0d, this.Bounds.Height / 2.0d) * Math.Sqrt(2.0d);
            var halfMin = Math.Min(this.Bounds.Width / 2.0d, this.Bounds.Height / 2.0d) / 1.3d;
            var halfWidth = this.Bounds.Width / 2.0d;
            var halfHeight = this.Bounds.Height / 2.0d;

            // 0,0 refers to the top-left of the control now. It is not prime time to draw gui stuff because it'll be under the world 

            var translateModifier = context.PushTransform(Matrix.CreateTranslation(new Vector(halfWidth, halfHeight)));

            // now 0,0 refers to the ViewportCenter(X,Y). 
            var rotationMatrix = Matrix.CreateRotation(Rotation);
            var rotationModifier = context.PushTransform(rotationMatrix);

            // everything is rotated but not scaled 

            var scaleModifier = context.PushTransform(Matrix.CreateScale(Scale, Scale));
            var mapPositionModifier = context.PushTransform(Matrix.CreateTranslation(new Vector(-ViewportCenterX, ViewportCenterY)));

            // now everything is rotated and scaled, and at the right position, now we're drawing strictly in world coordinates

            if (ImageSource != null)
            {
                var image = ImageSource;
                var rect = new Rect(image.Size);
                rect = rect.Translate(new Vector(-image.Size.Width / 2, -image.Size.Height / 2));
                context.DrawImage(image, rect);
            }


            // end drawing the world 
            mapPositionModifier.Dispose();
            scaleModifier.Dispose();
            rotationModifier.Dispose();
            translateModifier.Dispose();
            clip.Dispose();

            // oh and draw again when you can, no rush, right? 
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }

        private Point UIPointToWorldPoint(Point inPoint, double viewportCenterX, double viewportCenterY, double scale, double rotation)
        {
            Point workingPoint = new Point(inPoint.X, -inPoint.Y);
            workingPoint += new Vector(-this.Bounds.Width / 2.0d, this.Bounds.Height / 2.0d);
            workingPoint /= scale;

            workingPoint = Matrix.CreateRotation(rotation).Transform(workingPoint);

            workingPoint += new Vector(viewportCenterX, viewportCenterY);

            return workingPoint;
        }
    }
}