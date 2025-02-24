using System.Numerics;
using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;
using game;


class TileEditor
{
    // public static mapParser map = new mapParser();
    // public static string state {get; set;} = "";
    // public static Player player = new Player();
    Camera2D camera2D = new Camera2D();
    placeHolderProp placeHolder = new placeHolderProp();
    int gridSize;
    int currentZ;
    Color currentColor;
    public bool isPlayerPlaced;

    public void Begin()
    {
        camera2D.Target = new Vector2(0, 0); // Merkeze odaklan
        camera2D.Offset = new Vector2(0, 0);
        camera2D.Rotation = 0.0f;
        camera2D.Zoom = 1.0f;
        isPlayerPlaced = false;
        currentColor = Color.White;
        currentZ = -1;
        gridSize = 20;
    }

    public void Update(Vector2 WindowPosition)
    {
        
        //Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.ShowCursor();
          
        if (Raylib.IsKeyPressed(KeyboardKey.Up))
        {
            currentZ += 1;
            Console.WriteLine(currentZ);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Down))
        {
            currentZ -= 1;
            Console.WriteLine(currentZ);
        }
        placeHolder.position = new Vector2(Raylib.GetMousePosition().X - WindowPosition.X, Raylib.GetMousePosition().Y - WindowPosition.Y);
        Vector2 snappedPosition = placeHolder.GetSnappedPosition(gridSize);
        placeHolder.width = gridSize;
        placeHolder.height = gridSize;
        Rectangle placeHolderRect = new Rectangle(
            0,
            0,
            placeHolder.width,
            placeHolder.height
        );
                    
        Raylib.BeginMode2D(camera2D);
        for (int x = 0; x < Raylib.GetScreenWidth(); x += gridSize)
        {
            Raylib.DrawLine(x, 0, x, 450, Raylib_cs.Color.DarkGray);  
        }
        for (int y = 0; y < Raylib.GetScreenHeight(); y += gridSize)
        {
            Raylib.DrawLine(0, y, 800, y, Raylib_cs.Color.DarkGray);
        }            
        foreach (TileData tiles in game.game.map.MapData.tiles)
        {
            Vector2 tilePosition = new Vector2(tiles.x * gridSize, tiles.y * gridSize);
            Texture2D textures = !string.IsNullOrEmpty(tiles.texture) ? TextureManager.loadTexture(tiles.texture) : TextureManager.loadTexture("assets/a.png");
            textures.Width = gridSize;
            textures.Height = gridSize;
            Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, tiles.color);
        }
        foreach (DoorData door in game.game.map.MapData.doors)
        {
            Vector2 tilePosition = new Vector2(door.x * gridSize, door.y * gridSize);
            Texture2D textures = TextureManager.loadTexture(door.icon);
            textures.Width = gridSize;
            textures.Height = gridSize;
            Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, door.color);
        }
        foreach (playerData playerData1 in game.game.map.MapData.playerDatas)
        {
            Vector2 tilePosition = new Vector2(playerData1.x * gridSize, playerData1.y * gridSize);
            Texture2D textures = !string.IsNullOrEmpty(playerData1.icon) ? TextureManager.loadTexture(playerData1.icon) : TextureManager.loadTexture("assets/a.png");
            textures.Width = gridSize;
            textures.Height = gridSize;        
            Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, Color.White);
        }
        foreach (enemyData enemyData1 in game.game.map.MapData.enemyDatas)
        {
            Vector2 tilePosition = new Vector2(enemyData1.x * gridSize, enemyData1.y * gridSize);
            Texture2D textures = TextureManager.loadTexture(enemyData1.texture);
            textures.Width = gridSize;
            textures.Height = gridSize;        
            Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, Color.White);
        }
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            if (game.game.state == "cube")
            {

                string selectedTexturePath = gui.GetSelectedTexturePath();
                TileData tile = new TileData();
                if (game.game.map.MapData.tiles.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count == 0)
                {
                    game.game.map.MapData.tiles.Add(new TileData
                    {
                        x = (int)-snappedPosition.X,
                        y = (int)-snappedPosition.Y,
                        z = currentZ,
                        type = "cube",
                        texture = selectedTexturePath,
                        color = currentColor
                    });
                    mapWriter mapWriter = new mapWriter();
                    mapWriter.WriteMap(game.game.map.MapData);
                    Console.WriteLine(game.game.map.MapData.tiles.Count);
                }
            }
            if (game.game.state == "door")
            {

                string selectedTexturePath = gui.GetSelectedTexturePath();
                TileData tile = new TileData();
                if (game.game.map.MapData.doors.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count == 0)
                {
                    game.game.map.MapData.doors.Add(new DoorData
                    {
                        x = (int)-snappedPosition.X,
                        y = (int)-snappedPosition.Y,
                        z = currentZ,
                        icon = "assets/built-in/Door.png",
                        texture = selectedTexturePath,
                        color = currentColor
                    });
                    mapWriter mapWriter = new mapWriter();
                    mapWriter.WriteMap(game.game.map.MapData);
                }
                    
            }
            if (game.game.state == "player" && !isPlayerPlaced)
            {
                playerData playerData = new playerData
                {
                    x = (int)-snappedPosition.X,
                    y = (int)-snappedPosition.Y,
                    z = currentZ,
                    icon = "assets/built-in/playericon.png",
                    playerTexture = gui._selectedPlayerTexturePath
                };
                game.game.map.MapData.playerDatas.Add(playerData);
                mapWriter mapWriter = new mapWriter();
                mapWriter.WriteMap(game.game.map.MapData);
                isPlayerPlaced = true;
            }
            if (game.game.state == "enemy")
            {

                if (game.game.map.MapData.enemyDatas.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count == 0)
                {
                    enemyData enemyData = new enemyData
                    {
                        x = (int)-snappedPosition.X,
                        y = (int)-snappedPosition.Y,
                        z = currentZ,
                        texture = gui._selectedEnemyeTexturePath,
                        
                    };
                    game.game.map.MapData.enemyDatas.Add(enemyData);
                    mapWriter mapWriter = new mapWriter();
                    mapWriter.WriteMap(game.game.map.MapData);
                }
                
            }
            
            
        }
        if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                TileData tile = new TileData();
                playerData playerData = new playerData();
                if (game.game.map.MapData.tiles.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count > 0)
                {
                    game.game.map.MapData.tiles.RemoveAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ);
                    mapWriter mapWriter = new mapWriter();
                    mapWriter.WriteMap(game.game.map.MapData);
                }
                if (game.game.map.MapData.playerDatas.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count > 0)
                {
                    game.game.map.MapData.playerDatas.RemoveAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ);
                    mapWriter mapWriter = new mapWriter();
                    mapWriter.WriteMap(game.game.map.MapData);
                }
                        
            }
            Raylib.DrawRectangleV(snappedPosition * gridSize, new Vector2(gridSize, gridSize), Raylib.Fade(placeHolder.color, 0.9f));
        Raylib.EndMode2D();
        //Raylib.EndDrawing();
    }
}