using UnityEngine;

namespace CodeBase.Services.Input
{
    public class JoystickMovementInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                var axis = UnityAxis();

                if (axis == Vector2.zero)
                {
                    //TODO joystick Input
                    axis = JoystickAxis();
                }

                return axis;
            }
        }

        public override Vector2 ViewAxis
        {
            get
            {
                Vector2 viewAxis = JoystickViewAxis();
                return viewAxis;
            }
        }

        public override float ScrollAxis
        {
            get
            {
                float scrollAxis = JoystickScrollAxis();
                return scrollAxis;
            }
        }

        public override bool IsClickButtonDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public override bool IsClickButtonPress()
        {
            return UnityEngine.Input.GetMouseButton(0);
        }

        private Vector2 JoystickAxis()
        {
            Vector2 axis;
            axis = Vector2.zero;
            return axis;
        }

        private Vector2 JoystickViewAxis()
        {
            Vector2 axis;
            axis = Vector2.zero;
            return axis;
        }

        private float JoystickScrollAxis()
        {
            return 0;
        }
    }
}