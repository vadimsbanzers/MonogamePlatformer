using myPlatformer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
namespace myPlatformer;

public class MenuUI // Manages menus, menu states and scoring system  
{
    public LeaderboardManager _leaderboardManager;
    public SpriteFont _font; // 24px font
    public SpriteFont _smallFont; // 12px font
    private Camera _camera;
    private GameManager _gameManager;
    public int _score;
    public int _lives;
    public int _livesSaved;

    public TimeSpan _timer;
    private TimeSpan _totalTime;

    
    public Vector2 scorePosition; //
    public int itemsCollected;    //
    public int livePoints;        //
    public double timeScore;      // Variables to calculate score
    public double totalScore;     //
    public Game1 _game1;          //
    private bool scoreSubmitted;  //

    public Vector2 position;

    private MouseState previousMouseState;

    // Index for FrameDivider;
    private int startGameFrameIndex = 0;
    private int continueGameFrameIndex = 0;
    private int mainMenuGameFrameIndex = 0;
    private int quitGameFrameIndex = 0;
    private int restartGameFrameIndex = 0;
    private int controlsGameFrameIndex = 0;
    private int backGameFrameIndex = 0;
    private int leaderboardGameFrameIndex = 0;
    
    //framedivider for buttons / menu elements
    FrameDivider startGameDivided;
    FrameDivider quitGameDivided;
    FrameDivider continueGameDivided;
    FrameDivider mainMenuGameDivided;
    FrameDivider restartGameDivided;
    FrameDivider controlsGameDivided;
    FrameDivider backGameDivided;
    FrameDivider leaderboardGameDivided;

    Texture2D pauseGameMenu;
    Texture2D controlsGameMenu;
    Texture2D scoreboardDeathMenu;

    //button positions
    Vector2 startButtonVector;
    Vector2 quitButtonVector;
    Vector2 continueButtonVector;
    Vector2 mainMenuButtonVector;
    Vector2 restartButtonVector;
    Vector2 controlsButtonVector;
    Vector2 backButtonVector;
    Vector2 leaderboardButtonVector;

    //bigger menu elements position
    Vector2 pauseGameMenuVector;
    Vector2 controlsGameMenuVector;
    public Vector2 scoreboardDeathGameMenuVector;

    public enum GameStateMenus // menu / game states
    {
        MainMenu,
        PausedMenu,
        DeathMenu,
        Playing,
        Controls,
        leaderboard
    }

    public GameStateMenus _gameStateMenus; // menu states
    public MenuUI(GameManager gameManager,SpriteFont font ,string fontName, Vector2 startPosition, Camera camera)
    {
        _font = font;
        _camera = camera;
        _gameManager = gameManager;
        _score = 0;
        _lives = 5;

        _livesSaved = _lives; // variable for game reset

        _totalTime = TimeSpan.FromMinutes(5); // 5 minute countdown clock
        _timer = _totalTime;
        _gameStateMenus = GameStateMenus.MainMenu;

        _gameManager = gameManager;

        _smallFont = Globals.Content.Load<SpriteFont>("smallFont");                         // Loading font
  
        Texture2D startGame = Globals.Content.Load<Texture2D>("Menu/Buttons/playButton");        //
        Texture2D quitGame = Globals.Content.Load<Texture2D>("Menu/Buttons/quitButton");         //
        Texture2D continueGame = Globals.Content.Load<Texture2D>("Menu/Buttons/continueButton"); //
        Texture2D mainMenuGame = Globals.Content.Load<Texture2D>("Menu/Buttons/mainMenuButton"); //       Loading button textures
        Texture2D restartGame = Globals.Content.Load<Texture2D>("Menu/Buttons/restartButton");   //
        Texture2D controlsGame = Globals.Content.Load<Texture2D>("Menu/Buttons/controlsButton"); //
        Texture2D backGame = Globals.Content.Load<Texture2D>("Menu/Buttons/backButton");         //
        Texture2D leaderboardGame = Globals.Content.Load<Texture2D>("Menu/Buttons/leaderboardButton");//

        controlsGameMenu = Globals.Content.Load<Texture2D>("Menu/controlsMenu");     //
        pauseGameMenu = Globals.Content.Load<Texture2D>("Menu/pauseMenu");           //  Loading textures for large menu elements
        scoreboardDeathMenu = Globals.Content.Load<Texture2D>("Menu/ScoreboardDeath");//

        scoreSubmitted = false;
        //_animation = new Animation(controlsGameMenu, 23, 553, 244, 0.5f, false);

        position = startPosition;

        _leaderboardManager = new LeaderboardManager();

        //dividing buttons/elements into different frames
        startGameDivided = new FrameDivider(startGame, 3);
        quitGameDivided = new FrameDivider(quitGame, 3);
        continueGameDivided = new FrameDivider(continueGame, 3);
        mainMenuGameDivided = new FrameDivider(mainMenuGame, 3);
        restartGameDivided = new FrameDivider(restartGame, 3);
        controlsGameDivided = new FrameDivider(controlsGame, 3);
        backGameDivided = new FrameDivider(backGame, 3);
        leaderboardGameDivided = new FrameDivider(leaderboardGame, 3);

        

    }

