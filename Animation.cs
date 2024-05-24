using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Animation // Game Engine class that allows us to load textures and create animations
{
    public Texture2D _spriteSheet { get; set; }
    private Rectangle[] _frames; // list of frames
    private int _frameIndex;
    private float _frameTime; // Frame duration
    private float _timeSinceLastFrame;
    private SpriteEffects _spriteEffect = SpriteEffects.None; // Flipping
    private bool _playOnce = false; // Play animation once
    private bool _animationComplete = false; // Animation completion

    // Delegate and event ghandler for animation completion
    public delegate void AnimationCompletedHandler();
    public event AnimationCompletedHandler OnAnimationComplete;

    public Rectangle CurrentFrame => _frames[_frameIndex];

    public Animation(Texture2D spriteSheet, int frameCount, int frameWidth, int frameHeight, float frameTime, bool playOnce = false)
    {
        _spriteSheet = spriteSheet;
        _frameTime = frameTime;
        _frames = new Rectangle[frameCount];
        for (int i = 0; i < frameCount; i++) // divide sprite into frames
        {
            _frames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
        }
        _frameIndex = 0;
        _timeSinceLastFrame = 0f;
        _playOnce = playOnce;
    }

    public void Update(GameTime gameTime)
    {
        if (!_animationComplete) // plays the animation
        {
            _timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeSinceLastFrame >= _frameTime)
            {
                _frameIndex++;
                _timeSinceLastFrame = 0f;

                if (_frameIndex >= _frames.Length)
                {
                    _frameIndex = 0; 

                    if (_playOnce)
                    {
                        _animationComplete = true;
                        OnAnimationComplete?.Invoke(); 
                    }
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) // draws the animation
    {
        if (!_animationComplete || !_playOnce)
        {
            spriteBatch.Draw(_spriteSheet, position, CurrentFrame, color, 0f, Vector2.Zero, 1.0f, _spriteEffect, 0f);
        }
    }

    public void SetFlip(SpriteEffects effects) //to flip the animation
    {
        _spriteEffect = effects;
    }


}