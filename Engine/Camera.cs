using Microsoft.Xna.Framework;

namespace Engine
{
   

    
    public class Camera
    {
        
        // The position of the camera in world coordinates (top-left corner).
        public Vector2 Position { get; set; }

       
        // The size of the visible area (viewport size in world units).
       
        public Point ViewportSize { get; private set; }

        
        // Gets the bounds of the camera as a Rectangle.
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    ViewportSize.X,
                    ViewportSize.Y
                );
            }
        }

        
        //Creates a new Camera with the specified viewport size.
              
        public Camera(Point viewportSize)
        {
            ViewportSize = viewportSize;
            Position = Vector2.Zero;
        }

      
        // Converts a world position to a screen position.
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - Position;
        }

        // Converts a screen position to a world position.
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + Position;
        }

       
        // Centers the camera on a specific position while respecting world boundaries.
        public void FollowTarget(Vector2 targetPosition, int worldWidth, int worldHeight)
        {
            // Calculate desired camera position (centered on target)
            Vector2 desiredPosition = new Vector2(
                targetPosition.X - ViewportSize.X / 2f,
                targetPosition.Y - ViewportSize.Y / 2f
            );

            // Clamp the camera position to stay within world bounds
            float clampedX = desiredPosition.X;
            float clampedY = desiredPosition.Y;

            if (worldWidth > ViewportSize.X)
                clampedX = MathHelper.Clamp(desiredPosition.X, 0, worldWidth - ViewportSize.X);
            else
                clampedX = (worldWidth - ViewportSize.X) / 2f; // Center if world is smaller

            if (worldHeight > ViewportSize.Y)
                clampedY = MathHelper.Clamp(desiredPosition.Y, 0, worldHeight - ViewportSize.Y);
            else
                clampedY = (worldHeight - ViewportSize.Y) / 2f; // Center if world is smaller

            Position = new Vector2(clampedX, clampedY);
        }

       
        // Centers the camera on a specific position (without bounds checking).
        public void CenterOn(Vector2 targetPosition)
        {
            Position = new Vector2(
                targetPosition.X - ViewportSize.X / 2f,
                targetPosition.Y - ViewportSize.Y / 2f
            );
        }

       
        // Clamps the camera position within specified world bounds
        public void ClampToWorld(int worldWidth, int worldHeight)
        {
            float clampedX = Position.X;
            float clampedY = Position.Y;

            if (worldWidth > ViewportSize.X)
                clampedX = MathHelper.Clamp(Position.X, 0, worldWidth - ViewportSize.X);
            else
                clampedX = 0;

            if (worldHeight > ViewportSize.Y)
                clampedY = MathHelper.Clamp(Position.Y, 0, worldHeight - ViewportSize.Y);
            else
                clampedY = 0;

            Position = new Vector2(clampedX, clampedY);
        }
    }
}
