import jsony, std/os
import raylib
import drawsys
import textureManager

# Define a structure matching the JSON format
type
  Tile = object
    x: int
    y: int
    z: int
    texture: string
    color: string

  MapData = object
    gridWidth: int
    gridHeight: int
    gridDepth: int
    tileSize: int
    tiles: seq[Tile]


let jsonString = readFile("src/firstmap.json")


let mapData = jsonString.fromJson(MapData)
when isMainModule:
  initWindow(800, 450, "a")
  var camera: Camera3D = Camera3D(
    position: Vector3(x: 20, y: 10, z: 0),
    target: Vector3(x: 0, y: 1, z: 0),
    up: Vector3(x: 0, y: 1, z: 0),
    fovY: 60
  )
  let cameraMode: CameraMode = FirstPerson
  for tile in mapData.tiles:
    discard initTexture(tile.texture)

  
  while not windowShouldClose():
    updateCamera(camera, cameraMode)
    hideCursor()
    enableCursor()
    beginDrawing()
    clearBackground(Black)
    beginMode3D(camera)
    for tile in mapData.tiles:
      let texturePtr = initTexture(tile.texture)
      var position: Vector3 = Vector3(
        x: float32(-tile.x) * float32(mapData.tileSize), 
        y: float32(tile.z) * float32(mapData.tileSize), 
        z: float32(-tile.y) * float32(mapData.tileSize))
      drawCubeTexture(texturePtr[], position, float32(mapData.tileSize), float32(mapData.tileSize), float32(mapData.tileSize), White)
    endMode3D()
    endDrawing()
    # echo "Tile at (", tile.x, ", ", tile.y, ", ", tile.z, ") - Type: ", tile.type, " Texture: ", tile.texture, " Color: ", tile.color
