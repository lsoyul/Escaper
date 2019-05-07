using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class MapGenerator : MonoBehaviour
{
    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefab;
    public Vector2 maxMapSize;

    [Range(0,1)]
    public float outlinePercent;

    public float tileSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    private Map currentMap;


    // Callbacks
    public Action onFinishGenerateMap;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];
        System.Random prng = new System.Random(currentMap.seed);

        GetComponent<BoxCollider>().size = new Vector3(currentMap.mapSize.x * tileSize, .05f, currentMap.mapSize.y * tileSize);

        // Generating coords
        allTileCoords = new List<Coord>();

        // Set Coordinates of all tiles
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), currentMap.seed));

        // Create map holder object
        string holderName = "Generated Map";

        if (transform.Find(holderName)){
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = this.transform;

        // Spawning Tiles
        for (int x = 0; x < currentMap.mapSize.x; x++){
            for (int y = 0; y < currentMap.mapSize.y; y++){
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;

                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;

                if (x == currentMap.startPoint.x && y == currentMap.startPoint.y)
                {
                    Renderer tileRenderer = newTile.GetComponent<Renderer>();
                    Material tileMaterial = new Material(tileRenderer.sharedMaterial);
                    tileMaterial.color = currentMap.startPointColor;
                    tileRenderer.sharedMaterial = tileMaterial;
                }

                if (x == currentMap.finishPoint.x && y == currentMap.finishPoint.y)
                {
                    Renderer tileRenderer = newTile.GetComponent<Renderer>();
                    Material tileMaterial = new Material(tileRenderer.sharedMaterial);
                    tileMaterial.color = currentMap.finishPointColor;
                    tileRenderer.sharedMaterial = tileMaterial;
                }
            }
        }

        // Spawning Obstacles
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

        int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
        int currentObstacleCount = 0;

        for (int i=0; i< obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (
            !randomCoord.Equals(currentMap.startPoint) 
            && !randomCoord.Equals(currentMap.finishPoint) 
            && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)prng.NextDouble());

                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3( (1 - outlinePercent) * tileSize, obstacleHeight, (1-outlinePercent) * tileSize);

                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colourPercent = randomCoord.y / (float)currentMap.mapSize.y;
                obstacleMaterial.color = Color.Lerp(currentMap.foreGroundColour, currentMap.backGroundColour, colourPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }

        }

        // Creating navmesh mask
        Transform maskLeft 
            = Instantiate(navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        Transform maskRight
            = Instantiate(navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        Transform maskTop
            = Instantiate(navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        Transform maskBottom
            = Instantiate(navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;

        // Finish To Generate Map
        if (onFinishGenerateMap != null) onFinishGenerateMap();
    }

    // Flood Fill Algorithm
    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(currentMap.startPoint);

        mapFlags[currentMap.startPoint.x, currentMap.startPoint.y] = true;
        //mapFlags[currentMap.finishPoint.x, currentMap.finishPoint.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    // 인접한 타일들 중, 대각선에 위치한 타일은 제외하고 순회
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) 
                            && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);

        return targetAccessibleTileCount == accessibleTileCount;
    }

    public Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
    }

    public Coord positionToCoord(Vector3 pos)
    {
        int x = (int)((pos.x + currentMap.mapSize.x / 2f * tileSize) / tileSize);
        int z = (int)((pos.z + currentMap.mapSize.y / 2f * tileSize) / tileSize);

        return new Coord(x, z);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Map GetCurrentMap()
    {
        return currentMap;
    }

    public void LoadNextStage()
    {
        mapIndex++;
        if (mapIndex >= maps.Length) mapIndex = 0;

        GenerateMap();
    }

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coord))
            {
                return false;
            }

            var coord = (Coord)obj;
            return x == coord.x &&
                   y == coord.y;
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return (c1.x == c2.x) && (c1.y == c2.y);
        }

        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }

    [System.Serializable]
    public class Map
    {
        public Coord mapSize;
        [Range(0,1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foreGroundColour;
        public Color backGroundColour;

        public Coord startPoint;
        public Coord finishPoint;
        public Color startPointColor;
        public Color finishPointColor;
        //public Coord startPoint
        //{
        //    get { return new Coord(mapSize.x / 2, mapSize.y / 2); }
        //}
    }
}
