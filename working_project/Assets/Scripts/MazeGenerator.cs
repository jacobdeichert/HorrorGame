using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour {


    public int mapheight = 11;
    public int mapwidth = 11;
    private int[,] maze;
    public int wallSize = 1;
    public GameObject wall;
    public GameObject end;
    public GameObject floor;
   // public GameObject player1;
    public GameObject player2;
    public GameObject Floortrap;
    public GameObject Walltrap;
    CrushingWallTrap setWall;
    public GameObject Outerwallx;
    public GameObject Outerwallz;
    private int trapSpawnChance = 10;
    private static System.Random rand = new System.Random();
    public bool hasCeiling = true;

	private bool isSpawned;

	// Use this for initialization
	void Start () {
        maze = GenerateMaze(mapheight,mapwidth);
        wall.transform.localScale = new Vector3(1 * wallSize, 1 * wallSize, 1 * wallSize);
        floor.transform.localScale = new Vector3(1 * wallSize, 1 * wallSize, 1 * wallSize);
        end.transform.localScale = new Vector3(1 * wallSize, 1 * wallSize, 1 * wallSize);
        Vector3 Player1pos = new Vector3(1*wallSize,0,1*wallSize);
        Vector3 Player2pos = new Vector3(1 * wallSize, 0, (mapwidth-2) * wallSize);

		// This will place the monster as close to centre as possible
		do
		{
			Vector3 monsterPos = Vector3((mapwidth*wallSize)/2, 0, (mapheight*wallSize)/2);
			/*
			  if mapTile at monsterPos is !wallTile & !trapTile
				instantiate monster here
				isSpawned = true
			*/
		}
		while(!isSpawned);

        //GameObject Player1 = Instantiate(player1) as GameObject;
        GameObject Player2 = Instantiate(player2) as GameObject;
       // Player1.transform.position = Player1pos;
        Player2.transform.position = Player2pos;
        //GameObject End = Instantiate(end) as GameObject;

        
        // Create wall around the map
        Outerwallx.transform.localScale = new Vector3((1 * wallSize)*mapheight, 1 * wallSize, 1 * wallSize);
        Outerwallz.transform.localScale = new Vector3(1 * wallSize, 1 * wallSize, (1 * wallSize)*mapwidth);
        GameObject OuterWallx = Instantiate(Outerwallx) as GameObject;
        OuterWallx.transform.position = new Vector3((wallSize*mapheight)/2-2, 0, 0 - (1 * wallSize));
        OuterWallx.transform.Rotate( new Vector3(0, 90, 0));
        GameObject OuterWallx2 = Instantiate(Outerwallx) as GameObject;
        OuterWallx2.transform.position = new Vector3((wallSize * mapheight) / 2-2, 0, (mapwidth*wallSize));
        OuterWallx2.transform.Rotate(new Vector3(0, 90, 0));
        GameObject OuterWallz = Instantiate(Outerwallz) as GameObject;
        OuterWallz.transform.position = new Vector3(0-wallSize, 0, (wallSize * mapwidth) / 2-2);
        GameObject OuterWallz2 = Instantiate(Outerwallz) as GameObject;
        OuterWallz2.transform.position = new Vector3(mapheight*wallSize, 0, (wallSize * mapheight) / 2 - 2);

        for (int i = 0; i < mapheight; i++)
        {
            for(int j = 0;j<mapwidth;j++)
            {

                Vector3 Floorpos = new Vector3(i * wallSize, 0 - wallSize, j * wallSize);
                
                //Place floors
                if (!(maze[i, j] == 1))
                {
                    //Vector3 Floorpos = new Vector3(i * wallSize, 0 - wallSize, j * wallSize);
                    

                        switch (Random.Range(0,20))
                        {
                            case 1:
                                    GameObject FloorTrap = Instantiate(Floortrap) as GameObject;
                                    FloorTrap.transform.position = Floorpos;
                                    break;
                            default:
                                GameObject Floor = Instantiate(floor) as GameObject;
                                Floor.transform.position = Floorpos;
                                break;
                        }
                        
                    
                }

                if (maze[i, j] == 1)
                {
                    Vector3 pos = new Vector3(i * wallSize, 0, j * wallSize);
                    
                    
                        //Randomly take out walls and replace with floors to create loops/multi paths
                        switch (Random.Range(0,20))
                        {
                            case 1:
                               // if ((pos.x - Player1pos.x >= wallSize * 2) && (pos.z - Player1pos.z >= wallSize * 2))
                                //{


                                if (i == 0 || i == mapheight - 1 || j == 0 || j == mapwidth - 1)
                                {
                                    GameObject Wall = Instantiate(wall) as GameObject;
                                    Wall.transform.position = pos;

                                }
                                else
                                {
                                    GameObject Floor = Instantiate(floor) as GameObject;
                                    Floor.transform.position = Floorpos;
                                }
                                    
                                //}
                                break;
                            case 2:
                                if (i == mapheight - 2 && j == mapwidth - 1)
                                {
                                    //End position
                                    GameObject Floor = Instantiate(floor) as GameObject;
                                    Floor.transform.position = Floorpos;
                                    //End.transform.position = new Vector3((mapheight-2) * wallSize, 0 - wallSize, (j+1) * wallSize);
                                }
                                else
                                {
                                    GameObject WallTrap = Instantiate(Walltrap) as GameObject;
                                    
                                    WallTrap.transform.position = pos;

                                    
                                   
                                }
                                    
                                break;

                            default:
                                
                                if (i == mapheight - 2 && j == mapwidth - 1)
                                {
                                    //End position
                                    GameObject Floor = Instantiate(floor) as GameObject;
                                    Floor.transform.position = Floorpos;
                                    //End.transform.position = new Vector3((mapheight - 2) * wallSize, 0 - wallSize, (j+1) * wallSize);
                                }
                                else
                                {
                                    GameObject Wall = Instantiate(wall) as GameObject;
                                    Wall.transform.position = pos;
                                }
                               
                                break;
                        }

                        //Spawn Traps Randomly
                        //switch (Random.Range(0, 15))
                        //{
                        //    case 1:
                        //        if ((pos.x - Player1pos.x >= wallSize * 2) && (pos.z - Player1pos.z >= wallSize * 2))
                        //        {
                        //            Trap.transform.position = pos;
                        //        }
                        //        break;

                        //    default:
                        //        //Wall.transform.position = pos;
                        //        break;
                        //}                         
                }
            }
        }


        // fix the rotation of all traps
        Trap[] traps;
        traps = GameObject.FindObjectsOfType(typeof(Trap)) as Trap[];
        foreach (Trap t in traps) {
            t.correctRotation();
        }

	}

    private int[,] GenerateMaze(int height, int width)
    {
        int[,] maze = new int[height,width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                maze[i, j] = 1;
            }

        }

        System.Random random = new System.Random();
        int r = random.Next(height);
        while (r % 2 == 0)
            r = random.Next(height);
        int c = random.Next(width);
        while (c % 2== 0)
            c = random.Next(width);

        maze[r, c] = 0;

        MazeDigger(maze, r, c);

        return maze;
    }

    void MazeDigger(int[,] maze,int r, int c)
    {
        int[] directions = new int[] { 1, 2, 3, 4 };
        Shuffle(directions);

        for (int i = 0; i < directions.Length; i++)
        {
            switch (directions[i])
            {
                case 1://UP
                    if (r - 2 <= 0)
                        continue;
                    if (maze[r - 2, c] != 0)
                    {
                        maze[r - 2, c] = 0;
                        maze[r - 1, c] = 0;
                        MazeDigger(maze, r - 2, c);
                    }
                    break;
                case 2://RIGHT
                    if (c + 2 >= mapwidth - 1)
                        continue;
                    if (maze[r, c + 2] != 0)
                    {
                        maze[r, c + 2] = 0;
                        maze[r, c + 1] = 0;
                        MazeDigger(maze, r, c+2);
                    }
                    break;
                case 3://DOWN
                    if (r + 2>= mapheight - 1)
                        continue;
                    if (maze[r + 2, c] != 0)
                    {
                        maze[r + 2, c] = 0;
                        maze[r + 1, c] = 0;
                        MazeDigger(maze, r + 2, c);
                    }
                    break;
                case 4://LEFT
                    if (c - 2 <= 0)
                        continue;
                    if (maze[r, c - 2] != 0)
                    {
                        maze[r, c - 2] = 0;
                        maze[r, c - 1] = 0;
                        MazeDigger(maze, r, c - 2);
                    }
                    break;
            }
        }

    }
    public static void Shuffle<T>(T[] array)
    {
        var random = rand;
        for (int i = array.Length; i > 1; i--)
        {
            int j = random.Next(i);
            T tmp = array[j];
            array[j] = array[i - 1];
            array[i - 1] = tmp;
        }
    }
}
