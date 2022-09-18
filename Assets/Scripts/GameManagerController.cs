using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;
using System.Linq;

public class GameManagerController : MonoBehaviour
{
    string[] _parametrs;
    public int numberChips;
    public static int numberPoints;
    public static int[,] coordinatesPoints;
    public int[] startPosition;
    public int[] winPosition;
    public int numberConnections;
    public int[,] listConnections;
    public GameObject Chips;
    public GameObject Points;
    public GameObject Connections;
    private Vector3 pointsPosition;
    private Vector3 chipsPosition;
    private Vector3 connectionPosition;
    GameObject chip;
    GameObject connection;
    public static Dictionary<int, List<int>> Graph = new Dictionary<int, List<int>>();
   
    
    // Start is called before the first frame update
    void Start()
    {
        Graph.Clear();
        _parametrs = System.IO.File.ReadAllLines(@"parameters.txt");
        numberChips = Int32.Parse(_parametrs[0]);
        numberPoints = Int32.Parse(_parametrs[1]);
        coordinatesPoints = new int[numberPoints,2];
        numberConnections = Int32.Parse(_parametrs[numberPoints+4]);
        listConnections = new int[numberConnections,2];
        for (int i = 2; i < numberPoints+2; i++)
        {
           string[] str2 = _parametrs[i].Split(' ');// делит строку заданным символом
           for(int j = 0; j < str2.Length; j++)
            {
               coordinatesPoints[i-2,j] = Convert.ToInt32(str2[j]);                
            }
        }
        for (int i = numberPoints + 2; i < numberPoints + 3; i++)
        {
            string[] str3 = _parametrs[i].Split(' ');// делит строку заданным символом
            startPosition = new int [str3.Length];
            for (int j = 0; j < str3.Length; j++)
            {
                startPosition[j]= Convert.ToInt32(str3[j]);
            }
        }
        for (int i = numberPoints + 3; i < numberPoints + 4; i++)
        {
            string[] str4 = _parametrs[i].Split(' ');// делит строку заданным символом
            winPosition = new int[str4.Length];
            for (int j = 0; j < str4.Length; j++)
            {
                winPosition[j] = Convert.ToInt32(str4[j]);
            }
        }
        for (int i = numberPoints + 5; i < _parametrs.Length; i++)
        {
            string[] str5 = _parametrs[i].Split(' ');// делит строку заданным символом
            winPosition = new int[str5.Length];
            for (int j = 0; j < str5.Length; j++)
            {
                listConnections[i-numberPoints-5,j] = Convert.ToInt32(str5[j]);
            }
        }
        for(int i = 0; i < numberPoints; i++)
        {
            pointsPosition = new Vector3(coordinatesPoints[i,0], coordinatesPoints[i,1],1.5f);
            Instantiate(Points,pointsPosition,Points.transform.rotation);
        }
        for (int i = 0; i < numberChips; i++)
        {
            int k = startPosition[i];
            chipsPosition = new Vector3(coordinatesPoints[k-1, 0], coordinatesPoints[k-1, 1], 1.0f);
            chip = Instantiate(Chips, chipsPosition, Chips.transform.rotation);
            chip.tag = k.ToString() ;
        }
        for( int i = 0; i < numberConnections; i++)
        {
           int p1 = listConnections[i, 0];
           int p2 = listConnections[i, 1];
            int x1 = coordinatesPoints[p1 - 1, 0];
            int y1 = coordinatesPoints[p1 - 1, 1];
            int x2 = coordinatesPoints[p2 - 1, 0];
            int y2 = coordinatesPoints[p2 - 1, 1];
            if(x1==x2)
            { int y3 =y1+((y2-y1)/2);
                connectionPosition = new Vector3(x1, y3, 1.75f);
             connection = Instantiate(Connections, connectionPosition, Connections.transform.rotation);
                connection.transform.Rotate(0, 0, 90);
            }
            if (y1 == y2)
            {
                int x3 = x1+((x2-x1)/2) ;
                connectionPosition = new Vector3(x3, y1, 1.75f);
                connection = Instantiate(Connections, connectionPosition, Connections.transform.rotation);
            }
        }
        for ( int i = 0; i < numberPoints; i++)
        {
            List<int> _graph = new List<int>();
            int p = i + 1;
            for (int j = 0; j < numberConnections; j++)
            {
                if (p == listConnections[j, 0])
                {
                    _graph.Add(listConnections[j, 1]);
                }
                else
                { if (p == listConnections[j, 1])
                    {
                        _graph.Add(listConnections[j, 0]);
                    }
                }
               
            }
            Graph.Add(p, _graph);
        }

    }

}
