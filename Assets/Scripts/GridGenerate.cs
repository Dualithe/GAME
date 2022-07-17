using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk
{




};

public class GridGenerate : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject[] _listOfObjects;

    [SerializeField] private HashSet<Vector2> chunks = new HashSet<Vector2>();

    [SerializeField] private Dictionary<Vector2, GameObject> walls = new Dictionary<Vector2, GameObject>();

    [SerializeField] private Dictionary<Vector4, GameObject> mimics = new Dictionary<Vector4, GameObject>();

    [SerializeField] private float[] _howManyObjects;

    [SerializeField] private GameObject parentOfGrid;

    [SerializeField] private GameObject parentOfWall;

    [SerializeField] private GameObject parentOfStruct;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject wall;

    [SerializeField] private GameObject mimicPrefab;

    [SerializeField] private Vector2 chunkSize;

    [SerializeField] private ChunkGenData firstChunk;

    private Vector2 tileSize;

    private ChunkNeighboursProvider cnp = new ChunkNeighboursProvider();

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        tileSize = (_tilePrefab.GetComponent<SpriteRenderer>().size * _tilePrefab.transform.localScale);
        GenerateGrid(0, 0);
        chunks.Add(new Vector2(0, 0));
        var firstTile = _tiles[new Vector2(0, 0)];
        var lastTile = _tiles[new Vector2(_width - 1, _height - 1)];
        chunkSize = firstTile.transform.position - lastTile.transform.position;
        chunkSize.x = Mathf.Abs(chunkSize.x);
        chunkSize.y = Mathf.Abs(chunkSize.y);
        chunkSize += tileSize;
        generateStructures(0, 0, firstChunk);
        spawnPlayer();
        placeWalls(0, 0);
        spawnMimics(0, 0, firstChunk);
    }
    void GenerateGrid(int offX, int offY)
    {
        offX *= _width;
        offY *= _height;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3((x + offX)*tileSize.x, (y + offY)*tileSize.y), Quaternion.identity, parentOfGrid.transform);
                spawnedTile.name = $"Tile {x}.{y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                spawnedTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
                _tiles[new Vector2(x + offX, y + offY)] = spawnedTile;

            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }


    public Tile GetTileAtPos(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    void generateStructures(int offX, int offY, ChunkGenData chunkData)
    {
        var structures = chunkData.structures.OrderBy(s => s.structurePrefab.GetComponent<Structure>().priority).ToList();
        foreach (var structure in structures)
        {
            int amount = UnityEngine.Random.Range(structure.minAmount, structure.maxAmount);
            var typeOf = structure.structurePrefab;
            createObject(typeOf, amount, offX, offY, structure.structurePrefab.GetComponent<Structure>().radius);
        }
    }

    List<Vector2> getAllEmptyTiles(int offX, int offY)
    {
        var listOfTiles = new List<Vector2>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var k = GetTileAtPos(new Vector2(x + offX * _width, y + offY * _height));
                if (k != null && k.getVacant())
                {
                    listOfTiles.Add(new Vector2(x, y));
                }
            }
        }
        return listOfTiles;
    }
    void createObject(GameObject typeOf, float ammount, int offX, int offY, int radius)
    {
        for (int i = 0; i < ammount; i++)
        {
            var vacantCoords = getAllEmptyTiles(offX, offY).Where(v => v.x > radius - 1 && v.y > radius - 1 && v.x < _width - radius && v.y < _height - radius).ToList();
            var myCoords = vacantCoords[UnityEngine.Random.Range(0, vacantCoords.Count - 1)];
            var currObj = Instantiate(typeOf, new Vector3((myCoords.x + offX * _width)*tileSize.x, (myCoords.y + offY * _height) * tileSize.y - tileSize.y*0.6f, 0), Quaternion.identity, parentOfStruct.transform);
            currObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetTileAtPos(myCoords).setVacant(false);
        }
    }

    void spawnPlayer()
    {
        var pl = Instantiate(player, new Vector3(_cam.position.x, _cam.position.y, 0f), Quaternion.identity);
        pl.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void placeWalls(int offX, int offY)
    {
        Vector2[] xD ={new Vector2(1,0),
        new Vector2(0,1),
        new Vector2(1,1),
        new Vector2(-1,0),
        new Vector2(-1,-1),
        new Vector2(0,-1),
        new Vector2(1,-1),
        new Vector2(-1,1)
        };
        foreach (Vector2 off in xD)
        {
            var lol = new Vector2(offX + off.x, offY + off.y);
            if (!chunks.Contains(lol) && !walls.ContainsKey(lol))
            {
                var w = Instantiate(wall, transform.position + (Vector3)(lol * chunkSize + chunkSize / 2 - tileSize / 2), Quaternion.identity, parentOfWall.transform);
                w.GetComponent<BoxCollider2D>().size = chunkSize;
                walls.Add(lol, w);
            }
        }
    }

    public void buyLand(int dirX, int dirY, ChunkGenData chunkData)
    {
        GenerateGrid(dirX, dirY);
        chunks.Add(new Vector2(dirX, dirY));
        generateStructures(dirX, dirY, chunkData);
        placeWalls(dirX, dirY);
        if (walls.ContainsKey(new Vector2(dirX, dirY)))
        {
            Destroy(walls[new Vector2(dirX, dirY)].gameObject);
            walls.Remove(new Vector2(dirX, dirY));
        }
        spawnMimics(dirX, dirY, chunkData);
    }

    private void spawnMimics(int dirX, int dirY, ChunkGenData chunkData)
    {
        Vector2[] xD ={new Vector2(1,0),
        new Vector2(0,1),
        new Vector2(-1,0),
        new Vector2(0,-1),
        };
        var chunkIdx = new Vector2(dirX, dirY);
        int leftChunkKey = Cantor(chunkIdx);

        List<MimicLogic> newMimics = new List<MimicLogic>();

        foreach (Vector2 off in xD)
        {
            int rightChunkKey = Cantor(chunkIdx + off);
            Vector2 mimicKey = leftChunkKey < rightChunkKey ? new Vector2(leftChunkKey, rightChunkKey) : new Vector2(rightChunkKey, leftChunkKey);
            if (!mimics.ContainsKey(mimicKey))
            {
                var w = Instantiate(mimicPrefab, transform.position + (Vector3)(chunkIdx * chunkSize + chunkSize / 2 - tileSize / 2 + off * chunkSize / 2), Quaternion.identity, parentOfWall.transform);
                mimics.Add(mimicKey, w);
                newMimics.Add(w.GetComponent<MimicLogic>());
                w.GetComponent<MimicLogic>().chunkIdx = chunkIdx + off;
            }
            else
            {
                Destroy(mimics[mimicKey].gameObject);
                mimics.Remove(mimicKey);
            }
        }
        var neighbours = cnp.PushChunkAndGetPossibleNeighbours(chunkData, newMimics.Count);
        for (int i = 0; i < newMimics.Count; i++)
        {
            newMimics[i].SetChunkToUnlock(neighbours[i]);
        }
    }

    private int Cantor(Vector2 vec)
    {
        int a = (int)vec.x + 200;
        int b = (int)vec.y + 200;
        return (a + b) * (a + b + 1) / 2 + a;
    }

    public Vector2 getTileSize()
    {
        return tileSize;
    }
}
