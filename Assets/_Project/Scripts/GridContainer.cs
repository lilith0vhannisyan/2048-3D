using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    private float spacing = 2.0f; // the width of tiles
    private float startOffset = 3.0f; // ((4 colums * 2 width)/2)-(width/2)\
    [SerializeField] private List<Vector3> localPosList = new List<Vector3>();
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private bool isMoving = false;
    private GameObject[,] gridMap = new GameObject[4, 4];
    void Start()
    {
        //Spawn 2 cubes
        for (int i = 0; i < 2; i++)
        {
            SpawnNewTile();
        }
    }

    private void Update()
    {
        if (isMoving) return;

        // Inputs
        if (Input.GetKeyDown(KeyCode.UpArrow)) StartCoroutine(HandleMove(Vector2Int.up));
        else if (Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(HandleMove(Vector2Int.down));
        else if (Input.GetKeyDown(KeyCode.RightArrow)) StartCoroutine(HandleMove(Vector2Int.left));
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) StartCoroutine(HandleMove(Vector2Int.right));
    }

    private IEnumerator HandleMove(Vector2Int direction)
    {
        isMoving = true;
        bool boardChanged = false;

        // If moving Right (1,0), start from x=3. If moving Up (0,1), start from y=3.
        for (int x = (direction.x > 0 ? 3 : 0); (direction.x > 0 ? x >= 0 : x < 4); x += (direction.x > 0 ? -1 : 1))
        {
            for (int y = (direction.y > 0 ? 3 : 0); (direction.y > 0 ? y >= 0 : y < 4); y += (direction.y > 0 ? -1 : 1))
            {
                if (gridMap[x, y] != null)
                { 
                    if (ProcessTile(x, y, direction)) boardChanged = true;
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        if (boardChanged)
        {
            SpawnNewTile();
        }

        isMoving = false;
    }

    private bool ProcessTile(int x, int y, Vector2Int dir)
    {
        GameObject currentCube = gridMap[x, y];
        Tile currentTile = currentCube.GetComponent<Tile>();

        int nextX = x + dir.x;
        int nextY = y + dir.y;

        int finalX = x;
        int finalY = y;
        bool merged = false;

        //Find the farthest empty spot or a matching neighbor
        while (nextX >= 0 && nextX < 4 && nextY >= 0 && nextY < 4)
        {
            if (gridMap[nextX, nextY] == null)
            {
                finalX = nextX;
                finalY = nextY;
            }
            else
            {
                Tile neighborTile = gridMap[nextX, nextY].GetComponent<Tile>();
                // CHECK FOR MERGE
                if (neighborTile.CurrentLevel == currentTile.CurrentLevel)
                {
                    // Set the old pos null
                    gridMap[x, y] = null;
                    // Set new pos
                    //gridMap[finalX, finalY] = currentCube;
                    
                    neighborTile.MergeWith(currentTile);
                    merged = true;
                }
                break;
            }
            nextX += dir.x;
            nextY += dir.y;
        }

        // 2. If it didn't merge but found a new empty spot, move it
        if (!merged && (finalX != x || finalY != y))
        {
            gridMap[x, y] = null;
            gridMap[finalX, finalY] = currentCube;

            AnimateToPosition(currentCube, finalX, finalY);
            return true;
        }

        return merged;
    }

    private void AnimateToPosition(GameObject cube, int gridX, int gridY)
    {
        // IDK why camera/view is inverted, thats why i add 3-gridX
        float worldX = (gridX * spacing) - startOffset;
        float worldY = (gridY * spacing) - startOffset;

        Vector3 targetPos = new Vector3(worldX, worldY, 0);

        // This moves the cube in a straight line on the grid
        cube.transform.DOLocalMove(targetPos, 0.2f).SetEase(Ease.OutQuad);
    }

    private void SpawnNewTile()
    {
        // Find all empty slots in the gridMap
        List<Vector2Int> emptySpots = new List<Vector2Int>();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (gridMap[x, y] == null) emptySpots.Add(new Vector2Int(x, y));
            }
        }

        if (emptySpots.Count > 0)
        {
            Vector2Int spot = emptySpots[Random.Range(0, emptySpots.Count)];

            Vector3 localPos = new Vector3((spot.x * spacing) - startOffset, (spot.y * spacing) - startOffset, 0);
            GameObject newCube = Instantiate(cubePrefab, transform);
            newCube.transform.localPosition = localPos;
            newCube.transform.localRotation = Quaternion.identity; // Forces 0,0,0 rotation
            newCube.transform.localScale = Vector3.one;

            gridMap[spot.x, spot.y] = newCube;

            Tile tileScript = newCube.GetComponent<Tile>();
            tileScript.UpdateVisuals(TileLevel.Two);

            //// Nice spawn animation
            newCube.transform.localScale = Vector3.zero;
            newCube.transform.DOScale(Vector3.one, 0.2f);
        }
    }
}
    