using System.Text;
using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;
using System.Numerics;
using System.Net.Http.Json;
using System.Runtime.Versioning;
using System.Diagnostics;

namespace game
{
    class gui
    {
        private static FileBrowser _fileBrowser;
        private static byte[] _texturePathBuffer = new byte[256];
        private static string _selectedTexturePath = "";
        public static string _selectedPlayerTexturePath = "";
        public static string _selectedEnemyeTexturePath = "";
        public static string buildPath = "";
        public static bool isFileBrowserOpen = false;
        private static mapBuildParser mapBuildParser = new mapBuildParser();
        private static List<tilebuttons> tileButtons = new List<tilebuttons>();
        private static List<doorbuttons> doorButtons = new List<doorbuttons>();
        private static List<enemybuttons> enemyButtons = new List<enemybuttons>();
        //private static List<buildData> BuildData = new List<buildData>();
        private static Texture2D playerTextureOnGui;
        private static bool build = false;
        private enum mapSelectionEnum
        {
            Builder,
            Loader
        }
        private static mapSelectionEnum mapSelection = mapSelectionEnum.Loader; 
        private static int currentMapIndex = 0;
        public static void drawGui()
        {

            guiStyle.ApplyStyle();
            _fileBrowser = new FileBrowser(initialPath: "assets", allowedExtensions: new string[] { "png", "json" });
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

            // Tile Editor penceresi (sol taraf)
            ImGui.SetNextWindowPos(new Vector2(0, 0), ImGuiCond.FirstUseEver); // İlk açılışta sol üst köşeye yerleştir
            ImGui.SetNextWindowSize(new Vector2(ImGui.GetMainViewport().Size.X * 0.5f, ImGui.GetMainViewport().Size.Y), ImGuiCond.FirstUseEver);

            // Tile Mode penceresi (sağ taraf)
            ImGui.SetNextWindowPos(new Vector2(ImGui.GetMainViewport().Size.X * 0.5f, 0), ImGuiCond.FirstUseEver); // Sağ tarafa yerleştir
            ImGui.SetNextWindowSize(new Vector2(ImGui.GetMainViewport().Size.X * 0.5f, ImGui.GetMainViewport().Size.Y), ImGuiCond.FirstUseEver);
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.MenuItem("Save Map"))
                {
                    isFileBrowserOpen=true;
                }
                if (ImGui.MenuItem("Load Map"))
                {
                    isFileBrowserOpen=true;
                    mapSelection = mapSelectionEnum.Loader;
                }
                if (ImGui.MenuItem("Build Game"))
                {
                    build = true;
                }
            }
            if (build)
            {
                ImGui.Begin("Build");
                ImGui.BeginChild("Maps", new Vector2(200, 200));
                foreach (mapListData maps in mapBuildParser.MapData.mapListDatas)
                {
                    ImGui.Selectable($"{maps.mapIndex}: {maps.mapName}");
                }
                ImGui.EndChild();
                if (ImGui.Button("Add Map"))
                {
                    isFileBrowserOpen = true;
                    mapSelection = mapSelectionEnum.Builder;
                }
                ImGui.InputText("Build Path", ref buildPath, 256);
                if (ImGui.Button("Build Game"))
                {
                    buildGame buildGame = new buildGame();
                    buildGame.Main(buildPath);
                }
                ImGui.End();
            }

