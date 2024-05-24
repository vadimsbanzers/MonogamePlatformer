using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using myPlatformer;
using SharpDX.MediaFoundation;
using System;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class Objects // manages objects
{
    public Vector2 Position { get; set; }
    public bool IsActive { get; private set; } = true;
    public int ScoreValue { get; private set; }
    public int ID { get; private set; }
    private Animation _animation;
    private  Texture2D SpriteSheet;
    public MapReader _map;
    private bool _trampolineTriggered = false;
    public float FrameTime { get; private set; }
    public int FrameCount { get; private set; }
    public int FrameWidth { get; private set; }
    public int FrameHeight { get; private set; }
    public bool PlayOnce { get; private set; }
    public bool animationFinished;
    public Vector2 Velocity { get; set; }
    private TimeSpan _timer;
    private TimeSpan _totalTime;

    private static int SPRITE_SIZE;

    public delegate void AnimationCompleteDelegate(Vector2 position);
    public event AnimationCompleteDelegate OnAnimationComplete;



    public Objects(Texture2D spriteSheet, int frameCount, Vector2 position,int frameWidth, int frameHeight, float frameTime, int scoreValue, int id,bool playOnce, MapReader map)
    {
        _map = map;
        SpriteSheet = spriteSheet; 
        FrameCount = frameCount; 
        FrameTime = frameTime;
        FrameWidth = frameWidth;
        FrameHeight = frameHeight;

        SPRITE_SIZE = 16;
        
        Position = position;
        ScoreValue = scoreValue;
        ID = id;

        PlayOnce = playOnce;
        _animation = new Animation(SpriteSheet, FrameCount, frameWidth, frameHeight, FrameTime, PlayOnce);

        _totalTime = TimeSpan.FromSeconds(3.5); 
        _timer = _totalTime;

        

    }

    public void Update(GameTime gameTime)
    {

        switch (ID)
        {

            case 51: //cannon

                if (_timer.TotalMilliseconds > 0) // timer for cannon attack speed
                {
                    _timer -= gameTime.ElapsedGameTime;
                    if (_timer.TotalMilliseconds <= 0)
                    {
                        _animation = new Animation(SpriteSheet, 23, SPRITE_SIZE, SPRITE_SIZE, 0.1f, true);
                        _animation.OnAnimationComplete += _animationCannon_OnAnimationComplete;
                        _timer = TimeSpan.FromSeconds(3.5); 
                        animationFinished = true;
                    }

                }
                break;
            case 50: // trampoline

                _animation.OnAnimationComplete += _animationTrampoline_OnAnimationComplete;

                break;
            case 52: // cannonball

                
                Velocity = new Vector2(-150, 0);
                


                break;

        }
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _animation.Update(gameTime);
        
        
    }

    public Rectangle CannonballCalculateBounds(Vector2 pos) // cannonball bounds
    {

        return new Rectangle(
            (int)pos.X + 2,
            (int)pos.Y,
            14 - 2,
            11
        );
    }

        public void Deactivate()
    {
        IsActive = false;
        Debug.WriteLine("Item deactivated");
    }

    public void trampolineTriggered() // plays animation after tramploline is triggered
    {
        Debug.WriteLine("TRAMPOLINE TRIGGERED");
        _animation = new Animation(SpriteSheet, 13, SPRITE_SIZE, SPRITE_SIZE, 0.0f, true);
        
        _trampolineTriggered = true;
    }
    private void _animationCannon_OnAnimationComplete() // creates a cannonball after the animation is finished
    {
        _animation = new Animation(SpriteSheet, 1, SPRITE_SIZE, SPRITE_SIZE, 0.1f, false);
        OnAnimationComplete?.Invoke(this.Position);
        _map.CreateCannonball(Position);
    }
    private void _animationTrampoline_OnAnimationComplete()
    {
        _animation = new Animation(SpriteSheet, 1, SPRITE_SIZE, SPRITE_SIZE, 0.1f, false);
    }

    public bool IsTrampolineTriggered()
    {
        _animationTrampoline_OnAnimationComplete();
        return _trampolineTriggered;
        
    }
    public void ResetTrampolineTrigger()
    {
        
        _trampolineTriggered = false;
    }
    public void Draw()
    {
        
        _animation.Draw(Globals.SpriteBatch, Position, Color.White);
        
            

    }
    public void Reset()
    {
        _timer = _totalTime;
    }

}