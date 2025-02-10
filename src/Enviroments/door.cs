using System.Numerics;
using game;
using Raylib_cs;

class Door
{
    public Texture2D texture;
    public Vector3 position;
    public float rotation = 0f; // Kapının dönüş açısı
    public bool isOpening = false;

    public Door(Texture2D texture, Vector3 position)
    {
        this.texture = texture;
        this.position = position;
    }

    public void Update()
    {
        
        // Sağ ok tuşuna basıldığında 90 derece döndür
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            rotation += 90f;
            if (rotation >= 360f) rotation = 0f; // 360 dereceyi geçince sıfırla
        }
    }

    public void Draw()
    {
        // Kapıyı döndür ve çiz
        Rlgl.PushMatrix();
        Rlgl.Translatef(position.X, position.Y, position.Z);
        Rlgl.Rotatef(rotation, 0, 1, 0);
        draw.DrawCubeTexture(texture, Vector3.Zero, 10, 10, 5, Color.RayWhite);
        Rlgl.PopMatrix();
    }
    
}