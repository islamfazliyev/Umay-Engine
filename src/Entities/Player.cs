using System.Dynamic;
using System.Numerics;
using game;
using Raylib_cs;

public class Player
{
    public float MoveSpeed = 5.0f;
    public float MouseSensitivity = 0.003f;
    public float JumpForce = 7.0f;
    public float Gravity = 20.0f;
    
    // Oyuncu Durumu
    public Vector3 _position;
    public Vector3 StartPosition;
    private Vector2 _cameraAngle = new Vector2(0, 0);
    private Vector3 _velocity = Vector3.Zero;
    private bool _isGrounded = true;
    // Kamera
    public Camera3D _camera = new Camera3D();
    
    public void Reload()
    {
        if (game.game.map.MapData?.playerDatas != null && game.game.map.MapData.playerDatas.Count > 0)
        {
            // İlk playerData'yı al
            var playerData = game.game.map.MapData.playerDatas[0];
            
            // Tile boyutunu ve grid sistemini dikkate al
            float tileSize = game.game.map.MapData.tileSize;
            
            // 3D dünya koordinatlarını hesapla
            _position = new Vector3(
                playerData.x * tileSize, 
                playerData.z * tileSize, // Zemin yüksekliği
                playerData.y * tileSize
            );

            // Kamerayı doğru pozisyona yerleştir
            _camera.Position = _position + new Vector3(0, 1.8f, 0); // Baş yüksekliği
            _camera.Target = _position + Vector3.UnitZ; // İlk bakış yönü
        }
        else
        {
            // Varsayılan pozisyon
            _position = new Vector3(0, 2.0f, 0);
            _camera.Position = _position + new Vector3(0, 1.8f, 0);
            _camera.Target = _position + Vector3.UnitZ;
        }
        
        
        _camera.Up = Vector3.UnitY;
        _camera.FovY = 60;
        _camera.Projection = CameraProjection.Perspective;
    }
    public void Update(float deltaTime)
    {
        // Fare ile kamera kontrolü
        var mouseDelta = Raylib.GetMouseDelta();
        _cameraAngle.X += mouseDelta.X * MouseSensitivity;
        _cameraAngle.Y = Math.Clamp(_cameraAngle.Y - mouseDelta.Y * MouseSensitivity, -MathF.PI/2 + 0.1f, MathF.PI/2 - 0.1f);

        // Kamerayı güncelle
        Vector3 front = new Vector3(
            MathF.Cos(_cameraAngle.X) * MathF.Cos(_cameraAngle.Y),
            MathF.Sin(_cameraAngle.Y),
            MathF.Sin(_cameraAngle.X) * MathF.Cos(_cameraAngle.Y)
        );
        front = Vector3.Normalize(front);
        _camera.Target = _camera.Position + front;

        // Hareket vektörü
        Vector3 moveDir = Vector3.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.W)) moveDir += front;
        if (Raylib.IsKeyDown(KeyboardKey.S)) moveDir -= front;
        if (Raylib.IsKeyDown(KeyboardKey.D)) moveDir += Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        if (Raylib.IsKeyDown(KeyboardKey.A)) moveDir -= Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        
        // Yatay hareket (Y eksenini sıfırla)
        moveDir.Y = 0;
        if (moveDir != Vector3.Zero)
            moveDir = Vector3.Normalize(moveDir) * MoveSpeed;

        // Yerçekimi ve zıplama
        if (!_isGrounded)
            _velocity.Y -= Gravity * deltaTime;
        
        if (_isGrounded && Raylib.IsKeyPressed(KeyboardKey.Space))
            _velocity.Y = JumpForce;

        // Pozisyon güncelleme
        _position += (moveDir + _velocity) * deltaTime;
        
        // Basit zemin kontrolü (Yükseklik 2'de sabit)
        if (_position.Y < 2.0f)
        {
            _position.Y = 2.0f;
            _velocity.Y = 0;
            _isGrounded = true;
        }
        else
            _isGrounded = false;

        // Kamerayı oyuncu pozisyonuna bağla
        _camera.Position = _position;
    }
}