using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    /// <summary>
    /// A SpriteGameObject that moves at a different speed relative to the camera
    /// to create a parallax scrolling effect.
    /// </summary>
    public class ParallaxGameObject : SpriteGameObject
    {
        /// <summary>
        /// The parallax factor.
        /// 1.0f = Moves locked to the level (no parallax).
        /// 0.0f = Stays fixed on the screen (like UI).
        /// 0.5f = Moves at half the speed of the camera.
        /// </summary>
        public float ParallaxFactor { get; set; }

        public ParallaxGameObject(string assetName, float depth, float parallaxFactor = 1.0f, int sheetIndex = 0)
            : base(assetName, depth, sheetIndex) // Pass all 3 required args
        {
            ParallaxFactor = parallaxFactor;
        }

        /// <summary>
        /// Overrides the default Draw method to apply parallax scrolling.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible || sprite == null)
                return;

            // Get the camera's position
            Vector2 cameraPosition = ExtendedGame.Camera.Position;

            // Calculate the "counter-movement"
            Vector2 counterScroll = cameraPosition * (1.0f - ParallaxFactor);

            // Save our original position
            Vector2 originalLocalPos = LocalPosition;

            // Apply the counter-scroll to our local position
            LocalPosition += counterScroll;

            // Call the base class's Draw method
            base.Draw(gameTime, spriteBatch);

            // Restore our original position for the next Update frame
            LocalPosition = originalLocalPos;
        }
    }
}