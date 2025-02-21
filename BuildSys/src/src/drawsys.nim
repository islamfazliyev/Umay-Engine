from raylib import PixelFormat, TextureFilter, BlendMode, ShaderLocationIndex,
  ShaderUniformDataType, ShaderAttributeDataType, MaxShaderLocations, ShaderLocation,
  Matrix, Vector2, Vector3, Color, ShaderLocsPtr
export PixelFormat, TextureFilter, BlendMode, ShaderLocationIndex, ShaderUniformDataType,
  ShaderAttributeDataType, MaxShaderLocations, ShaderLocation, Matrix, Vector2, Vector3,
  Color, ShaderLocsPtr
import raylib
import rlgl

proc drawCubeTexture*(texture: Texture2D, position: Vector3, width, height, length: float, color: Color) =
  let
    x = position.x
    y = position.y
    z = position.z

  setTexture(texture.id)
  
  rlBegin(DrawMode.Quads)
  color4ub(color.r, color.g, color.b, color.a)

  # Front face
  normal3f(0.0f, 0.0f, 1.0f)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x - width/2, y - height/2, z + length/2)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x + width/2, y - height/2, z + length/2)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x + width/2, y + height/2, z + length/2)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x - width/2, y + height/2, z + length/2)

  # Back face
  normal3f(0.0f, 0.0f, -1.0f)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x - width/2, y - height/2, z - length/2)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x - width/2, y + height/2, z - length/2)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x + width/2, y + height/2, z - length/2)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x + width/2, y - height/2, z - length/2)

  # Top face
  normal3f(0.0f, 1.0f, 0.0f)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x - width/2, y + height/2, z - length/2)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x - width/2, y + height/2, z + length/2)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x + width/2, y + height/2, z + length/2)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x + width/2, y + height/2, z - length/2)

  # Bottom face
  normal3f(0.0f, -1.0f, 0.0f)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x - width/2, y - height/2, z - length/2)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x + width/2, y - height/2, z - length/2)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x + width/2, y - height/2, z + length/2)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x - width/2, y - height/2, z + length/2)

  # Right face
  normal3f(1.0f, 0.0f, 0.0f)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x + width/2, y - height/2, z - length/2)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x + width/2, y + height/2, z - length/2)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x + width/2, y + height/2, z + length/2)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x + width/2, y - height/2, z + length/2)

  # Left face
  normal3f(-1.0f, 0.0f, 0.0f)
  texCoord2f(0.0f, 0.0f)
  vertex3f(x - width/2, y - height/2, z - length/2)
  texCoord2f(1.0f, 0.0f)
  vertex3f(x - width/2, y - height/2, z + length/2)
  texCoord2f(1.0f, 1.0f)
  vertex3f(x - width/2, y + height/2, z + length/2)
  texCoord2f(0.0f, 1.0f)
  vertex3f(x - width/2, y + height/2, z - length/2)

  rlEnd()
  setTexture(0)