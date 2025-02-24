using System.Numerics;
using Raylib_cs;


class render3D
{
    Camera3D camera = new Camera3D();
    public shaders shaders = new shaders();
    private List<enemy> _enemies = new List<enemy>();
    private bool isDoorOpened;

    public void Begin()
    {
        shaders.LoadResources();
        camera.Position = new Vector3(4.0f, 2.0f, 4.0f);
        camera.Target = new Vector3(0.0f, 1.8f, 0.0f);
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        camera.FovY = 60.0f;
        camera.Projection = CameraProjection.Perspective;
        //CameraMode cameraMode = CameraMode.FirstPerson;
    }

    public void Update(RenderTexture2D targetTexture)
    {
        float time = (float)Raylib.GetTime();
        float deltaTime = Raylib.GetFrameTime();
        Rectangle playerRec = new Rectangle(
            0,
            0,
            500,
            500
        );
                    // Raylib.UpdateCamera(ref camera, CameraMode.Free);
        game.game.player.Update(deltaTime);
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            game.game.player.Reload();
        }
        //Raylib.HideCursor();
        //Raylib.DisableCursor();
                    // 3D sahneyi renderTexture'a çiz
        //Raylib.BeginDrawing();

        Raylib.BeginTextureMode(targetTexture);
                    //Raylib.ClearBackground(Color.Black);
                    
        Raylib.ClearBackground(Color.Black);
        Raylib.BeginMode3D(game.game.player._camera);  
        Texture2D texture2D = TextureManager.loadTexture("assets/a.png"); 
        
                 
        if (game.game.map.MapData.tiles != null)
        {   
            foreach (TileData tile in game.game.map.MapData.tiles)
            {
                Vector3 position = new Vector3(
                    tile.x * game.game.map.MapData.tileSize,
                    tile.z * game.game.map.MapData.tileSize,
                    tile.y * game.game.map.MapData.tileSize
                );
                Texture2D texture = TextureManager.loadTexture(tile.texture);
                game.draw.DrawCubeTexture(
                    texture,
                    position,
                    game.game.map.MapData.tileSize,
                    game.game.map.MapData.tileSize,
                    game.game.map.MapData.tileSize,
                    tile.color
                );
                
            }
        }
        if (game.game.map.MapData.enemyDatas != null)
        {   
            if (_enemies.Count == 0)
            {
                foreach (enemyData data in game.game.map.MapData.enemyDatas)
                {
                    enemy newEnemy = new enemy();
                    newEnemy.Data = data; // 👈 enemyData referansını ata
                    newEnemy.enemyPosition = new Vector3(
                        data.x * game.game.map.MapData.tileSize,
                        data.z * game.game.map.MapData.tileSize,
                        data.y * game.game.map.MapData.tileSize
                    );
                    _enemies.Add(newEnemy);
                }
            }
            foreach (var enemy in _enemies)
            {
                enemy.Update(game.game.player._position);
                enemy.Draw(game.game.map.MapData.tileSize, game.game.map.MapData.tileSize, game.game.map.MapData.tileSize);
            }
        }
        if (game.game.map.MapData.doors != null)
        {   
            foreach (DoorData doorData in game.game.map.MapData.doors)
            {
                Vector3 position = new Vector3(
                doorData.x * game.game.map.MapData.tileSize,
                doorData.z * game.game.map.MapData.tileSize,
                doorData.y * game.game.map.MapData.tileSize
            );
            Texture2D texture = TextureManager.loadTexture(doorData.texture);
            Door door = new Door(texture, position);
            door.Update(); // Kapıyı güncelle (Rotasyon için)
            door.Draw();   // Kapıyı çiz
            float distance = Vector3.Distance(game.game.player._position, position);
                            
            if (distance <= 10)
            {
                if (!isDoorOpened)
                {
                    doorData.z += 1;
                    isDoorOpened = true;
                }
            }
            if (distance > 13)
            {
                if (isDoorOpened)
                {
                    doorData.z -= 1;
                    isDoorOpened = false;
                }
            }
        }
                    
        }
        Raylib.EndMode3D();
                    //Raylib.BeginMode2D(new Camera2D());
        foreach (playerData data in game.game.map.MapData.playerDatas)
        {
            if (data.placed == true)
            {
                Texture2D PlayerTexture = TextureManager.loadTexture(data.playerTexture);
                PlayerTexture.Width = (int)playerRec.Width;
                PlayerTexture.Height = (int)playerRec.Height;
                Raylib.DrawTextureRec(PlayerTexture, playerRec, new Vector2(Raylib.GetScreenWidth() -400, Raylib.GetScreenHeight()-501), Color.RayWhite);
            }
        }
        Raylib.EndTextureMode();
        Raylib.EndShaderMode();
        //Raylib.EndDrawing();
    }
}