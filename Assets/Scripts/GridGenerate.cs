using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerate : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject[] _listOfObjects;

    [SerializeField] private float[] _howManyObjects;

    [SerializeField] private GameObject parentOfGrid;

    [SerializeField] private GameObject parentOfStruct;

    [SerializeField] private GameObject player;


    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
        generateStructures();
        spawnPlayer();
    }
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, parentOfGrid.transform);
                spawnedTile.name = $"Tile {x}{y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                spawnedTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
                _tiles[new Vector2(x, y)] = spawnedTile;

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

    void generateStructures()
    {
        for (int i = 0; i < _listOfObjects.Length; i++)
        {
            var ammount = _howManyObjects[i];
            var typeOf = _listOfObjects[i];
            createObject(typeOf, ammount);
        }
    }

    List<Vector2> getAllEmptyTiles()
    {
        var listOfTiles = new List<Vector2>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (GetTileAtPos(new Vector2(x, y)).getVacant())
                {
                    listOfTiles.Add(new Vector2(x, y));
                }
            }
        }
        return listOfTiles;
    }
    void createObject(GameObject typeOf, float ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            var vacantCoords = getAllEmptyTiles();
            var myCoords = vacantCoords[Random.Range(0,vacantCoords.Count-1)];
            var currObj = Instantiate(typeOf, new Vector3(myCoords.x, myCoords.y, 0), Quaternion.identity, parentOfStruct.transform);
            currObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetTileAtPos(myCoords).setVacant(true);
        }
    }

    void spawnPlayer()
    {
        var pl = Instantiate(player, new Vector3(_cam.position.x, _cam.position.y, 0f), Quaternion.identity);
        pl.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
