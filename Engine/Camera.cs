using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Camera
    {
        /// <summary>
        /// Top-left corner of the camera view in world coordinates.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Size of the visible area in world units.
        /// </summary>
        public Point ViewportSize { get; private set; }

        /// <summary>
        /// Rectangle of the world currently visible.
        /// </summary>
        public Rectangle ViewRectangle
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
        /// Create a camera with the specified viewport size (world units).
        /// </summary>
        public Camera(Point viewportSize)
        {
            ViewportSize = viewportSize;
            Position = new Vector2(0, 0);
        }

       
        public Camera(Point viewportSize, Vector2 position)
        {
            ViewportSize = viewportSize;
            Position = position;
        }

   
        public Matrix GetTransformationMatrix()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
        }

       
        public void Move(Vector2 offset)
        {
            Position += offset;
        }

        public void CenterOn(Vector2 worldPosition)
        {
            Position = new Vector2(
                worldPosition.X - ViewportSize.X / 2f,
                worldPosition.Y - ViewportSize.Y / 2f
            );
        }

        public void ClampToWorld(Rectangle worldBounds)
        {
            float minX = worldBounds.Left;
            float minY = worldBounds.Top;
            float maxX = worldBounds.Right - ViewportSize.X;
            float maxY = worldBounds.Bottom - ViewportSize.Y;

            Position = new Vector2(
                MathHelper.Clamp(Position.X, minX, maxX),
                MathHelper.Clamp(Position.Y, minY, maxY)
            );
        }

   
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - Position;
        }

      
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + Position;
        }

      
        public bool IsVisible(Rectangle worldRectangle)
        {
            return ViewRectangle.Intersects(worldRectangle);
        }

       
        public bool IsVisible(Vector2 worldPoint)
        {
            return ViewRectangle.Contains(worldPoint.ToPoint());
        }
    }


}

