using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    /// <summary>
    /// A class that manages all objects belonging to a single game state.
    /// </summary>
    public abstract class GameState : IGameLoopObject
    {
        /// <summary>
        /// The game objects in the game world, affected by the camera.
        /// </summary>
        protected GameObjectList world;

        /// <summary>
        /// The game objects on the UI layer, not affected by the camera.
        /// </summary>
        protected GameObjectList ui;

        /// <summary>
        /// Creates a new GameState object with an empty list for world and UI objects.
        /// </summary>
        protected GameState()
        {
            world = new GameObjectList();
            ui = new GameObjectList();
        }

        /// <summary>
        /// Calls HandleInout for all world and UI objects in this GameState.
        /// </summary>
        /// <param name="inputHelper">An object required for handling player input.</param>
        public virtual void HandleInput(InputHelper inputHelper)
        {
            world.HandleInput(inputHelper);
            ui.HandleInput(inputHelper);
        }

        /// <summary>
        /// Calls Update for all world and UI objects in this GameState.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        public virtual void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            ui.Update(gameTime);
        }

        /// <summary>
        /// Draws all world objects in this GameState.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            world.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draws all UI objects in this GameState.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public virtual void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ui.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Calls Reset for all objects in this GameState.
        /// </summary>
        public virtual void Reset()
        {
            world.Reset();
            ui.Reset();
        }
    }
}