using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageViewer.Enums;
using System.Collections.Generic;

namespace ImageViewer.Sample.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private Bitmap currentImage;

    [ObservableProperty]
    private ImageFit selectedImageFit;

    [ObservableProperty]
    private List<ImageFit> imageFits;

    [ObservableProperty]
    private float minScale = 0.3f;

    [ObservableProperty]
    private float maxScale = 4f;

    public MainViewModel() 
    {
        CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://ImageViewer.Sample/Assets/cyber-science-fiction-digital-art-concept-art-cyberpunk-artwork-futuristic-fantasy-art-fan-art-3D-spaceship-PC-gaming-cityscape-futuristic-city-sunset-CGI-1592967.jpg")));
        ImageFits = new List<ImageFit>((IEnumerable<ImageFit>)Enum.GetValues(typeof(ImageFit)));
        SelectedImageFit = ImageFit.WidthCenter;
    }
}
