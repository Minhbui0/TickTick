using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static System.Formats.Asn1.AsnWriter;

class SpeedBoost : SpriteGameObject
{
    Level level;
    protected float bounce;
    Vector2 startPosition;
    const float SPEED_DURATION = 5.0f; // 5 seconds of speed boost

    public SpeedBoost(Level level, Vector2 startPosition) : base("Sprites/LevelObjects/spr_speedboost", TickTick.Depth_LevelObjects)
        
    {
        this.level = level;
        this.startPosition = startPosition;

        SetOriginToCenter();

        Reset();
    }


    

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Bouncing animation 
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + LocalPosition.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        localPosition.Y += bounce;

        // Check if the player collects this speed boost
        if (Visible && level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
        {
            Visible = false;

            // Apply speed boost to player
            level.Player.ApplySpeedBoost(SPEED_DURATION);

            ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_watercollected");
            
        }
    }

    public override void Reset()
    {
        localPosition = startPosition;
        Visible = true;
    }
}
