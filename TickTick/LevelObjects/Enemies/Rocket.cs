using Engine;
using Microsoft.Xna.Framework;

/// <summary>
/// Represents a rocket enemy that flies horizontally through the screen.
/// </summary>
class Rocket : AnimatedGameObject
{
    Level level;
    Vector2 startPosition;
    const float speed = 500;
    bool isDead;
    float timeUntilRemoved;

    public Rocket(Level level, Vector2 startPosition, bool facingLeft) 
        : base(TickTick.Depth_LevelObjects)
    {
        this.level = level;

        LoadAnimation("Sprites/LevelObjects/Rocket/spr_rocket@3", "rocket", true, 0.1f);
        PlayAnimation("rocket");
        SetOriginToCenter();

        sprite.Mirror = facingLeft;
        if (sprite.Mirror)
        {
            velocity.X = -speed;
            this.startPosition = startPosition + new Vector2(2 * speed, 0);
        }
        else
        {
            velocity.X = speed;
            this.startPosition = startPosition - new Vector2(2 * speed, 0);
        }
        Reset();
    }

    public override void Reset()
    {
        // go back to the starting position
        LocalPosition = startPosition;

        // reset the rocket's state
        isDead = false;
        Visible = true;
        velocity.Y = 0; // Make sure it's not falling
        velocity.X = sprite.Mirror ? -speed : speed; // restore horizontal speed
        PlayAnimation("rocket"); // play the flying animation
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // if the rocket is dead, make it fall off-screen
        if (isDead)
        {
            // apply gravity
            velocity.Y += 50;
            // count down until it is removed
            timeUntilRemoved -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeUntilRemoved <= 0)
                Reset();
            return;
        }

            // if the rocket has left the screen, reset it
            if (sprite.Mirror && BoundingBox.Right < level.BoundingBox.Left)
            Reset();
        else if (!sprite.Mirror && BoundingBox.Left > level.BoundingBox.Right)
            Reset();

        // if the rocket touches the player
        if (level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
        {
            // if the player is falling and if the player's bottom edge is above the rocket (rocket dies)
            if (level.Player.Velocity.Y > 0 && level.Player.BoundingBox.Bottom < this.GlobalPosition.Y)
            {
                Die();
                Vector2 playerVel = level.Player.Velocity;
                playerVel.Y = -600; // make the player bounce
                level.Player.Velocity = playerVel;
                ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_stomp");
            }
            // otherwise regular player death
            else
                level.Player.Die();
        }  
    }

    /// <summary>
    /// Kills the rocket, playing a sound and a falling animation.
    /// </summary>
    public void Die()
    {
        if (isDead) return; // if the rocket is dead already, it will not die again

        isDead = true;
        timeUntilRemoved = 1.0f; // 1 second to fall off-screen
        velocity.X = 0; // stop horizontal movement
    }
}
