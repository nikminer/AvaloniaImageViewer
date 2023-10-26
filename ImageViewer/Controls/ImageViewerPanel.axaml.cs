using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace BK.Controls
{
    public partial class ImageViewerPanel : UserControl
    {
        public static readonly StyledProperty<double> MinScaleProperty = AvaloniaProperty.Register<ImageViewerPanel, double>(nameof(MinScale), 0.5d, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
        public double MinScale
        {
            get => GetValue(MinScaleProperty);
            set => SetValue(MinScaleProperty, value);
        }

        public static readonly StyledProperty<double> ScaleProperty = AvaloniaProperty.Register<ImageViewer, double>(nameof(Scale), 1.0d, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
        public double Scale
        {
            get => GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }


        public static readonly StyledProperty<double> MaxScaleProperty = AvaloniaProperty.Register<ImageViewerPanel, double>(nameof(MaxScale), 5d, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
        public double MaxScale
        {
            get => GetValue(MaxScaleProperty);
            set => SetValue(MaxScaleProperty, value);
        }

        public static readonly StyledProperty<Bitmap?> ImageSourceProperty = AvaloniaProperty.Register<ImageViewerPanel, Bitmap?>(nameof(ImageSource), null, defaultBindingMode:Avalonia.Data.BindingMode.TwoWay);
        public Bitmap? ImageSource
        {
            get => GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
          
        public ImageViewerPanel()
        {
            InitializeComponent();
        }
    }
}