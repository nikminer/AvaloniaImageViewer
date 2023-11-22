using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;

namespace BK.Controls
{
    public partial class ImageViewer: Control
    {
        
        private void InitSmartphones()
        {
            GestureRecognizers.Add(new PinchGestureRecognizer());
            var scrollRecognizer = new ScrollGestureRecognizer()
            {
                CanHorizontallyScroll = true,
                CanVerticallyScroll = true,
            };
            GestureRecognizers.Add(scrollRecognizer);
            Gestures.PinchEvent.AddClassHandler<ImageViewer>((reciver, e) => reciver.OnPinching(e));
            Gestures.DoubleTappedEvent.AddClassHandler<ImageViewer>((reciver, e) => reciver.FitImage());
            Gestures.ScrollGestureEvent.AddClassHandler<ImageViewer>((reciver, e) => reciver.PullImage(e));
            Gestures.ScrollGestureInertiaStartingEvent.AddClassHandler<ImageViewer>((reciver, e) => _smartphoneScrolling = true);
            Gestures.ScrollGestureEndedEvent.AddClassHandler<ImageViewer>((reciver, e) => _smartphoneScrolling = false);
        }
        
        private bool _smartphoneScrolling = false;

        private void PullImage(ScrollGestureEventArgs e)
        {
            bool canX = true;
            bool canY = true;

            Point workingPoint = new Point(e.Delta.X, e.Delta.Y);
            workingPoint /= Scale;

            if (image != null)
            {
                canX = (workingPoint.X > 0 || ViewportCenterX - workingPoint.X < image.Size.Width / 2) && (workingPoint.X < 0 || ViewportCenterX - workingPoint.X > -image.Size.Width / 2);

                canY = (workingPoint.Y > 0 || ViewportCenterY - workingPoint.Y < image.Size.Height / 2) && (workingPoint.Y < 0 || ViewportCenterY - workingPoint.Y > -image.Size.Height / 2);
            }

            if (canX)
            {
                ViewportCenterX += workingPoint.X;
            }

            if (canY)
            {
                ViewportCenterY -= workingPoint.Y;
            }
        }

        protected void OnPinching(PinchEventArgs e)
        {
            oldScale = Scale;

            var newScale = Scale + (e.Scale - 1) * oldScale / 50;

            if (newScale < MinScale || newScale > MaxScale)
            {
                return;
            }
            Scale = newScale;
            return;
        }
    }
}
