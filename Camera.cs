using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace myPlatformer
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        private Viewport _viewport;
        private Vector2 _position;
        private float _lerpFactor = 0.1f;
        private float _zoomLevel = 2.0f;  

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            _position = Vector2.Zero;
        }

        public void Update(Hero target)
        {
            Follow(target);
        }

        private void Follow(Hero target) // camera that follows the player with some effects applied
        {
            _zoomLevel = 2.0f;
            Vector2 targetPosition = new Vector2(target.position.X + 12, target.position.Y + 12);
            _position = Vector2.Lerp(_position, targetPosition, _lerpFactor);

            
            var screenCenter = new Vector2(_viewport.Width / 2, _viewport.Height / 2);

            Transform = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) * 
                        Matrix.CreateScale(_zoomLevel, _zoomLevel, 1) *         
                        Matrix.CreateTranslation(screenCenter.X, screenCenter.Y, 0); 
        }

        public void UpdateMenu(MenuUI target)
        {
            FollowMenu(target);
        }
        private void FollowMenu(MenuUI target) // camera that changes depending on the menus position
        {
            _zoomLevel = 2.5f;
            Vector2 _position = new Vector2(target.position.X + 32, target.position.Y + 40);

            var screenCenter = new Vector2(_viewport.Width / 2, _viewport.Height / 2);

            Transform = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
                        Matrix.CreateScale(_zoomLevel, _zoomLevel, 1) *         
                        Matrix.CreateTranslation(screenCenter.X, screenCenter.Y, 0); 
        }
        public Vector2 ScreenToWorld(Vector2 screenPosition) // allows to accurately calculate position
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
        }
    }
}
