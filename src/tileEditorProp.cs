using System.Numerics;
using Raylib_cs;


public class placeHolderProp
{
    public Vector2 position;
    public Raylib_cs.Color color = Raylib_cs.Color.Red;
    public int width = 10;
    public int height = 10;

    public Vector2 GetSnappedPosition(int gridSize)
    {
        return new Vector2(
            (int)position.X / gridSize,
            (int)position.Y / gridSize
        );
        
        
    }
}

public class placedCubeProps
{
    public Vector3 position;
    public Raylib_cs.Color color = Raylib_cs.Color.Red;
    public int width = 10;
    public int height = 10;
    public int depth = 10;

    public Vector3 GetSnappedPosition(int gridSize)
    {
        return new Vector3(
            (int)position.X / gridSize,
            (int)position.Y / gridSize,
            (int)position.Z / gridSize
        );
        
        
    }
}