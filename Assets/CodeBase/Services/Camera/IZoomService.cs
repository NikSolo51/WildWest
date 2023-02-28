namespace CodeBase.Services.Camera
{
    public interface IZoomService 
    {
        void Zoom(float zoomSpeed,bool inverseScroll,float zoomMinBound,float zoomMaxBound);
    }
}