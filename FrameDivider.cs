using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FrameDivider // Game engine class that divides sprites into frames that can be individually drawn
{
    private Texture2D texture;
    private int frameWidth;
    private int frameHeight;
    private int frameCount;
    private Rectangle[] frames;

    public FrameDivider(Texture2D texture, int frameCount)
    {
        this.texture = texture;
        this.frameCount = frameCount;
        frameWidth = texture.Width / frameCount;
        frameHeight = texture.Height;

        frames = new Rectangle[frameCount];
        for (int i = 0; i < frameCount; i++)
        {
            frames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
        }
    }


    public void DrawFrame(SpriteBatch spriteBatch, int frameIndex, Vector2 position, Color color) // to draw specific frame
    {
        if (frameIndex >= 0 && frameIndex < frameCount)
        {
            spriteBatch.Draw(texture, position, frames[frameIndex], color);
        }
    }
    public bool IsMouseOverFrame(Vector2 position, Point mousePosition) // check if mouse hovers over the frame
    {
        Rectangle frameRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
        return frameRectangle.Contains(mousePosition);
    }

}