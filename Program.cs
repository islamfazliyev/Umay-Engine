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
        static RenderTexture2D tileViewTexture;
        static RenderTexture2D gameViewTexture;

        static void Main(){
            bool isPlayerPlaced = false;
            Raylib.InitWindow(1600, 900, "Umay Engine");
            Raylib.SetTargetFPS(60);
            rlImGui.Setup(true);
            tileViewTexture = Raylib.LoadRenderTexture(800, 450);
            gameViewTexture = Raylib.LoadRenderTexture(800, 450);
            TileEditor tileEditor = new TileEditor();
            render3D render = new render3D();
            // testEditor testEditor = new testEditor();
            render.Begin();
            tileEditor.Begin();
            Vector2 TileWindowPosition = new Vector2(0, 0);
            // testEditor.Begin();
            // bool tileMode = true;
            // bool testMode = false;
            while(!Raylib.WindowShouldClose()){
                
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                rlImGui.Begin();
                Raylib.BeginTextureMode(tileViewTexture);
                //Raylib.ClearBackground(Color.Black);
                tileEditor.Update(TileWindowPosition);
                Raylib.EndTextureMode();
                Raylib.BeginTextureMode(gameViewTexture);
                //Raylib.ClearBackground(Color.Black);
                render.Update(gameViewTexture);
                Raylib.EndTextureMode();

                // ImGui render
                Raylib.ClearBackground(Color.Black);
                
                ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), 
                ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.NoDockingOverCentralNode) ;

                // Game View (Sol panel)
                ImGui.SetNextWindowDockID(1, ImGuiCond.FirstUseEver);
                ImGui.Begin("Tile Editor", ImGuiWindowFlags.NoScrollbar);
                Vector2 WindowPosition = new Vector2(ImGui.GetWindowPos().X, ImGui.GetWindowPos().Y);
                TileWindowPosition = WindowPosition;
                
                //Console.WriteLine(TileWindowSize);
                rlImGui.ImageRect(tileViewTexture.Texture, 
                    (int)ImGui.GetWindowSize().X,
                    (int)ImGui.GetWindowSize().Y - 100, 
                    new Rectangle(0, 0, tileViewTexture.Texture.Width, -tileViewTexture.Texture.Height));
                ImGui.End();

                ImGui.SetNextWindowDockID(2, ImGuiCond.FirstUseEver);
                ImGui.Begin("3D View", ImGuiWindowFlags.NoScrollbar);    
                // Raylib.BeginShaderMode(render.shaders.vhsShader);        
                // Raylib.SetShaderValue(render.shaders.vhsShader, 
                // Raylib.GetShaderLocation(render.shaders.vhsShader, "time"), 
                // (float)Raylib.GetTime(), ShaderUniformDataType.Float);
                            
                            // // RenderTexture'ı ekrana çiz
                // Raylib.DrawTextureRec(
                //     gameViewTexture.Texture,
                //     new Rectangle(0, 0, gameViewTexture.Texture.Width, -gameViewTexture.Texture.Height),
                //     Vector2.Zero,
                //     Color.White
                // );            
                //Console.WriteLine(TileWindowSize);
                rlImGui.ImageRect(gameViewTexture.Texture, 
                    (int)ImGui.GetWindowSize().X,
                    (int)ImGui.GetWindowSize().Y - 100, 
                    new Rectangle(0, 0, gameViewTexture.Texture.Width, -gameViewTexture.Texture.Height));
                ImGui.End();
                gui.drawGui();
                ImGui.End();

                rlImGui.End();
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
            TextureManager.unloadTextures();
            // shaders.UnloadResources();
            // Raylib.UnloadRenderTexture(renderTexture);
            rlImGui.Shutdown();
        }

        
    }
}