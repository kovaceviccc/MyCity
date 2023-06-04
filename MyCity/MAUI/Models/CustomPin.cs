using Android.Views.Animations;
using Java.Lang;
using Microsoft.Maui.Controls.Maps;
using System.Runtime.CompilerServices;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MAUI.Models;

public abstract class CustomPin : Pin
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CustomPin));
    public string PublisherId { get; set; }
    public DateTime DateCreated { get; set; }
    public new string Id { get; set; }

    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public abstract void Add(Map map);
    public abstract void Remove(Map map);

}
