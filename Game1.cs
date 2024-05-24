using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace myPlatformer
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameManager _gameManager;
        private MenuUI _menuui;
        private Color backgroundColor;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Globals.GraphicsDeviceManager = _graphics;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.WindowSize = new(MapReader.tiles.GetLength(1) * MapReader.TILE_SIZE, MapReader.tiles.GetLength(0) * MapReader.TILE_SIZE);
            _graphics.PreferredBackBufferWidth = 1280;  
            _graphics.PreferredBackBufferHeight = 900;

            _graphics.ApplyChanges();
            backgroundColor = new Color(33, 31, 48);
            Globals.Content = Content;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.GraphicsDevice = GraphicsDevice;

            _gameManager = new(this,_menuui);
            _gameManager.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {


            Globals.Update(gameTime);
            _gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _gameManager.Draw();
            
            base.Draw(gameTime);
        }
    }
}
