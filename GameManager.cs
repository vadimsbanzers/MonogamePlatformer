using myPlatformer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX.Direct2D1;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Collections.Generic;
using SharpDX.Direct2D1;
namespace myPlatformer;

public class GameManager
{

    private readonly MapReader _map;
    private readonly Hero _hero;
    private Camera _camera;

    private MenuUI _menuUI;
    Game1 _game;
    SpriteFont font;
    Texture2D menuBackground;

    KeyboardState currentKeyboardState;
    KeyboardState previousKeyboardState;


    public enum GameStates
    {
        Menu,
        Playing,
        Paused,
        Death
    }

    private GameStates _gameState;
    public GameManager(Game1 game, MenuUI menuui)
    {
        _menuUI = menuui;
        _game = game;
        font = Globals.Content.Load<SpriteFont>("gameFont");

        Texture2D idleTexture = Globals.Content.Load<Texture2D>("Player\\Idle"); //
        Texture2D walkTexture = Globals.Content.Load<Texture2D>("Player\\Run");  // Loads player animations
        Texture2D jumpTexture = Globals.Content.Load<Texture2D>("Player\\Jump"); //

        var animations = new Dictionary<string, Animation> // creates new animations and adds then to a dictuinary
    {
        { "Idle", new Animation(idleTexture, 11, 32, 32, 0.25f) },  
        { "Walk", new Animation(walkTexture, 12, 32, 32, 0.05f) },  
        { "Jump", new Animation(walkTexture, 1, 32, 32, 0.1f) }   
    };
        _camera = new Camera(Globals.GraphicsDevice.Viewport);


        _menuUI = new(this, font, "gameFont", new Vector2(100, 100), _camera);
        //_gameUI = new(font,_camera,this);
        _map = new();
        _hero = new Hero(animations, _map.spawnPoint, _menuUI, _map);

;
        previousKeyboardState = Keyboard.GetState();
    }
    public void Initialize()
    {
        _gameState = GameStates.Menu;
        menuBackground = new Texture2D(Globals.GraphicsDevice, 1, 1);
        menuBackground.SetData(new Color[] { Color.White });
    }
    public void Update(GameTime gt) // updates controls and methods depending on state
    {
        currentKeyboardState = Keyboard.GetState();


        if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            if (_gameState == GameStates.Playing)
            {
                Paused();
                _menuUI._gameStateMenus = MenuUI.GameStateMenus.PausedMenu;
            }
            else
            {
                if (_gameState == GameStates.Paused)
                {
                    Playing();
                }
            }
        } // changes state to Pause if "esc" is pressed and released
        if (currentKeyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R)) // resets the game if "R" is pressed and released
        {
            if (_gameState == GameStates.Playing)
            {
                Reset();
            }

        }
        previousKeyboardState = currentKeyboardState;


        if (_gameState == GameStates.Playing) // change state to "Playing"
        {
            _map.Update(gt);
            _hero.Update(gt);
            _menuUI.Update(_camera,gt);
            _camera.Update(_hero);
        }
        if( _gameState == GameStates.Paused)// change state to "Paused"
        {
            _camera.UpdateMenu(_menuUI);
            _menuUI.Update(_camera,gt);
        }
        if (_gameState == GameStates.Menu) // change state to "Menu"
        {
            _camera.UpdateMenu(_menuUI);
            _menuUI.Update(_camera, gt);
        }
        if (_gameState == GameStates.Death)// change state to "Death"
        {

            
            
            _camera.UpdateMenu(_menuUI);
            _menuUI._gameStateMenus = MenuUI.GameStateMenus.DeathMenu;
            _menuUI.Update(_camera, gt);
        }





    }

    public void Draw() // draws methods depending on state
    {
        Globals.SpriteBatch.Begin(
            SpriteSortMode.Deferred,      
            BlendState.AlphaBlend,        
            SamplerState.PointClamp,      
            DepthStencilState.Default,    
            RasterizerState.CullNone,     
            null,                         
            _camera.Transform              
        );
        if ((_gameState == GameStates.Menu)|| (_gameState == GameStates.Paused))
        {
            
            _menuUI.Draw();

        }
        if (_gameState == GameStates.Death)
        {
            _menuUI.Draw();
            _menuUI.ScoreDraw(_menuUI.scoreboardDeathGameMenuVector);

            
            

        }
        if ((_gameState == GameStates.Playing))
        {
            
            _map.Draw();
            _hero.Draw();
            _menuUI.Draw();

        }
        Globals.SpriteBatch.End();

        
    }
    // functions to change states
    public void GameOver() // method to change game state to "death" from outside this class
    {
        _gameState = GameStates.Death;
    }
    public void Paused()// method to change game state to "paused" from outside this class
    {
        _gameState = GameStates.Paused;
    }
    public void Playing()// method to change game state to "playing" from outside this class
    {
        _gameState = GameStates.Playing;
    }
    public void Menu()// method to change game state to "menu" from outside this class
    {
        _gameState = GameStates.Menu;
    }
    public void Exit()// // method to exit the game from outside this class
    {
        _game.Exit();
    }

    public void Reset() // runs all reset methods that reset data for a new game
    {
       
        _menuUI.Reset();
        _hero.Reset();
        _map.Reset();
    }
}