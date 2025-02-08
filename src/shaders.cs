using Raylib_cs;
using System.Numerics;

class shaders
{
    public Shader vhsShader;
    public Texture2D noiseTexture;
    public RenderTexture2D renderTexture;

    public void LoadResources()
    {
        vhsShader = Raylib.LoadShader(null, "shaders/shader.fs");
        noiseTexture = Raylib.LoadTexture("assets/noise.png");
        renderTexture = Raylib.LoadRenderTexture(800, 450);
        
        // Set initial uniform values
        int resLoc = Raylib.GetShaderLocation(vhsShader, "resolution");
        Raylib.SetShaderValue(vhsShader, resLoc, 
            new Vector2(800, 450), ShaderUniformDataType.Vec2);
    }

    public void UnloadResources()
    {
        Raylib.UnloadShader(vhsShader);
        Raylib.UnloadTexture(noiseTexture);
        Raylib.UnloadRenderTexture(renderTexture);
    }
}