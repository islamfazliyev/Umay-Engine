using game;
using Raylib_cs;
using System.ComponentModel;
using System.Numerics;


class enemy
{
    public Vector3 enemyPosition {get; set;}
    public enemyData Data { get; set; }
    public void Update(Vector3 playerPosition)
    {
        float distance = Vector3.Distance(playerPosition, enemyPosition);
        if (distance <= Data.minDistance)
        {
            Vector3 direction = Vector3.Normalize(playerPosition - enemyPosition);
            enemyPosition += direction * Data.speed * Raylib.GetFrameTime();
        }
    }

    public void Draw(float width, float height, float length)
    {
        Texture2D texture = TextureManager.loadTexture(Data.texture);
        draw.DrawCubeTexture(texture, enemyPosition, width, height, length, Color.White);
    }
}