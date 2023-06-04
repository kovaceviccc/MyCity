using Android.Gms.Maps;
using Microsoft.Maui.Maps.Handlers;
using IMap = Microsoft.Maui.Maps.IMap;

namespace MAUI.Helpers;

class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
    private readonly IMapHandler mapHandler;

    public MapCallbackHandler(IMapHandler mapHandler)
    {
        this.mapHandler = mapHandler;
    }

    public void OnMapReady(GoogleMap googleMap)
    {
        mapHandler.UpdateValue(nameof(IMap.Pins));
    }
}