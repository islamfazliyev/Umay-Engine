﻿using System.Data;
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

        static void Main(){
            bool isPlayerPlaced = false;
            Raylib.InitWindow(800, 450, "Raylib_CsLo");
            Raylib.SetTargetFPS(60);
            rlImGui.Setup(true);
            TileEditor tileEditor = new TileEditor();
            render3D render = new render3D();
            render.Begin();
            tileEditor.Begin();
            bool tileMode = true;
            while(!Raylib.WindowShouldClose()){
                
                
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
                    tileEditor.Update();
                }
                if (!tileMode)
                {
                    render.Update();
                }
            }
            Raylib.CloseWindow();
            TextureManager.unloadTextures();
            // shaders.UnloadResources();
            // Raylib.UnloadRenderTexture(renderTexture);
            rlImGui.Shutdown();
        }

        
    }
}