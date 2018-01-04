using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Scripts;
using Unicam.AgentSimulator.Utility;
using UnityEngine;

public class StreetMaker : MonoBehaviour {

    [SerializeField]
    GameObject prefabRoad;

    [SerializeField]
    GameObject prefabStop;

    [SerializeField]
    TextAsset busRouteText;

    [SerializeField]
    TextAsset stopRouteText;

    char[] positionDelimiter = Environment.NewLine.ToCharArray();

    public static Vector2 EdinburghUTMOrigin = new Vector2(
                383695.7f,
                9647538f
            );


    private void Start()
    {
        
        CreateStreet();

        CreateStops();
        
    }

    private void CreateStreet()
    {
        string[] roadNodesSet = busRouteText.text.Split(positionDelimiter,
            StringSplitOptions.RemoveEmptyEntries);

        if (roadNodesSet.Length != 0)
        {

            Vector3 startNode = new Vector3();
            Vector3 endNode = new Vector3();
            Vector3 previousEndNode = new Vector3();
            for (int i = 0; i < roadNodesSet.Length; i += 2)
            {
                try
                {
                    startNode = this.GetNodePosition(roadNodesSet[i]);
                    if (previousEndNode != new Vector3())
                    {
                        this.CreateRoad(previousEndNode, startNode);
                    }
                    endNode = this.GetNodePosition(roadNodesSet[i + 1]);
                    previousEndNode = endNode;
                    this.CreateRoad(startNode, endNode);
                }
                catch (IndexOutOfRangeException)
                {
                    //Positions are odd and the last node position ends out of the position array
                }
            }
        }
    }

    private void CreateStops()
    {
        string[] stopPositionSet = stopRouteText.text.Split(positionDelimiter,
            StringSplitOptions.RemoveEmptyEntries);

        if (stopPositionSet.Length != 0)
        {
            for (int i = 0; i < stopPositionSet.Length; i += 2)
            {
                Vector3 position = this.GetStopPosition(stopPositionSet[i]);
                string name = this.GetStopName(stopPositionSet[i]);
                this.CreateStop(position, name);
            }
        }
    }

    /// <summary>
    /// This method returns a road node position, given a string representing the actual position
    /// </summary>
    /// <param name="currentStringPosition">a string representing the position to be parsed</param>
    /// <returns></returns>
    private Vector3 GetNodePosition(string currentStringPosition)
    {
        string[] propertyStrings = currentStringPosition.Split('\t');

        Vector3 position = new Vector3();
        if (propertyStrings.Length > 0)
        {
            string[] positionValues = propertyStrings[0].Split(',');
            if (positionValues.Length != 3)
                throw new System.FormatException("Problem parsing position" + positionValues.Length);
            positionValues = UTMUtility.ParseLongLatToUTM(positionValues, EdinburghUTMOrigin);
            position = new Vector3(float.Parse(positionValues[0]), float.Parse(positionValues[1]), float.Parse(positionValues[2]));
        }
        return position;    
    }

    /// <summary>
    /// This method returns a stop position, given a string representing the actual position + name
    /// </summary>
    /// <param name="currentStringPosition">a string containing the position to be parsed</param>
    /// <returns></returns>
    private Vector3 GetStopPosition(string currentStringPosition)
    {
        Vector3 position = new Vector3();
        string[] positionValues = currentStringPosition.Split(';');
        if (positionValues.Length != 3)
            throw new System.FormatException("Problem parsing position" + positionValues.Length);
        //Solving text format problems
        positionValues[1] = positionValues[1].Substring(0, positionValues[1].Length);
        positionValues[0] = positionValues[2].Substring(0, positionValues[2].Length);


        positionValues = UTMUtility.ParseLongLatToUTM(positionValues, EdinburghUTMOrigin);
        float positionX = float.Parse(positionValues[0]);
        float positionY = float.Parse(positionValues[2]);
        position = new Vector3(positionX,
            0f, positionY);
        return position;
    }

    /// <summary>
    /// This method returns a stop name, given a string representing the actual position + name
    /// </summary>
    /// <param name="currentStringPosition"> a string containing the name to be extracted </param>
    /// <returns></returns>
    private string GetStopName(string currentStringPosition)
    {
        return currentStringPosition.Split(';')[0];
    }

    /// <summary>
    /// This method creates given a position and a name
    /// </summary>
    /// <param name="stopPosition"> the position of the stop </param>
    /// <param name="name"> the name of the stop </param>
    void CreateStop(Vector3 stopPosition, string name)
    {
        //TODO: da vedere la rotazione come esce
        GameObject stop = GameObject.Instantiate(prefabStop, stopPosition, Quaternion.identity);
        stop.GetComponent<StopState>().Name = name;
    }

    void CreateRoad(Vector3 roadStart, Vector3 roadEnd)
    {
        GameObject road = GameObject.Instantiate(prefabRoad);
        road.transform.position = roadStart + new Vector3(0, 0.01f, 0);

        road.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, roadStart - roadEnd);
        float width = 1f;
        float length = Vector3.Distance(roadStart, roadEnd);
        if (length < 1)
        {
            return;
        }

        Mesh mesh = new Mesh();
        Vector3[] vertices =
        {
            new Vector3(-width/2,0,0),
            new Vector3(1-width/2,0,0),
            new Vector3(1-width/2,0,length),
            new Vector3(0-width/2,0,length)
        };

        int[] triangles =
        {
            2,1,0,
            0,3,2
        };

        Vector2[] uv =
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,length),
            new Vector2(0,length)
        };

        Vector3[] normals =
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;

        road.GetComponent<MeshFilter>().mesh = mesh;
    }
}
