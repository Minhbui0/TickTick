using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

partial class Level : GameObjectList
{
    public const int TileWidth = 72;
    public const int TileHeight = 55;

    Tile[,] tiles;
    List<WaterDrop> waterDrops;

    public Player Player { get; private set; }
    public int LevelIndex { get; private set; }

    SpriteGameObject goal;
    BombTimer timer;
    bool completionDetected;

    // Properties for level dimensions
    public int LevelWidth { get; private set; }
    public int LevelHeight { get; private set; }

    public Level(int levelIndex, string filename)
    {
        LevelIndex = levelIndex;

        // load the rest of the level
        LoadLevelFromFile(filename);

        // Calculate level dimensions after loading tiles
        LevelWidth = tiles.GetLength(0) * TileWidth;
        LevelHeight = tiles.GetLength(1) * TileHeight;

        // load the backgrounds
        LoadBackgrounds();

        // add the timer
        timer = new BombTimer();
        AddChild(timer);
    }

    private void LoadBackgrounds()
    {
        GameObjectList backgrounds = new GameObjectList();

        // Background sky - tile it horizontally if level is wider than default
        int numSkyTiles = (int)Math.Ceiling((float)LevelWidth / 1440f);
        if (numSkyTiles < 1) numSkyTiles = 1;

        for (int i = 0; i < numSkyTiles; i++)
        {
            SpriteGameObject backgroundSky = new SpriteGameObject("Sprites/Backgrounds/spr_sky", TickTick.Depth_Background);
            backgroundSky.LocalPosition = new Vector2(i * 1440, 825 - backgroundSky.Height);
            backgrounds.AddChild(backgroundSky);
        }

        AddChild(backgrounds);

        // Add mountains with some variation - scale based on level width
        int numMountains = Math.Max(4, (int)Math.Ceiling((float)LevelWidth / 360f));
        for (int i = 0; i < numMountains; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject(
                "Sprites/Backgrounds/spr_mountain_" + (ExtendedGame.Random.Next(2) + 1),
                TickTick.Depth_Background + 0.01f * (float)ExtendedGame.Random.NextDouble());
            mountain.LocalPosition = new Vector2(mountain.Width * (i - 1) * 0.4f,
                LevelHeight - mountain.Height);
            backgrounds.AddChild(mountain);
        }

        // add clouds
        for (int i = 0; i < 6; i++)
            backgrounds.AddChild(new Cloud(this));
    }

    public Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(0, 0, LevelWidth, LevelHeight);
        }
    }

    public BombTimer Timer { get { return timer; } }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * TileWidth, y * TileHeight);
    }

    public Point GetTileCoordinates(Vector2 position)
    {
        return new Point((int)Math.Floor(position.X / TileWidth), (int)Math.Floor(position.Y / TileHeight));
    }

    public Tile.Type GetTileType(int x, int y)
    {
        // If the x-coordinate is out of range, treat the coordinates as a wall tile.
        // This will prevent the character from walking outside the level.
        if (x < 0 || x >= tiles.GetLength(0))
            return Tile.Type.Wall;

        // If the y-coordinate is out of range, treat the coordinates as an empty tile.
        // This will allow the character to still make a full jump near the top of the level.
        if (y < 0 || y >= tiles.GetLength(1))
            return Tile.Type.Empty;

        return tiles[x, y].TileType;
    }

    public Tile.SurfaceType GetSurfaceType(int x, int y)
    {
        // If the tile with these coordinates doesn't exist, return the normal surface type.
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
            return Tile.SurfaceType.Normal;

        // Otherwise, return the actual surface type of the tile.
        return tiles[x, y].Surface;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Update camera to follow player
        if (Player != null && Player.IsAlive)
        {
            UpdateCamera();
        }

        // check if we've finished the level
        if (!completionDetected && AllDropsCollected && Player.HasPixelPreciseCollision(goal))
        {
            completionDetected = true;
            ExtendedGameWithLevels.GetPlayingState().LevelCompleted(LevelIndex);
            Player.Celebrate();
            // stop the timer
            timer.Running = false;
        }
        // check if the timer has passed
        else if (Player.IsAlive && timer.HasPassed)
        {
            Player.Explode();
        }
    }

    private void UpdateCamera()
    {
        // Center camera on player
        ExtendedGame.Camera.CenterOn(Player.GlobalPosition);

        // Clamp camera to level bounds
        ExtendedGame.Camera.ClampToWorld(LevelWidth, LevelHeight);
    }

    /// <summary>
    /// Checks and returns whether the player has collected all water drops in this level.
    /// </summary>
    bool AllDropsCollected
    {
        get
        {
            foreach (WaterDrop drop in waterDrops)
                if (drop.Visible)
                    return false;
            return true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        completionDetected = false;
    }
}
