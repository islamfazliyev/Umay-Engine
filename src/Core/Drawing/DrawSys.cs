using System.Numerics;
using Raylib_cs;

namespace game{
    class draw{
        public static void DrawCubeTexture(
            Texture2D texture,
            Vector3 position,
            float width,
            float height,
            float length,
            Color color
        )
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;

            // Set desired texture to be enabled while drawing following vertex data
            Rlgl.SetTexture(texture.Id);

            // Vertex data transformation can be defined with the commented lines,
            // but in this example we calculate the transformed vertex data directly when calling Rlgl.Vertex3f()

            Rlgl.Begin(DrawMode.Quads);
            Rlgl.Color4ub(color.R, color.G, color.B, color.A);

            // Front Face
            // Normal Pointing Towards Viewer
            Rlgl.Normal3f(0.0f, 0.0f, 1.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);

            // Back Face
            // Normal Pointing Away From Viewer
            Rlgl.Normal3f(0.0f, 0.0f, -1.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);

            // Top Face
            // Normal Pointing Up
            Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);

            // Bottom Face
            // Normal Pointing Down
            Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);

            // Right face
            // Normal Pointing Right
            Rlgl.Normal3f(1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);

            // Left Face
            // Normal Pointing Left
            Rlgl.Normal3f(-1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.End();
            Rlgl.SetTexture(0);
        }

        public static void DrawPlaneBetweenPoints(Vector3 point1, Vector3 point2, float height, Color color)
        {
            // Calculate midpoint
            Vector3 midpoint = new Vector3(
                (point1.X + point2.X) / 2,
                (point1.Y + point2.Y) / 2,
                (point1.Z + point2.Z) / 2
            );

            // Calculate distance
            float width = Vector3.Distance(point1, point2);

            // Calculate angle (only for 2D rotation on the X-Z plane)
            float angle = (float)Math.Atan2(point2.Z - point1.Z, point2.X - point1.X);

            Rlgl.PushMatrix();
            Rlgl.Translatef(midpoint.X, midpoint.Y, midpoint.Z);
            Rlgl.Rotatef(angle * (180.0f / (float)Math.PI), 0.0f, 1.0f, 0.0f);

            Rlgl.Begin(DrawMode.Quads);
            Rlgl.Color4ub(color.R, color.G, color.B, color.A);

            // Front face
            Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
            Rlgl.Vertex3f(-width / 2, -height / 2, 0);
            Rlgl.Vertex3f(width / 2, -height / 2, 0);
            Rlgl.Vertex3f(width / 2, height / 2, 0);
            Rlgl.Vertex3f(-width / 2, height / 2, 0);

            // Back face
            Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
            Rlgl.Vertex3f(-width / 2, -height / 2, 0);
            Rlgl.Vertex3f(-width / 2, height / 2, 0);
            Rlgl.Vertex3f(width / 2, height / 2, 0);
            Rlgl.Vertex3f(width / 2, -height / 2, 0);

            Rlgl.End();
            Rlgl.PopMatrix();
        }
        
    }
    
    
}