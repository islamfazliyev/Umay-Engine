using game;
using Raylib_cs;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

class testEditor
{
    Camera3D camera = new Camera3D();

    Camera2D camera2D = new Camera2D();
    placeHolderProp placeHolder = new placeHolderProp();
    int gridSize;
    int currentZ;
    Color currentColor;
    public bool isPlayerPlaced;
    circleID ids = new circleID();
    renderProp propRender = new renderProp();
    List<circlePos> circles = new();
    List<lineProp> lines = new();
    List<renderLineToPos> rendering = new();
    bool render;

    public void Begin()
    {
        camera.Position = new Vector3(4.0f, 2.0f, 4.0f);
        camera.Target = new Vector3(0.0f, 1.8f, 0.0f);
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        camera.FovY = 60.0f;
        camera.Projection = CameraProjection.Perspective;
        CameraMode cameraMode = CameraMode.FirstPerson;
        render = false;
        camera2D.Target = new Vector2(0.0f, 0.0f);
        camera2D.Offset = new Vector2(0.0f, 0.0f);
        camera2D.Rotation = 0.0f;
        camera2D.Zoom = 1.0f;
        isPlayerPlaced = false;
        currentColor = Color.White;
        currentZ = -1;
        gridSize = 20;
        
    }

    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if(!render){render=true;}
            else{render=false;}
            Console.WriteLine(render);
        }
        if (!render)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DarkBlue);
            Raylib.BeginMode2D(camera2D);
            for (int x = 0; x < Raylib.GetScreenWidth(); x += gridSize)
            {
                Raylib.DrawLine(x, 0, x, 450, Color.Black);  
            }
            for (int y = 0; y < Raylib.GetScreenHeight(); y += gridSize)
            {
                Raylib.DrawLine(0, y, 800, y, Color.Black);
            }
            foreach (circlePos circlePos in circles)
            {
                Raylib.DrawCircle(circlePos.x, circlePos.y, 0, Color.White);
            }
            foreach (lineProp lineProp in lines)
            {
                Raylib.DrawLine(lineProp.startX, lineProp.startY, lineProp.endX, lineProp.endY, Color.White);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                addCircle();
            }
            Raylib.EndMode2D();
            Raylib.EndDrawing();
        }
        else{
            Raylib.UpdateCamera(ref camera, CameraMode.FirstPerson);
     
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Gray);
            Raylib.BeginMode3D(camera);
            foreach (renderLineToPos renderLine in rendering)
            {
                // Swap start and end points to correct rotation
                draw.DrawPlaneBetweenPoints(
                    new Vector3(-renderLine.endX, 0, -renderLine.endY), 
                    new Vector3(-renderLine.startX, 0, -renderLine.startY), 
                    0.5f, // Adjusted height for visibility
                    Color.DarkBrown
                );
            }
            //Raylib.DrawCube(new Vector3(-10, 0, -10), 10,10,10, Color.White);
            Raylib.EndMode3D();
            Raylib.EndDrawing();
        }

    }

    public void addCircle()
    {
        var mousePos = Raylib.GetMousePosition();
        
        circlePos circlePos = new circlePos{
            x = (int)mousePos.X,
            y = (int)mousePos.Y
        };
        ids.id += 1;
        circles.Add(circlePos);
        if (circles.Count > 1)
        {
            circlePos previousCirclePos = circles[circles.Count - 2];

            lineProp lineProp = new lineProp{
                startX = previousCirclePos.x,
                startY = previousCirclePos.y,
                endX = circlePos.x,
                endY = circlePos.y,
                
            };
            renderLineToPos renderLineTo = new renderLineToPos{
                // Scale down by gridSize and invert coordinates
                startX = previousCirclePos.x / (float)gridSize,
                startY = previousCirclePos.y / (float)gridSize,
                endX = circlePos.x / (float)gridSize,
                endY = circlePos.y / (float)gridSize,
            };
            lines.Add(lineProp);
            rendering.Add(renderLineTo);
        }
    }
}

class circleID
{
    public int id {get;set;}
    public List<circlePos> CirclePos = new List<circlePos>();
    public List<lineProp> lineProps = new List<lineProp>();
}

class circlePos
{
    public int x {get;set;}
    public int y {get;set;}
}
class lineProp
{
    public int startX {get;set;}
    public int startY {get;set;}
    public int endX {get; set;}
    public int endY {get; set;}
}

class renderProp
{
    public List<renderLineToPos> renderPos = new List<renderLineToPos>();
}

class renderLineToPos
{
    public float startX {get;set;}
    public float startY {get;set;}
    public float endX {get;set;}
    public float endY {get;set;}
}