using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    private RailNode[,] nodes;
    public Vector2Int firstStart;
    public Vector2Int secondStart;


    private static RailManager instance;

    void Awake()
    {
        instance = this;
    }

    public void CreateNodes(RailNode[,] _nodes, Vector2Int _firstStart, Vector2Int _secondStart)
    {
        nodes = _nodes;
        firstStart = _firstStart;
        secondStart = _secondStart;

        GameManager.Camera.transform.position = nodes[secondStart.x, secondStart.y].position + new Vector3(-13, 0, 20);
    }

    public void Clear()
    {
        nodes = null;
        firstStart = Vector2Int.zero;
        secondStart = Vector2Int.zero;
    }

	public void GetRailNodeData(Vector2Int index, ref ushort dir, ref ushort rotation)
	{
		dir = nodes[index.y, index.x].dir;
		rotation = nodes[index.y, index.x].rotation;
	}

    public void GetNext(ref Vector3 currentPos, ref Vector3 nextPos, Vector2Int currentIndex, Vector2Int nextIndex)
    {
        //Debug.Log(currentIndex.x + " ------- " + currentIndex.y);
        currentPos = nodes[currentIndex.x, currentIndex.y].position;
        nextPos = nodes[nextIndex.x, nextIndex.y].position;
    }

    //uh, awful
    public void Next(ref Vector2Int currentIndex, ref Vector2Int nextIndex)
    {
        if (currentIndex == -Vector2Int.one)
        {
            Debug.Log("current: "+ currentIndex + " ,next index: "+nextIndex);
            var diff = nextIndex - currentIndex;
            Debug.Log("Diff: " + diff);
            currentIndex = nextIndex;
            nextIndex += diff;
            return;
        }
        
        var node = nodes[nextIndex.x, nextIndex.y];
        switch (node.type)
        {
            case 1:
            {
                var neighbours = new Vector2Int[2];
                switch (node.rotation)
                {
                    case 0:
                        neighbours[0] = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                        neighbours[1] = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                        break;
                    case 1:
                        neighbours[0] = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                        neighbours[1] = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                        break;
                }

                for (int i = 0; i < 2; i++)
                {
                    if (neighbours[i] != currentIndex && nodes[neighbours[i].x,neighbours[i].y].isValid)
                    {
                        currentIndex = nextIndex;
                        nextIndex = neighbours[i];
                        break;
                    }

                }
            }
                break;
            case 2:
            {
                var neighbours = new Vector2Int[2];
                switch (node.rotation)
                {
                    case 0:
                        neighbours[0] = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                        neighbours[1] = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                        break;
                    case 1:
                        neighbours[0] = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                        neighbours[1] = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                        break;
                    case 2:
                        neighbours[0] = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                        neighbours[1] = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                        break;
                    case 3:
                        neighbours[0] = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                        neighbours[1] = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                        break;
                }

                for (int i = 0; i < 2; i++)
                {
                    if (neighbours[i] != currentIndex && nodes[neighbours[i].x,neighbours[i].y].isValid)
                    {
                        currentIndex = nextIndex;
                        nextIndex = neighbours[i];
                        break;
                    }

                }
            }
                break;
            case 3:
            {
                Vector2Int neighbours = Vector2Int.zero;
                switch (node.rotation)
                {
                    case 0:
                        switch (node.dir)
                        {
                            case 0:
                                if (currentIndex.y <= nextIndex.y)
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                                }

                                break;
                            case 1:
                                if (currentIndex.y < nextIndex.y)
                                {
                                    neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                                }

                                break;
                        }

                        break;
                    case 1:
                        switch (node.dir)
                        {
                            case 0:
                                if (currentIndex.x >= nextIndex.x)
                                {
Debug.Log("rail 1 0");
                                    neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
Debug.Log("rail 1 1");
                                }

                                break;
                            case 1:
                                if (currentIndex.x > nextIndex.x)
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
Debug.Log("rail 1 2");
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
Debug.Log("rail 1 3");
                                }

                                break;
                        }

                        break;
                    case 2:
                        switch (node.dir)
                        {
                            case 0:
                                if (currentIndex.y >= nextIndex.y)
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
Debug.Log("rail 2 1");

                                }
                                else
                                {
Debug.Log("rail 2 2");
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                                }

                                break;
                            case 1:
                                if (currentIndex.y > nextIndex.y)
                                {
                                    neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
Debug.Log("rail 2 3");
                                }
                                else if(currentIndex.y < nextIndex.y)
                                {
                                    neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y); //???
Debug.Log("rail 2 4");
                                }
								else
{
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1); //???
Debug.Log("rail 2 5");
}
                                break;
                        }

                        break;
                    case 3:
                        switch (node.dir)
                        {
                            case 0:
                                if (currentIndex.x <= nextIndex.x)
                                {
                                    neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                                }

                                break;
                            case 1:
                                if (currentIndex.x < nextIndex.x)
                                {
                                    neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                                }
                                else
                                {
                                    neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                                }

                                break;
                        }

                        break;
                }

                currentIndex = nextIndex;
                nextIndex = neighbours;
            }
                break;
            case 4:
              {
                  Vector2Int neighbours = Vector2Int.zero;

                  if (currentIndex.y < nextIndex.y)
                  {
                      switch (node.dir)
                      {
                          case 0:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                              break;
                          case 1:
                              neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                              break;
                          case 2:
                              neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                              break;
                      }
                  }
                  else if (currentIndex.y > nextIndex.y)
                  {
                      switch (node.dir)
                      {
                          case 0:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                              break;
                          case 1:
                              neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                              break;
                          case 2:
                              neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                              break;
                      }
                  }
                  else if (currentIndex.x < nextIndex.x)
                  {
                      switch (node.dir)
                      {
                          case 0:
                              neighbours = new Vector2Int(nextIndex.x + 1, nextIndex.y);
                              break;
                          case 1:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                              break;
                          case 2:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                              break;
                      }
                  }
                  else
                  {
                      switch (node.dir)
                      {
                          case 0:
                              neighbours = new Vector2Int(nextIndex.x - 1, nextIndex.y);
                              break;
                          case 1:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y - 1);
                              break;
                          case 2:
                              neighbours = new Vector2Int(nextIndex.x, nextIndex.y + 1);
                              break;
                      }
                  }
                  currentIndex = nextIndex;
                  nextIndex = neighbours;
              }
                  break;
        }
    }

    public void Switch(Vector2Int index)
    {
        nodes[index.y, index.x].dir++;
        nodes[index.y, index.x].dir %= nodes[index.y, index.x].maxDir;
    }

}
