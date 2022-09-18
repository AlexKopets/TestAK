using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class ChipsController : MonoBehaviour
{
    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static ChipsController previousSelected = null;
    private SpriteRenderer render;
    private bool isSelected = false;
    Queue<int> Queue = new Queue<int>();
    List<int> UsedValues = new List<int>();
    
    int startPosition;
    int endPosition;
    float positionChipX;
    float positionChipY;
    float positionPointX;
    float positionPointY;
    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
     
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Select()
    {  
            isSelected = true;
            render.color = selectedColor;
            previousSelected = gameObject.GetComponent<ChipsController>();
            var positionChip = render.transform.position;
            positionChipX = positionChip.x;
            positionChipY = positionChip.y;
        
        

    }

    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }
    void OnMouseDown()
    {
        
            if (render.sprite == null)
            {
                return;
            }


            if (isSelected)
            {
                Deselect();
            }
            else
            {
                if (previousSelected == null)
                {
                    Select();
                    Poisk();
                }
                else
                {
                    previousSelected.Deselect();
                }
            }

                               
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int p = GameManagerController.numberPoints;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Point")
                {
                    var positionPoint = hit.transform.position;
                    positionPointX = positionPoint.x;
                    positionPointY = positionPoint.y;

                }
            }
            for (int i = 0; i < p; i++)
            {
                if (positionChipX == GameManagerController.coordinatesPoints[i, 0] && positionChipY == GameManagerController.coordinatesPoints[i, 1])
                {
                    startPosition = i + 1;

                }

                if (positionPointX == GameManagerController.coordinatesPoints[i, 0] && positionPointY == GameManagerController.coordinatesPoints[i, 1])
                {
                    endPosition = i + 1;


                }
            }
            
        }
        


    }
    private void Poisk()
    {
        Queue.Enqueue(startPosition);

        while (Queue.Count != 0)
        {
            int node = Queue.Dequeue();
            List<int> vertites = GameManagerController.Graph[node+1];

            if (node == endPosition)
            {
                return;
            }

            foreach (int vertite in vertites)
            {
                if (!UsedValues.Contains(vertite))
                {
                    if (vertite == endPosition)
                    {
                        if (Queue.Count > 0)
                        {
                            Queue.Dequeue();
                        }
                        Queue.Enqueue(node+1);
                        Queue.Enqueue(endPosition);
                        return;
                    }
                    Queue.Enqueue(vertite);

                }

            }
            UsedValues.Add(node+1);
        }
        foreach (int value in Queue)
        {
            Debug.Log(value);
        }
    }
    
}