            ImGui.Begin("Tile Mode");
            if (ImGui.Button("Clear All Tiles"))
            {
                game.map.MapData.tiles.RemoveAll(x => true);
            }
            if (ImGui.Button("Cube"))
            {
                game.state = "cube";
            }
            ImGui.SameLine();
            if (ImGui.Button("Player"))
            {
                game.state = "player";
                
            }
            ImGui.SameLine();
            if (ImGui.Button("Door"))
            {
                game.state = "door";
                
            }
            ImGui.SameLine();
            if (ImGui.Button("Enemy"))
            {
                game.state = "enemy";
                
            }
            if (game.state == "cube")
            {
                ImGui.InputText("Texture Path", _texturePathBuffer, (uint)_texturePathBuffer.Length);
                string currentPath = Encoding.UTF8.GetString(_texturePathBuffer).TrimEnd('\0');
                // Dosya tarayıcıyı aç
                if (ImGui.Button("Select Texture"))
                    isFileBrowserOpen = true;
    
                // Texture ekleme butonu
                if (ImGui.Button("Add Texture") && !string.IsNullOrEmpty(_selectedTexturePath))
                {
                    Texture2D texture = TextureManager.loadTexture(_selectedTexturePath);
                    if (texture.Width > 0)
                    {
                        tileButtons.Add(new tilebuttons
                        {
                            name = Path.GetFileNameWithoutExtension(_selectedTexturePath),
                            path = _selectedTexturePath,
                            texture = texture,
                            PreviewSize = new Vector2(64, 64)
                        });
                    }
                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        ImGui.Text($"Selected Texture: {currentPath}");
                        if (File.Exists(currentPath))
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), "✓ Valid Texture");
                            _selectedTexturePath = currentPath;
                        }
                        else
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "✗ Invalid Texture");
                        }
                    }
                    
                }
                ImGui.BeginChild("Tiles");
                foreach (var button in tileButtons)
                {
                    
                    if (rlImGui.ImageButtonSize(button.name, button.texture, button.PreviewSize))
                    {
                        _selectedTexturePath = button.path;
                        Console.WriteLine(button.path);
                        
                    }
                    ImGui.SameLine();
                    if (ImGui.Button($"X##{button.name}"))
                        tileButtons.Remove(button);
                    
                    if (ImGui.IsItemHovered())
                            ImGui.SetTooltip(button.name);
                    
                    
                }
                
            }
            if (game.state == "enemy")
            {
                bool enemyProp = true;
                ImGui.InputText("Texture Path", _texturePathBuffer, (uint)_texturePathBuffer.Length);
                string currentPath = Encoding.UTF8.GetString(_texturePathBuffer).TrimEnd('\0');
                // Dosya tarayıcıyı aç
                if (ImGui.Button("Select Enemy"))
                    isFileBrowserOpen = true;
    
                // Texture ekleme butonu
                if (ImGui.Button("Add Enemy") && !string.IsNullOrEmpty(_selectedEnemyeTexturePath))
                {
                    Texture2D texture = TextureManager.loadTexture(_selectedEnemyeTexturePath);
                    if (texture.Width > 0)
                    {
                        enemyButtons.Add(new enemybuttons
                        {
                            name = Path.GetFileNameWithoutExtension(_selectedEnemyeTexturePath),
                            path = _selectedEnemyeTexturePath,
                            texture = texture,
                            PreviewSize = new Vector2(64, 64)
                        });
                    }
                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        ImGui.Text($"Selected Texture: {currentPath}");
                        if (File.Exists(currentPath))
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), "✓ Valid Texture");
                            _selectedEnemyeTexturePath = currentPath;
                        }
                        else
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "✗ Invalid Texture");
                        }
                    }
                    
                }
                ImGui.BeginChild("enemy");
                foreach (var button in enemyButtons)
                {
                    if (rlImGui.ImageButtonSize(button.name, button.texture, button.PreviewSize))
                    {
                        _selectedEnemyeTexturePath = button.path;
                        enemyProp = true;

                    }
                    if (enemyProp)
                    {
                        
                        float speed = button.enemySpeed;
                        float minDistance = button.enemyMinDistance;
                        ImGui.InputFloat("Speed", ref speed);
                        button.enemySpeed = speed;
                        ImGui.InputFloat("Minimum Distance", ref minDistance);
                        button.enemyMinDistance = minDistance;
                        
                    }
                    ImGui.SameLine();
                    if (ImGui.Button($"X##{button.name}"))
                        enemyButtons.Remove(button);
                    
                    if (ImGui.IsItemHovered())
                            ImGui.SetTooltip(button.name);
                    
                    
                }
                
            }
            if (game.state == "player")
            {
                ImGui.BeginChild("Player Props");
                
                ImGui.InputFloat("move Speed", ref game.player.MoveSpeed, 1,1);
                ImGui.InputFloat("Mouse Sens", ref game.player.MouseSensitivity, 1,1);
                ImGui.InputFloat("Jump Force", ref game.player.JumpForce, 1,1);
                ImGui.InputFloat("Gravity", ref game.player.Gravity, 1,1);
                if (rlImGui.ImageButtonSize("player texture", playerTextureOnGui, new Vector2(64,64)))
                {
                    isFileBrowserOpen = true;
                }
                
                
            }
            if (game.state == "door")
            {
                ImGui.InputText("Texture Path", _texturePathBuffer, (uint)_texturePathBuffer.Length);
                string currentPath = Encoding.UTF8.GetString(_texturePathBuffer).TrimEnd('\0');
                // Dosya tarayıcıyı aç
                if (ImGui.Button("Select Texture"))
                    isFileBrowserOpen = true;
        
                    // Texture ekleme butonu
                if (ImGui.Button("Add Texture") && !string.IsNullOrEmpty(_selectedTexturePath))
                {
                    Texture2D texture = TextureManager.loadTexture(_selectedTexturePath);
                    if (texture.Width > 0)
                    {
                        doorButtons.Add(new doorbuttons
                        {
                            name = Path.GetFileNameWithoutExtension(_selectedTexturePath),
                            path = _selectedTexturePath,
                            texture = texture,
                            PreviewSize = new Vector2(64, 64)
                        });
                    }
                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        ImGui.Text($"Selected Texture: {currentPath}");
                        if (File.Exists(currentPath))
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), "✓ Valid Texture");
                            _selectedTexturePath = currentPath;
                        }
                        else
                        {
                            ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "✗ Invalid Texture");
                        }
                    }
                    
                }
                ImGui.BeginChild("Doors");
                foreach (var button in doorButtons)
                {
                        
                    if (rlImGui.ImageButtonSize(button.name, button.texture, button.PreviewSize))
                    {
                        _selectedTexturePath = button.path;
                        Console.WriteLine(button.path);
                        
                    }
                    ImGui.SameLine();
                    if (ImGui.Button($"X##{button.name}"))
                        doorButtons.Remove(button);
                        
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip(button.name);
                }
                
                ImGui.EndChild();
            }
            ImGui.End();

            if (isFileBrowserOpen)
            {
                bool fileSelected = _fileBrowser.Draw();
                if (fileSelected)
                {
                    if (_fileBrowser.SelectedPath.Contains(".json"))
                    {
                       
                        switch (mapSelection)
                        {
                            case mapSelectionEnum.Builder:
                                mapBuildParser.MapData.mapListDatas.Add(new mapListData{
                                    mapIndex = currentMapIndex += 1,
                                    mapName = _fileBrowser.SelectedPath,
                                    mapPath = _fileBrowser.SelectedPath
                                });
                                
                                mapBuidWriter mapBuidWriter = new mapBuidWriter();
                                mapBuidWriter.WriteBuildMap(mapBuildParser.MapData);
                                return;
                            case mapSelectionEnum.Loader:
                                game.map.JsonPath = _fileBrowser.SelectedPath;
                                return;
                        }
                        
                    }
                    if (game.state == "cube")
                    {
                        if (_fileBrowser.SelectedPath.Contains(".png"))
                        {
                            _selectedTexturePath = _fileBrowser.SelectedPath;
                        }
                    }
                    if (game.state == "door")
                    {
                        if (_fileBrowser.SelectedPath.Contains(".png"))
                        {
                            _selectedTexturePath = _fileBrowser.SelectedPath;
                        }
                    }
                    if (game.state == "enemy")
                    {
                        if (_fileBrowser.SelectedPath.Contains(".png"))
                        {
                            _selectedEnemyeTexturePath = _fileBrowser.SelectedPath;
                        }
                    }
                    if (game.state == "player")
                    {
                        _selectedPlayerTexturePath = _fileBrowser.SelectedPath;
                        playerTextureOnGui = Raylib.LoadTexture(_selectedPlayerTexturePath);
                    }
                    isFileBrowserOpen = false;
                }
            }            
        }
        public static string GetSelectedTexturePath() => _selectedTexturePath;
    }

    public class tilebuttons{
        public string name { get; set; }
        public string path { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 PreviewSize { get; set; } = new Vector2(64, 64);
    }
    public class doorbuttons{
        public string name { get; set; }
        public string path { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 PreviewSize { get; set; } = new Vector2(64, 64);
    }
    public class enemybuttons{
        public string name { get; set; }
        public string path { get; set; }
        public float enemySpeed {get;set;}
        public float enemyMinDistance {get;set;}
        public Texture2D texture { get; set; }
        public Vector2 PreviewSize { get; set; } = new Vector2(64, 64);
    }
    
}