using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myPlatformer;
using System.Diagnostics;

public class Items // manages Items
{
    public Vector2 Position { get; private set; } // objects position
    public bool IsActive { get; private set; } = true; // check if object is active
    public int ScoreValue { get; private set; } // score value of an object
    private Animation _animation; //object animation


    public Items(Texture2D spriteSheet, int frameCount, int frameWidth, int frameHeight,  Vector2 position, float frameTime, int scoreValue)
    {


        _animation = new Animation(spriteSheet, frameCount, frameWidth, frameHeight, frameTime); // creating a new animation
        Position = position;
        ScoreValue = scoreValue;
    }

    public void Update(GameTime gameTime)
    {
        
        if (IsActive)  // updates object if active
        {
            _animation.Update(gameTime);
        }
    }

    public void Draw()
    {

        if (IsActive)  //draws object if active
        {
            _animation.Draw(Globals.SpriteBatch, Position, Color.White);
        }
    }


}