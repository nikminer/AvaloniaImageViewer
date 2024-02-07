using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace ImageViewer
{
    public partial class ImageViewer: Control
    {
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            Point previousPoint = _cursorPoint;
            _cursorPoint = e.GetCurrentPoint(this).Position;

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
            if (_smartphoneScrolling)
            {
                return;
            }
            e.Handled = true;
            e.Pointer.Capture(this);
            _isPointerCaptured = true;
            base.OnPointerPressed(e);
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (_smartphoneScrolling)
            {
                return;
            }
            e.Pointer.Capture(null);
            _isPointerCaptured = false;
            base.OnPointerReleased(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            oldScale = Scale;

            var newScale = Scale * (1.0d + e.Delta.Y / 12.0d);

            if (newScale < MinScale || newScale > MaxScale)
            {
                return;
            }

            Scale = newScale;
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
