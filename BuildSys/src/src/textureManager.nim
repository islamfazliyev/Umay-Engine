import raylib
import tables

type
  TextureHandle = ptr Texture2D

var textureCache: Table[string, TextureHandle]

proc initTexture*(path: string): TextureHandle =
  # Return existing texture if cached
  if textureCache.hasKey(path):
    return textureCache[path]

  # Load new texture and store in heap memory
  let rawTexture = loadTexture(path)
  result = create(Texture2D)
  result[] = rawTexture
  textureCache[path] = result

proc getTexture*(path: string): TextureHandle =
  textureCache.getOrDefault(path)