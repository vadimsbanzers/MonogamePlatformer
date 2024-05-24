using myPlatformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection.Metadata;
using SharpDX.Direct2D1;


namespace myPlatformer;

public class Hero// Player class which controls player movement/animation and collisions with other objects
{
    private const float SPEED = 250f;
    private const float GRAVITY = 3900f;
    private const float JUMP = 1000f;
    private const int OFFSET = 1;

    public static readonly int PLAYER_SIZE = 32;
    public Vector2 position;
    public Vector2 StartPosition;
    private Vector2 _velocity;
    private bool _onGround;
    private bool isMoving = false;
    private bool lastDirectionLeft = false;
    private Animation _currentAnimation;
    private Dictionary<string, Animation> _animations;

    private MenuUI _menuUI;
    private MapReader _mapReader;


    public Hero( Dictionary<string, Animation> animations, Vector2 position, MenuUI menuUI, MapReader mapReader)
    {
        this.position = position;
        _animations = animations;
        _currentAnimation = _animations["Idle"];  
        _velocity = Vector2.Zero;
        _menuUI = menuUI;
        _mapReader = mapReader ;
        StartPosition = position;
    }

    public void SetAnimation(string animationKey, SpriteEffects flipEffect = SpriteEffects.None) //setting the player animaton
    {
        if (_animations.ContainsKey(animationKey))
        {
            if (_currentAnimation != _animations[animationKey])
            {
                _currentAnimation = _animations[animationKey];
            }
            _currentAnimation.SetFlip(flipEffect);
        }
    }


    public Rectangle CalculateBounds(Vector2 pos) //player bounds
    {


        return new Rectangle(
            (int)pos.X + OFFSET,
            (int)pos.Y,
            PLAYER_SIZE - 2,
            PLAYER_SIZE
        );
    }

    private void UpdateVelocity() // player movement update
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.A))
        {
            _velocity.X = -SPEED;
            isMoving = true;
            lastDirectionLeft = true;
        }

        else if (keyboardState.IsKeyDown(Keys.D))
        {
            _velocity.X = SPEED;
            isMoving = true;
            lastDirectionLeft = false;
        }
        else
        {
            _velocity.X = 0;
            isMoving = false;
        }
        foreach (var obj in _mapReader.objects) // calculates collisions with trampolines and applies boost
        {
            if (obj.ID == 50 && CalculateBounds(position).Intersects(new Rectangle((int)obj.Position.X - 8, (int)obj.Position.Y - 8, 16, 16)))
            {
                _velocity.Y = -JUMP * 1.5f;
                break;
            }
        }

        _velocity.Y += GRAVITY * Globals.Time;
        if (_velocity.Y > 1000) _velocity.Y = 1000; // velocity clamp
        if (keyboardState.IsKeyDown(Keys.Space) && _onGround)
        {
            _velocity.Y = -JUMP;
        }
    }

    public void UpdatePosition() // calculating collisions with object bounds
    {
        _onGround = false;
        Vector2 newPos = position + (_velocity * Globals.Time);
        Rectangle newRect = CalculateBounds(newPos);

        MapReader mapReader = MapReader.Instance;

        foreach (var collider in MapReader.GetNearestColliders(newRect, mapReader.IsBluePressed, mapReader.IsRedPressed, mapReader.IsYellowPressed))
        {
            if (newPos.X != position.X)
            {
                newRect = CalculateBounds(new Vector2(newPos.X, position.Y));
                if (newRect.Intersects(collider))//stops horrizontal collision
                {
                    newPos.X = newPos.X > position.X ? collider.Left - PLAYER_SIZE + OFFSET : collider.Right - OFFSET;
                    _velocity.X = 0; 
                    continue;
                }
            }

            newRect = CalculateBounds(new Vector2(position.X, newPos.Y));
            if (newRect.Intersects(collider))
            {
                if (_velocity.Y > 0) // vertical collision
                {
                    newPos.Y = collider.Top - PLAYER_SIZE;
                    _onGround = true;
                    _velocity.Y = 0; 
                }
                else
                {
                    newPos.Y = collider.Bottom;
                    _velocity.Y = 0; 
                }
            }
        }

        position = newPos;
        

    }

    public void Update(GameTime gt) // setting animations and running methods
    {
        if (!_onGround)
        {
            if (lastDirectionLeft) SetAnimation("Jump", SpriteEffects.FlipHorizontally);
            if (!lastDirectionLeft) SetAnimation("Jump", SpriteEffects.None);
        }

        

        if (isMoving && _onGround)
        {
            if(lastDirectionLeft) SetAnimation("Walk", SpriteEffects.FlipHorizontally);
            if(!lastDirectionLeft) SetAnimation("Walk", SpriteEffects.None);

        }
        if (!isMoving && _onGround)
        {
            if (lastDirectionLeft) SetAnimation("Idle", SpriteEffects.FlipHorizontally);
            if (!lastDirectionLeft) SetAnimation("Idle", SpriteEffects.None);
        }
        _currentAnimation.Update(gt);
        UpdateVelocity();
        UpdatePosition();


        CheckObjectCollisions();
        CheckItemCollisions();
    }
    private void CheckItemCollisions() // checks for collisions with items and removes item and increases the score
    {
        Rectangle heroBounds = CalculateBounds(position);

        for (int i = _mapReader.items.Count - 1; i >= 0; i--)
        {
            var item = _mapReader.items[i];
            if (item.IsActive && heroBounds.Intersects(new Rectangle((int)item.Position.X - 8, (int)item.Position.Y - 8, 16, 16)))
            {

                _menuUI.IncreaseScore(100);
                _mapReader.RemoveItems(x => x == item);
            }
        }
    }
    private void CheckObjectCollisions() // checks collisions with object by their IDs
    {
        Rectangle heroBounds = CalculateBounds(position);
        for (int i = _mapReader.objects.Count - 1; i >= 0; i--)
        {
            var objectI = _mapReader.objects[i];
            if (objectI.ID == 50 && heroBounds.Intersects(new Rectangle((int)objectI.Position.X - 8, (int)objectI.Position.Y - 8, 16, 16))) //trampoline
            {

                if (!objectI.IsTrampolineTriggered())
                {

                    objectI.trampolineTriggered();
                }
            }

            else
            {

                if (objectI.ID == 50)
                {
                    objectI.ResetTrampolineTrigger(); // Implement this method in your Objects class
                }
            }
            if (objectI.ID == 53 && heroBounds.Intersects(new Rectangle((int)objectI.Position.X - 8, (int)objectI.Position.Y - 8, 16, 16))) // level finish
            {
                _menuUI.GameOver();
            }
            }
        for (int i = _mapReader.cannonBalls.Count - 1; i >= 0; i--) // checks collision for every cannonball in the list 
        {
            var objectI = _mapReader.cannonBalls[i];
            if (objectI.ID == 52 && heroBounds.Intersects(new Rectangle((int)objectI.Position.X - 7, (int)objectI.Position.Y - 6, 14, 11)))
            {
                
                _menuUI._lives -= 1;
                _mapReader.RemoveObject(x => x == objectI);
            }

            else
            {

            }

        }
    }
        public void Draw()
    {
        _currentAnimation.Draw(Globals.SpriteBatch, position, Color.White);

    }
    public void Reset()
    {
        position = StartPosition;

    }

}