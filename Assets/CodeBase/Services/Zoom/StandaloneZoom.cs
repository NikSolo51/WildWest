namespace CodeBase.Services.Zoom
{
    public class StandaloneZoom : CameraZoomer
    {
        public override void Zoom(float zoomSpeed, bool inverseScroll, float zoomMinBound, float zoomMaxBound)
        {
            float scroll = _inputService.ScrollAxis;

            if (inverseScroll)
                scroll = -scroll;
            
            CalculateZoom(scroll, zoomSpeed, zoomMinBound, zoomMaxBound);
        }
    }
}