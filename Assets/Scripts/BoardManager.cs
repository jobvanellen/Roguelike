using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool isPassable;
        public CellObject ContainedObject;
    }

    private CellData[,] m_BoardData;
    private Tilemap m_TileMap;
    private Grid m_Grid;
    private List<Vector2Int> m_EmptyCells;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] BlockingTiles;

    public PlayerController Player;
    public ExitCellObject Exit;
    
    public List<CellObject> FoodPrefabs;
    public List<CellObject> WallPrefabs;
    public CellObject EnemyPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        m_Grid = GetComponentInChildren<Grid>();
        m_TileMap = GetComponentInChildren<Tilemap>();

        m_EmptyCells = new List<Vector2Int>();
        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                DrawBoardTile(x, y);
                InitializeCellData(x, y);

                if (!IsEdgeTile(x, y))
                {
                    m_EmptyCells.Add(new Vector2Int(x, y));
                }
            }
        }

        m_EmptyCells.Remove(new Vector2Int(1, 1)); // player location

        AddEndCellObject(new Vector2Int(Width - 2, Height - 2));

        GenerateObjects(WallPrefabs, Random.Range(6, 10));
        GenerateObjects(FoodPrefabs, Random.Range(2, 6));
        GenerateObjects(new List<CellObject> { EnemyPrefab }, Random.Range(1, 4));
    }

    private void DrawBoardTile(int x, int y)
    {
        var blockingTile = BlockingTiles[Random.Range(0, BlockingTiles.Length)];
        var groundTile = GroundTiles[Random.Range(0, GroundTiles.Length)];
        Tile tile = IsEdgeTile(x, y) ? blockingTile : groundTile;
        m_TileMap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    private void InitializeCellData(int x, int y)
    {
        CellData cellData = new CellData();
        cellData.isPassable = !IsEdgeTile(x, y);
        m_BoardData[x, y] = cellData;
    }

    private void AddEndCellObject(Vector2Int endCoord)
    {
        AddObject(Instantiate(Exit), endCoord);
        m_EmptyCells.Remove(endCoord);
    }

    private bool IsEdgeTile(int x, int y)
    {
        return x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
    }
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return m_Grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
    }

    public CellData GetCellData(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= Width || cell.y < 0 || cell.y >= Height)
        {
            return null;
        }
        return m_BoardData[cell.x, cell.y];
    }
    
    public void GenerateObjects(List<CellObject> objectList, int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];
            CellObject newObject = Instantiate(objectList[Random.Range(0, objectList.Count)]);
            AddObject(newObject, coord);
            m_EmptyCells.RemoveAt(randomIndex);
        }
    }

    public void AddObject(CellObject obj, Vector2Int cell)
    {
        CellData data = m_BoardData[cell.x, cell.y];
        obj.transform.position = CellToWorld(cell);
        data.ContainedObject = obj;
        obj.Init(cell);
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_TileMap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_TileMap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    
    public void ClearLevel()
    {
        if(m_BoardData == null)
        {
            return;
        }

        for(int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                ClearCell(x, y);
            }
        }
    }

    private void ClearCell(int x, int y)
    {
        var cellData = m_BoardData[x, y];
        if (cellData.ContainedObject != null)
        {
            Destroy(cellData.ContainedObject.gameObject);
        }
        SetCellTile(new Vector2Int(x, y), null);
    }
}
