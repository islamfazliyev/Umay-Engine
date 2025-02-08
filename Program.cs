using System.Data;
using System.Numerics;
using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;
using System.Reflection.Metadata;
using System.Diagnostics.Tracing;


namespace game{

    class game{
        public static mapParser map = new mapParser();
        public static string state {get; set;} = "";
        public static Player player = new Player();
        //public static Door Door = new Door();


        static void Main(){
            bool isPlayerPlaced = false;
            Raylib.InitWindow(800, 450, "Raylib_CsLo");
            Raylib.SetTargetFPS(60);
            rlImGui.Setup(true);
            placeHolderProp placeHolder = new placeHolderProp();
            int gridSize = 20;
            string texturePath = "";
            shaders shaders = new shaders();
            shaders.LoadResources();
            RenderTexture2D renderTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            Camera3D camera = new Camera3D();
            camera.Position = new Vector3(4.0f, 2.0f, 4.0f);
            camera.Target = new Vector3(0.0f, 1.8f, 0.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;
            CameraMode cameraMode = CameraMode.FirstPerson;
            Camera2D camera2D = new Camera2D();
            camera2D.Target = new Vector2(0.0f, 0.0f);
            camera2D.Offset = new Vector2(0.0f, 0.0f);
            camera2D.Rotation = 0.0f;
            camera2D.Zoom = 1.0f;
            bool isDoorOpened = false;
            
            bool tileMode = true;
            int currentZ = -1;
            Raylib_cs.Color currentColor = Raylib_cs.Color.White;
            while(!Raylib.WindowShouldClose()){
                float time = (float)Raylib.GetTime();
                
                if (Raylib.IsKeyDown(KeyboardKey.F1))
                {
                    tileMode = false;
                    player.Reload();


                }
                if (Raylib.IsKeyDown(KeyboardKey.F2))
                {
                    tileMode = true;
                }               
                

                if (tileMode)
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib_cs.Color.Black);
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
                    // if (Raylib.IsKeyPressed(KeyboardKey.Left))
                    // {
                    //     currentColor = Raylib_cs.Color.DarkGreen;
                    // }
                    // if (Raylib.IsKeyPressed(KeyboardKey.Right))
                    // {
                    //     currentColor = Raylib_cs.Color.Gray;
                    // }
                    placeHolder.position = Raylib.GetMousePosition();
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
                    
                    foreach (TileData tiles in map.MapData.tiles)
                    {
                        Vector2 tilePosition = new Vector2(tiles.x * gridSize, tiles.y * gridSize);
                        Texture2D textures = !string.IsNullOrEmpty(tiles.texture) ? TextureManager.loadTexture(tiles.texture) : TextureManager.loadTexture("assets/a.png");
                        textures.Width = gridSize;
                        textures.Height = gridSize;
                        Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, tiles.color);
                    }
                    foreach (DoorData door in map.MapData.doors)
                    {
                        Vector2 tilePosition = new Vector2(door.x * gridSize, door.y * gridSize);
                        Texture2D textures = TextureManager.loadTexture(door.icon);
                        textures.Width = gridSize;
                        textures.Height = gridSize;
                        Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, door.color);
                    }
                    foreach (playerData playerData1 in map.MapData.playerDatas)
                    {
                        Vector2 tilePosition = new Vector2(playerData1.x * gridSize, playerData1.y * gridSize);
                        Texture2D textures = !string.IsNullOrEmpty(playerData1.icon) ? TextureManager.loadTexture(playerData1.icon) : TextureManager.loadTexture("assets/a.png");
                        textures.Width = gridSize;
                        textures.Height = gridSize;
                        
                        Raylib.DrawTextureRec(textures, placeHolderRect, -tilePosition, Color.RayWhite);
                    }
                    if (InputManager.IsMouseAvailable() && Raylib.IsMouseButtonDown(MouseButton.Left))
                    {
                        if (state == "cube")
                        {
                            string selectedTexturePath = gui.GetSelectedTexturePath();
                            TileData tile = new TileData();
                            if (map.MapData.tiles.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count == 0)
                            {
                                map.MapData.tiles.Add(new TileData
                                {
                                    x = (int)-snappedPosition.X,
                                    y = (int)-snappedPosition.Y,
                                    z = currentZ,
                                    type = "cube",
                                    texture = selectedTexturePath,
                                    color = currentColor
                                });
                                mapWriter mapWriter = new mapWriter();
                                mapWriter.WriteMap(map.MapData);
                            }
                        }
                        if (state == "door")
                        {
                            string selectedTexturePath = gui.GetSelectedTexturePath();
                            TileData tile = new TileData();
                            if (map.MapData.doors.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count == 0)
                            {
                                map.MapData.doors.Add(new DoorData
                                {
                                    x = (int)-snappedPosition.X,
                                    y = (int)-snappedPosition.Y,
                                    z = currentZ,
                                    icon = "assets/built-in/Door.png",
                                    texture = selectedTexturePath,
                                    color = currentColor
                                });
                                mapWriter mapWriter = new mapWriter();
                                mapWriter.WriteMap(map.MapData);
                            }
                        }
                        if (state == "player" && !isPlayerPlaced)
                        {
                            

                            playerData playerData = new playerData();
                            map.MapData.playerDatas.Add(new playerData{
                                x = (int)-snappedPosition.X,
                                y = (int)-snappedPosition.Y,
                                z = currentZ,
                                icon = "assets/built-in/playericon.png",
                                playerTexture = gui._selectedPlayerTexturePath
                                 
                            });
                            mapWriter mapWriter = new mapWriter();
                            mapWriter.WriteMap(map.MapData);
                            isPlayerPlaced = true;
                        }
                        
                    }
                    
