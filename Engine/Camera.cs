using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// A camera that defines which part of the game world is currently visible on screen.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The position of the camera in world coordinates (top-left corner).
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of the visible area (viewport size in world units).
        /// </summary>
        public Point ViewportSize { get; private set; }

        /// <summary>
        /// Gets the bounds of the camera as a Rectangle.
        /// </summary>
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

        /// <summary>
        /// Creates a new Camera with the specified viewport size.
        /// </summary>
        /// <param name="viewportSize">The size of the visible area in world units.</param>
        public Camera(Point viewportSize)
        {
            ViewportSize = viewportSize;
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Converts a world position to a screen position.
        /// </summary>
        /// <param name="worldPosition">Position in world coordinates.</param>
        /// <returns>Position in screen coordinates.</returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - Position;
        }

        /// <summary>
        /// Converts a screen position to a world position.
        /// </summary>
        /// <param name="screenPosition">Position in screen coordinates.</param>
        /// <returns>Position in world coordinates.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + Position;
        }

        /// <summary>
        /// Centers the camera on a specific position while respecting world boundaries.
        /// </summary>
        /// <param name="targetPosition">The position to center on.</param>
        /// <param name="worldWidth">Total width of the world.</param>
        /// <param name="worldHeight">Total height of the world.</param>
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

        /// <summary>
        /// Centers the camera on a specific position (without bounds checking).
        /// </summary>
        /// <param name="targetPosition">The position to center on.</param>
        public void CenterOn(Vector2 targetPosition)
        {
            Position = new Vector2(
                targetPosition.X - ViewportSize.X / 2f,
                targetPosition.Y - ViewportSize.Y / 2f
            );
        }

        /// <summary>
        /// Clamps the camera position within specified world bounds.
        /// </summary>
        /// <param name="worldWidth">Total width of the world.</param>
        /// <param name="worldHeight">Total height of the world.</param>
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