    public void Update(Camera camera, GameTime gameTime) // runs functions depending on the game state
    {
        
       
        
        if (_gameStateMenus == GameStateMenus.MainMenu)
        {
            _camera = camera;
            mainMenuUpdate();
        }
        if (_gameStateMenus == GameStateMenus.PausedMenu)
        {
            _camera = camera;
            pauseMenuUpdate();
        }
        if (_gameStateMenus == GameStateMenus.DeathMenu)
        {
            _camera = camera;
            deathScreenUpdate();
            
        }
        if (_gameStateMenus == GameStateMenus.Controls)
        {
            _camera = camera;
            controlsMenuUpdate();

        }
        if (_gameStateMenus == GameStateMenus.Playing)
        {
            playingUpdate(gameTime);
        }
        if (_gameStateMenus == GameStateMenus.leaderboard)
        {
            leaderboardMenuUpdate();
        }

    }
    public void Draw()
    {
        Vector2 currentPos = position;
        if (_gameStateMenus == GameStateMenus.MainMenu)
        {
            mainMenuDraw();
        }
        if (_gameStateMenus == GameStateMenus.PausedMenu)
        {
            pauseMenuDraw();
        }
        if (_gameStateMenus == GameStateMenus.DeathMenu)
        {
            deathScreenDraw();
        }
        if (_gameStateMenus == GameStateMenus.Controls)
        {
           controlsMenuDraw();
        }
        if (_gameStateMenus == GameStateMenus.Playing)
        {
            playingDraw();
        }
        if (_gameStateMenus == GameStateMenus.leaderboard)
        {
            leaderboardMenuDraw();
        }

    }



