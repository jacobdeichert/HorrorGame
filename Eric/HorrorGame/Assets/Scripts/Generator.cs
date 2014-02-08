using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public int mapWidth = 10;
    public int mapHieght = 10;
    private int mapSize;

    public GameObject[] prefabs;
    
	// Use this for initialization
	void Start () {
        mapSize = mapWidth * mapHieght;
        GenerateMap();
	}
    #region MapGeneration
    void GenerateMap()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for(int j = 0;j<mapHieght;j++)
            {
                GameObject tile = new GameObject();
                if (i == 0 && j == 0)
                {
                    tile = Instantiate(prefabs[0]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 90, 0));
                }
                else if (i == mapWidth-1 && j == 0)
                {
                    tile = Instantiate(prefabs[0]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 0, 0));

                }
                else if (i == mapWidth-1 && j == mapHieght-1)
                {
                    tile = Instantiate(prefabs[0]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, -90, 0));

                }
                else if (i == 0 && j == mapHieght-1)
                {
                    tile = Instantiate(prefabs[0]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 180, 0));
                }
                else if (j == 0)
                {
                    tile = Instantiate(prefabs[Random.Range(0,1)]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 0, 0));
                }
                else if (j == mapHieght-1)
                {
                    tile = Instantiate(prefabs[Random.Range(0, 1)]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 180, 0));
                }
                else if (i == 0)
                {
                    tile = Instantiate(prefabs[Random.Range(0, 1)]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, 90, 0));
                }
                else if (i == mapWidth-1)
                {
                    tile = Instantiate(prefabs[Random.Range(0, 1)]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                    tile.transform.Rotate(new Vector3(0, -90, 0));
                }
                else
                {
                    tile = Instantiate(prefabs[Random.Range(0, 4)]) as GameObject;
                    tile.transform.position = new Vector3(i * 10, 0, j * 10);
                }
            }
        }

    }
    #endregion
    // Update is called once per frame
    void Update()
    {
	
	}
}