                    if (Raylib.IsMouseButtonDown(MouseButton.Right))
                    {
                        TileData tile = new TileData();
                        playerData playerData = new playerData();
                        if (map.MapData.tiles.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count > 0)
                        {
                            map.MapData.tiles.RemoveAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ);
                            mapWriter mapWriter = new mapWriter();
                            mapWriter.WriteMap(map.MapData);
                        }
                        if (map.MapData.playerDatas.FindAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ).Count > 0)
                        {
                            map.MapData.playerDatas.RemoveAll(x => x.x == -snappedPosition.X && x.y == -snappedPosition.Y && x.z == currentZ);
                            mapWriter mapWriter = new mapWriter();
                            mapWriter.WriteMap(map.MapData);
                        }
                        
                    }
                    Raylib.DrawRectangleV(snappedPosition * gridSize, new Vector2(gridSize, gridSize), Raylib.Fade(placeHolder.color, 0.9f));
                    rlImGui.Begin();
                    gui.drawGui();
                    rlImGui.End();
                    Raylib.EndMode2D();
                    Raylib.EndDrawing();


                }
                if (!tileMode)
                {
                    float deltaTime = Raylib.GetFrameTime();
                    Rectangle playerRec = new Rectangle(
                        0,
                        0,
                        500,
                        500
                    );
                    // Raylib.UpdateCamera(ref camera, CameraMode.Free);
                    player.Update(deltaTime);
                    Raylib.HideCursor();
                    Raylib.DisableCursor();
                    // 3D sahneyi renderTexture'a çiz
                    Raylib.BeginDrawing();

                    //Raylib.BeginTextureMode(renderTexture);
                    //Raylib.ClearBackground(Color.Black);
                    
                    Raylib.ClearBackground(Color.Black);
                    Raylib.BeginMode3D(player._camera);
                    
                    if (map.MapData.tiles != null)
                    {   
                        foreach (TileData tile in map.MapData.tiles)
                        {
                            Vector3 position = new Vector3(
                                tile.x * map.MapData.tileSize,
                                tile.z * map.MapData.tileSize,
                                tile.y * map.MapData.tileSize
                            );
                            Texture2D texture = TextureManager.loadTexture(tile.texture);
                            draw.DrawCubeTexture(
                                texture,
                                position,
                                map.MapData.tileSize,
                                map.MapData.tileSize,
                                map.MapData.tileSize,
                                tile.color
                            );
                        }
                    }
                    if (map.MapData.doors != null)
                    {   
                        foreach (DoorData doorData in map.MapData.doors)
                        {
                            Vector3 position = new Vector3(
                                doorData.x * map.MapData.tileSize,
                                doorData.z * map.MapData.tileSize,
                                doorData.y * map.MapData.tileSize
                            );
                            Texture2D texture = TextureManager.loadTexture(doorData.texture);
                            Door door = new Door(texture, position);
                            door.Update(); // Kapıyı güncelle (Rotasyon için)
                            door.Draw();   // Kapıyı çiz
                            float distance = Vector3.Distance(player._position, position);
                            
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
                    foreach (playerData data in map.MapData.playerDatas)
                    {
                        if (data.placed == true)
                        {
                            
                            Texture2D PlayerTexture = TextureManager.loadTexture(data.playerTexture);
                            PlayerTexture.Width = (int)playerRec.Width;
                            PlayerTexture.Height = (int)playerRec.Height;
                            Raylib.DrawTextureRec(PlayerTexture, playerRec, new Vector2(Raylib.GetScreenWidth() -400, Raylib.GetScreenHeight()-501), Color.RayWhite);
                        }
                    }
                    
                    //Raylib.EndMode2D();
                    Raylib.EndTextureMode();

                    // Shader ile renderTexture'ı ekrana çiz
                    // Raylib.BeginShaderMode(shaders.vhsShader);
                    
                    // Raylib.SetShaderValue(shaders.vhsShader, 
                    //     Raylib.GetShaderLocation(shaders.vhsShader, "time"), 
                    //     time, ShaderUniformDataType.Float);
                    
                    // // RenderTexture'ı ekrana çiz
                    // Raylib.DrawTextureRec(
                    //     renderTexture.Texture,
                    //     new Rectangle(0, 0, renderTexture.Texture.Width, -renderTexture.Texture.Height),
                    //     Vector2.Zero,
                    //     Color.White
                    // );
                    // Raylib.EndShaderMode();
                    Raylib.EndDrawing();
                }
            }
            Raylib.CloseWindow();
            TextureManager.unloadTextures();
            shaders.UnloadResources();
            Raylib.UnloadRenderTexture(renderTexture);
            rlImGui.Shutdown();
        }

        
    }
}