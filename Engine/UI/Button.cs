using Microsoft.Xna.Framework;

namespace Engine.UI
{
    /// <summary>
    /// A class that can represent a UI button in the game.
    /// </summary>
    public class Button : SpriteGameObject
    {
        Vector2 mouseUIPosition;

        /// <summary>
        /// Whether this button has been pressed (clicked) in the current frame.
        /// </summary>
        public bool Pressed { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="Button"/> with the given sprite name and depth.
        /// </summary>
        /// <param name="assetName">The name of the sprite to use.</param>
        /// <param name="depth">The depth at which the button should be drawn.</param>
        public Button(string assetName, float depth) : base(assetName, depth)
        {
            Pressed = false;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            // Convert the world mouse position to a UI mouse position by subtracting the camera's offset
            Vector2 mouseUIPosition = inputHelper.MousePositionWorld - ExtendedGame.Camera.Position;

            Pressed = Visible && inputHelper.MouseLeftButtonPressed()
                && BoundingBox.Contains(mouseUIPosition);
        }

        public override void Reset()
        {
            base.Reset();
            Pressed = false;
        }
    }
}