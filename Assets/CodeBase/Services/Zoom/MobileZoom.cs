using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class MobileZoom : CameraZoomer
    {
        public override void Zoom(float zoomSpeed, bool inverseScroll, float zoomMinBound, float zoomMaxBound)
        {
            if (Input.touchCount == 2)
            {
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);


                float deltaDistance = oldTouchDistance - currentTouchDistance;
                CalculateZoom(deltaDistance, zoomSpeed, zoomMinBound, zoomMaxBound);
            }
        }
    }
}