using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

public class GridMapData
{
    public int gridWidth { get; set; }
    public int gridHeight { get; set; }
    public int gridDepth { get; set; } // Yeni
    public int tileSize { get; set; }
    public List<TileData> tiles { get; set; }
    public List<playerData> playerDatas { get; set; }
    public List<DoorData> doors { get; set; }
    public List<enemyData> enemyDatas { get; set; }
}

public class TileData
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public string type { get; set; }
    public string texture { get; set; }
    
    [JsonConverter(typeof(ColorNameConverter))]
    public Color color { get; set; }

}

public class DoorData
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public string icon {get; set;}
    public string texture { get; set; }
    public Color color { get; set; }
}

public class playerData
{
    public int x {get; set;}
    public int y {get; set;}
    public int z {get; set;}
    public string icon {get; set;}
    public string playerTexture {get;set;}
    public bool placed {get; set;} = true;
}

public class enemyData
{
    public int x {get; set;}
    public int y {get; set;}
    public int z {get; set;}
    public float speed {get; set;}
    public float minDistance {get; set;}
    public string texture {get;set;}
}

public static class TextureManager
{
    private static Dictionary<string, Texture2D> _textures = new();
    public static Texture2D loadTexture(string path)
    {
        if (_textures.TryGetValue(path, out Texture2D existingTexture))
        {
            return existingTexture;
        }
        if (_textures.ContainsKey(path))
        {
            return _textures[path];
        }
        Texture2D texture = Raylib.LoadTexture(path);
        _textures.Add(path, texture);
        return texture;
    }

    public static void unloadTextures()
    {
        foreach (var texture in _textures.Values)
        {
            Raylib.UnloadTexture(texture);
        }
        _textures.Clear();
    }
}

public class ColorNameConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string colorName = reader.GetString().ToUpper();
        return colorName switch
        {
            "DARKGREEN" => Color.DarkGreen,
            "GRAY" => Color.Gray,
            "RED" => Color.Red,
            _ => Color.White
        };
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStringValue("WHITE");
    }
}

public class mapParser
{
    public GridMapData MapData { get; private set; }
    private string jsonPath = "assets/mapData.json";
    public string JsonPath
    {
        get => jsonPath;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value Cannot Be Null", nameof(value));
            }
            jsonPath = value;
            LoadMapData();
            
        }
    } 
    public mapParser()
    {
        LoadMapData();
        try
        {
            string jsonContent = File.ReadAllText(JsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MapData = JsonSerializer.Deserialize<GridMapData>(jsonContent, options);
            Console.WriteLine(JsonPath);
            
            if (MapData?.tiles == null)
            {
                Console.WriteLine("JSON içeriği boş!");
                //MapData = new MapData { map = new mapProp() };
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("Dosya bulunamadı: " + ex.Message);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("JSON okuma hatası: " + ex.Message);
        }
    }
    private void LoadMapData()
    {
        try
        {
            string jsonContent = File.ReadAllText(JsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MapData = JsonSerializer.Deserialize<GridMapData>(jsonContent, options);
            Console.WriteLine(JsonPath);
            Console.WriteLine($"Loaded map data from {JsonPath}");
            Console.WriteLine($"Number of tiles loaded: {MapData?.tiles?.Count ?? 0}");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found: " + ex.Message);
            MapData = new GridMapData { tiles = new List<TileData>() }; // Boş map oluştur
        }
        catch (JsonException ex)
        {
            Console.WriteLine("JSON parsing error: " + ex.Message);
            MapData = new GridMapData { tiles = new List<TileData>() }; // Boş map oluştur
        }
    }
}



public class mapWriter()
{
    public void WriteMap(GridMapData mapData)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonContent = JsonSerializer.Serialize(mapData, options);
            File.WriteAllText("assets/mapData.json", jsonContent);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("JSON yazma hatası: " + ex.Message);
        }
    }
}

