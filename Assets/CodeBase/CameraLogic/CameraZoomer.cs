﻿using CodeBase.Services.Camera;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public abstract class CameraZoomer : IZoomService
    {
        protected Camera _camera;
        protected IInputService _inputService;

        public void Construct(Camera camera, IInputService inputService)
        {
            _camera = camera;
            _inputService = inputService;
        }

        public abstract void Zoom(float zoomSpeed, bool inverseScroll, float zoomMinBound, float zoomMaxBound);

        protected void CalculateZoom(float deltaMagnitudeDiff, float speed, float zoomMinBound, float zoomMaxBound)
        {
            _camera.fieldOfView += deltaMagnitudeDiff * speed;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, zoomMinBound, zoomMaxBound);
        }
    }
}