    public void mainMenuUpdate()
    {
        pauseGameMenuVector = new Vector2(position.X - 32, position.Y - 80);
        startButtonVector = new Vector2(position.X , position.Y);
        controlsButtonVector = new Vector2(position.X -16, position.Y + 50);
        leaderboardButtonVector = new Vector2(position.X -32, position.Y + 100);
        quitButtonVector = new Vector2(position.X , position.Y + 150);
        MouseState currentMouseState = Mouse.GetState();
        Vector2 mouseWorldPosition = _camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));
        
        if (startGameDivided.IsMouseOverFrame(startButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            startGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Playing(); // Change game state to Playing
                _gameManager.Reset();
                _gameStateMenus = GameStateMenus.Playing;
            }
        }
        else
        {
            startGameFrameIndex = 0; // Default frame index
        }
        if (controlsGameDivided.IsMouseOverFrame(controlsButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            controlsGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                _gameStateMenus = GameStateMenus.Controls;
            }
        }
        else
        {
            controlsGameFrameIndex = 0;
        }
        if (leaderboardGameDivided.IsMouseOverFrame(leaderboardButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            leaderboardGameFrameIndex = 2;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                _gameStateMenus = GameStateMenus.leaderboard;
            }
        }
        else
        {
            leaderboardGameFrameIndex = 0;
        }
        if (quitGameDivided.IsMouseOverFrame(quitButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            quitGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Exit(); // Exit the game
            }
        }
        else
        {
            quitGameFrameIndex = 0; 
        }
        previousMouseState = currentMouseState;
    } // Update for main menu
    public void mainMenuDraw()
    {


        
        startGameDivided.DrawFrame(Globals.SpriteBatch, startGameFrameIndex, startButtonVector, Color.White);
        controlsGameDivided.DrawFrame(Globals.SpriteBatch, controlsGameFrameIndex, controlsButtonVector, Color.White);
        leaderboardGameDivided.DrawFrame(Globals.SpriteBatch, leaderboardGameFrameIndex, leaderboardButtonVector, Color.White);
        quitGameDivided.DrawFrame(Globals.SpriteBatch, quitGameFrameIndex, quitButtonVector, Color.White);
    } // Draw for main menu


    public void pauseMenuUpdate()
    {
        pauseGameMenuVector = new Vector2(position.X - 32, position.Y - 80);
        continueButtonVector = new Vector2(position.X - 16, position.Y);
        mainMenuButtonVector = new Vector2(position.X - 16, position.Y + 50);
        quitButtonVector = new Vector2(position.X, position.Y + 100);

        MouseState currentMouseState = Mouse.GetState();
        Vector2 mouseWorldPosition = _camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));
        
        if (continueGameDivided.IsMouseOverFrame(continueButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            continueGameFrameIndex = 2;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Playing(); 
                _gameStateMenus = GameStateMenus.Playing;
            }
        }
        else continueGameFrameIndex = 0;

        if (mainMenuGameDivided.IsMouseOverFrame(mainMenuButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            mainMenuGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Menu();
                _gameStateMenus = GameStateMenus.MainMenu;
            }
        } else mainMenuGameFrameIndex = 0;
        if (quitGameDivided.IsMouseOverFrame(quitButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            quitGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Exit(); // Exit the game
            }
        } else quitGameFrameIndex = 0;

        previousMouseState = currentMouseState;
    } // Pause Screen Update
    public void pauseMenuDraw()
    {
        
        Globals.SpriteBatch.Draw(pauseGameMenu, pauseGameMenuVector, Color.White);
        continueGameDivided.DrawFrame(Globals.SpriteBatch, continueGameFrameIndex, continueButtonVector, Color.White);
        mainMenuGameDivided.DrawFrame(Globals.SpriteBatch, mainMenuGameFrameIndex, mainMenuButtonVector, Color.White);
        quitGameDivided.DrawFrame(Globals.SpriteBatch, quitGameFrameIndex, quitButtonVector, Color.White);
    } // Pause Screen Draw


    public void deathScreenUpdate()
    {
        pauseGameMenuVector = new Vector2(position.X - 192, position.Y - 80);
        restartButtonVector = new Vector2(position.X - 168, position.Y);
        mainMenuButtonVector = new Vector2(position.X - 176, position.Y + 50);
        quitButtonVector = new Vector2(position.X - 160, position.Y + 100);
        scoreboardDeathGameMenuVector = new Vector2(position.X - 50, position.Y - 120);

        MouseState currentMouseState = Mouse.GetState();
        Vector2 mouseWorldPosition = _camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));

        if (restartGameDivided.IsMouseOverFrame(restartButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            restartGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Reset();
                _gameManager.Playing();
                _gameStateMenus = GameStateMenus.Playing;
            }
        }
        else restartGameFrameIndex = 0;

        if (mainMenuGameDivided.IsMouseOverFrame(mainMenuButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            mainMenuGameFrameIndex = 2;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Menu();
                _gameStateMenus = GameStateMenus.MainMenu;
            }
        }
        else mainMenuGameFrameIndex = 0;
        if (quitGameDivided.IsMouseOverFrame(quitButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            quitGameFrameIndex = 2; 
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _gameManager.Exit(); 
            }
        }
        else quitGameFrameIndex = 0;

        previousMouseState = currentMouseState;
    } // Win/Defeat Screen Update
    public void deathScreenDraw() // Win/Defeat Screen Draw
    {


        Globals.SpriteBatch.Draw(pauseGameMenu, pauseGameMenuVector, Color.White);
        Globals.SpriteBatch.Draw(scoreboardDeathMenu, scoreboardDeathGameMenuVector, Color.White);
        restartGameDivided.DrawFrame(Globals.SpriteBatch, restartGameFrameIndex, restartButtonVector, Color.White);
        mainMenuGameDivided.DrawFrame(Globals.SpriteBatch, mainMenuGameFrameIndex, mainMenuButtonVector, Color.White);
        quitGameDivided.DrawFrame(Globals.SpriteBatch, quitGameFrameIndex, quitButtonVector, Color.White);
    }


    public void controlsMenuUpdate()
    {
        controlsGameMenuVector = new Vector2(position.X - 210, position.Y - 140);
        backButtonVector = new Vector2(position.X, position.Y + 100);

        MouseState currentMouseState = Mouse.GetState();
        Vector2 mouseWorldPosition = _camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));

        if (backGameDivided.IsMouseOverFrame(backButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            backGameFrameIndex = 2;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                _gameStateMenus = GameStateMenus.MainMenu;
            }
        }
        else backGameFrameIndex = 0;
        previousMouseState = currentMouseState;
    } // Controls Menu Update
    public void controlsMenuDraw()
    {
        Globals.SpriteBatch.Draw(controlsGameMenu, controlsGameMenuVector, Color.White);
        
        backGameDivided.DrawFrame(Globals.SpriteBatch, backGameFrameIndex, backButtonVector, Color.White);
    }  // Controls Menu Draw

    public void leaderboardMenuUpdate()
    {
        scoreboardDeathGameMenuVector = new Vector2(position.X - 170, position.Y - 120);
        backButtonVector = new Vector2(position.X + 160, position.Y + 150);

        MouseState currentMouseState = Mouse.GetState();
        Vector2 mouseWorldPosition = _camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));

        if (backGameDivided.IsMouseOverFrame(backButtonVector, new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
        {
            backGameFrameIndex = 2; // Hovered frame index
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                _gameStateMenus = GameStateMenus.MainMenu;
            }
        }
        else backGameFrameIndex = 0;
        previousMouseState = currentMouseState;
    } // Leaderboard Menu Update
    public void leaderboardMenuDraw()
    {
        Globals.SpriteBatch.Draw(scoreboardDeathMenu, scoreboardDeathGameMenuVector, Color.White);
        backGameDivided.DrawFrame(Globals.SpriteBatch, backGameFrameIndex, backButtonVector, Color.White);
        DisplayLeaderboard();
    } // Leaderboard Menu Draw

    public void playingUpdate(GameTime gameTime)
    {

        if (_lives <= 0)
        {

                _gameManager.GameOver();

            
        }



        if (_timer.TotalMilliseconds > 0) // timer for player to finish the level
        {
            _timer -= gameTime.ElapsedGameTime;
            if (_timer.TotalMilliseconds < 0)
            {

                _gameManager.GameOver();
            }
        }
    } // Updates Playing State
    public void playingDraw()
    {
        {
            
            
            Vector2 scorePosition = Vector2.Transform(new Vector2(20, 20), Matrix.Invert(_camera.Transform)); // Draw score in the top left corner of the camera view
            //Debug.Write("test");
            Globals.SpriteBatch.DrawString(_font, $"Score: {_score}", scorePosition, Color.White);
            Vector2 livesPosition = Vector2.Transform(new Vector2(20, 100), Matrix.Invert(_camera.Transform));
            //Debug.Write("test");
            Globals.SpriteBatch.DrawString(_font, $"Lives: {_lives}", livesPosition, Color.White);

            string timeText = $"Time: {_timer.Minutes:D2}:{_timer.Seconds:D2}";
            Vector2 timeTextSize = _font.MeasureString(timeText);
            int screenWidth = Globals.GraphicsDevice.Viewport.Width;

            Vector2 timeTextPosition = Vector2.Transform(new Vector2(screenWidth - timeTextSize.X - 200, 20), Matrix.Invert(_camera.Transform));

            Globals.SpriteBatch.DrawString(_font, timeText, timeTextPosition, Color.White);
            //Globals.SpriteBatch.End();
        }
    } // Draws Playing State
    public void Reset()
    {
        _lives = _livesSaved;
        _score = 0;
        _timer = _totalTime;
    } // Resets objects

    public void calculateScore() // calculates the final score
    {
        itemsCollected = _score / 100;
        livePoints = _lives * 100;
        timeScore = Convert.ToInt32(_timer.TotalMilliseconds / 100);
        totalScore = _score + livePoints + timeScore;
        AddPlayerScore("player", Convert.ToInt32(totalScore));

    }
    public void IncreaseScore(int amount) // increses the score
    {
        _score += amount;
    }
    public void ScoreDraw(Vector2 Position) // draws the score at the end of the game
    {
        TimeSpan t2 = new TimeSpan(_timer.Ticks - (_timer.Ticks % 100000));
        if (_lives <= 0)
        {
            Globals.SpriteBatch.DrawString(_font, $"YOU LOST", new Vector2(Position.X + 30, Position.Y + 15), Color.Black);
        }
        else
        {
            Globals.SpriteBatch.DrawString(_font, $"Congratulations", new Vector2(Position.X + 30, Position.Y + 15), Color.Black);
        }
        Globals.SpriteBatch.DrawString(_smallFont, $"Items Collected:{itemsCollected} ({_score} points)", new Vector2(Position.X + 30, Position.Y + 60), Color.Black);
        Globals.SpriteBatch.DrawString(_smallFont, $"Time left: {_timer.Minutes:D2}:{_timer.Seconds:D2}  ({timeScore} points)", new Vector2(Position.X + 30, Position.Y + 90), Color.Black);
        Globals.SpriteBatch.DrawString(_smallFont, $"Lives left: {_lives} ({livePoints} points )", new Vector2(Position.X + 30, Position.Y + 120), Color.Black);
        Globals.SpriteBatch.DrawString(_smallFont, $"Total Points: {Convert.ToInt32(totalScore)}", new Vector2(Position.X + 30, Position.Y + 150), Color.Black);

    }

    public void GameOver() // change state
    {
        if (!scoreSubmitted)
        {
            calculateScore();
            Debug.WriteLine("SCORE SUBMITTED");
            scoreSubmitted = true;
        }
        _gameStateMenus = GameStateMenus.DeathMenu;
        _gameManager.GameOver();
        
    }
    public void DisplayLeaderboard() // Display the leaderboard
    {
        var entries = _leaderboardManager.GetEntries();
        int yPosition = 100; 
        foreach (var entry in entries)
        {
            Globals.SpriteBatch.DrawString(_smallFont, $"{entry.PlayerName} - {entry.Score}", new Vector2(- 50, yPosition - 100), Color.Black);
            yPosition += 30; 
        }
    }

    public void AddPlayerScore(string playerName, int score) // adding player and score to the leaderboard
    {
        var entry = new LeaderboardEntry(playerName, score);
        _leaderboardManager.AddEntry(entry);
    }
}
