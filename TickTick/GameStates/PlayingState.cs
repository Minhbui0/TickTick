using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class PlayingState : GameState, IPlayingState
{
    Level level;
    Button quitButton;
    SpriteGameObject completedOverlay, gameOverOverlay, hintFrame;
    TextGameObject hintText;
    BombTimer timer;


    public PlayingState()
    {
        // add a "quit" button
        quitButton = new Button("Sprites/UI/spr_button_quit", 1);
        quitButton.LocalPosition = new Vector2(1290, 20);
        ui.AddChild(quitButton);

        // add the timer
        timer = new BombTimer();
        timer.LocalPosition = new Vector2(20, 20);
        ui.AddChild(timer);

        // Add the hint UI elements to the ui list
        hintFrame = new SpriteGameObject("Sprites/UI/spr_frame_hint", TickTick.Depth_UIBackground);
        hintFrame.SetOriginToCenter();
        hintFrame.LocalPosition = new Vector2(720, 50);
        ui.AddChild(hintFrame);

        hintText = new TextGameObject("Fonts/HintFont", TickTick.Depth_UIForeground, Color.Black, TextGameObject.Alignment.Left);
        hintText.LocalPosition = new Vector2(510, 40);
        ui.AddChild(hintText);

        // add overlay images
        completedOverlay = AddOverlay("Sprites/UI/spr_welldone");
        gameOverOverlay = AddOverlay("Sprites/UI/spr_gameover");
    }

    SpriteGameObject AddOverlay(string spriteName)
    {
        SpriteGameObject result = new SpriteGameObject(spriteName, 1);
        result.SetOriginToCenter();
        result.LocalPosition = new Vector2(720, 412);
        ui.AddChild(result);
        return result;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);

        if (level != null)
        {
            // if the player character has died, allow the player to reset the level (inludes restarting the timer).
            if (gameOverOverlay.Visible)
            {
                if (inputHelper.KeyPressed(Keys.Space))
                { 
                    level.Reset();
                    timer.Reset();
                    timer.Running = true;
                    hintFrame.Visible = true;
                    hintText.Visible = true;
                }
            }
            
            // if the level has been completed, pressing the spacebar should send the player to the next level
            else if (completedOverlay.Visible)
            {
                if (inputHelper.KeyPressed(Keys.Space))
                    ExtendedGameWithLevels.GoToNextLevel(level.LevelIndex);
            }

            // otherwise, update the level itself, and check for button presses
            else 
            {
                level.HandleInput(inputHelper);

                if (quitButton.Pressed)
                    ExtendedGame.GameStateManager.SwitchTo(ExtendedGameWithLevels.StateName_LevelSelect);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (level != null)
        {
            level.Update(gameTime);

            // show or hide the "game over" image
            gameOverOverlay.Visible = !level.Player.IsAlive;

            if (!level.Player.IsAlive)
            {
                timer.Running = false;
                //hintFrame.Visible = false;
                //hintText.Visible = false;
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        if (level != null)
            level.Draw(gameTime, spriteBatch);
    }

    public void LoadLevel(int levelIndex)
    {
        level = new Level(levelIndex, ExtendedGame.ContentRootDirectory + "/Levels/level" + levelIndex + ".txt");

        level.Timer = this.timer;

        // Reset and start the timer for the new level
        timer.Reset();
        timer.Running = true;

        // Set the hint text from the level's description
        hintText.Text = level.Description ?? "";
        hintFrame.Visible = true;
        hintText.Visible = true;

        // Hide the overlay images
        completedOverlay.Visible = false;
        gameOverOverlay.Visible = false;
    }

    public void LevelCompleted(int levelIndex)
    {
        // show an overlay image
        completedOverlay.Visible = true;

        // Hide hint text on level complete
        hintFrame.Visible = false;
        hintText.Visible = false;

        // play a sound
        ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_won");

        // mark the level as solved, and unlock the next level
        ExtendedGameWithLevels.MarkLevelAsSolved(levelIndex);
    }
}