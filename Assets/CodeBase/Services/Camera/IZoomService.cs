using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;

namespace CodeBase.Services.Camera
{
    public interface IZoomService : IService
    {
        void Zoom(float zoomSpeed,bool inverseScroll,float zoomMinBound,float zoomMaxBound);
    }
}