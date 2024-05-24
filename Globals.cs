using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace myPlatformer
{
    public static class Globals // Class for accessing frequently used properties
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        private static GraphicsDeviceManager _graphicsDeviceManager;
        public static GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return _graphicsDeviceManager; }
            set { _graphicsDeviceManager = value; }
        }
        public static Point WindowSize { get; set; }

        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}


// Source: https://www.youtube.com/watch?v=vdCqKIEalvs&t=101s