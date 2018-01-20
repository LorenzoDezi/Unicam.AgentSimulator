using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model.Bus;
using Unicam.AgentSimulator.Scripts;
using Unicam.AgentSimulator.Utility;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    /// <summary>
    /// This class specifiy a node, giving it an orientation.
    /// It is useful because it is used to place bus stops, and they need
    /// to know the street orientation.
    /// </summary>
    public class OrientedNode
    {
        public Vector3 position;
        public Quaternion rotation;

        public OrientedNode(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public class StreetMaker : MonoBehaviour
    {
        //The edimburgh origin in UTM x,y coordinates. It will be used to adapt coordinates to the scene
        [NonSerialized]
        public static Vector2 EdinburghUTMOrigin = new Vector2(
                    383695.7f,
                    9647538f
                );

        [Header("Scene build objects")]
        [SerializeField]
        //The road-part material we will use to build the road
        Material roadMaterial;
        [SerializeField]
        //The bus stop prefab 
        GameObject prefabStop;

        [Header("Text files input")]
        [SerializeField]
        //input text file with bus route
        TextAsset busRouteText;
        [SerializeField]
        //input text file with bus stop positions
        TextAsset stopRouteText;

        //constant used in parsing the input text file
        char[] positionDelimiter = Environment.NewLine.ToCharArray();
        //Variables used in street making, used to solve graphic problems.
        float maxNodeDistance = 100f;
        float maxStopDistance = 100f;
        //Street nodes, used to instantiate bus stops
        OrientedNode[] streetNodes;


        private void Awake()
        {

            CreateStreet();

            CreateStops();

        }

        /// <summary>
        /// This method makes the street procedurally, using the input text file
        /// </summary>
        private void CreateStreet()
        {
            string[] roadNodesSet = busRouteText.text.Split(positionDelimiter,
                StringSplitOptions.RemoveEmptyEntries);

            if (roadNodesSet.Length != 0)
            {

                List<Vector3> streetNodes = new List<Vector3>();
                List<OrientedNode> orientedStreetNodes = new List<OrientedNode>();
                for (int i = 0; i < roadNodesSet.Length; i++)
                {
                    Vector3 currentNode = this.GetNodePosition(roadNodesSet[i]);
                   
                    foreach(Vector3 node in streetNodes)
                    {
                        //Solving overlapping textures problem - texture height trick
                        if(Vector3.Distance(node, currentNode) < maxNodeDistance)
                        {
                            currentNode.y += node.y;
                        }
                    }
                    streetNodes.Add(currentNode);
                }

                Quaternion currentRotation;
                for(int i=0; i < streetNodes.Count; i++)
                {
                    
                    if(i == streetNodes.Count - 1)
                    {
                        currentRotation = Quaternion.identity;
                    } else
                    {
                        currentRotation = Quaternion.LookRotation(streetNodes[i + 1]);
                    }
                    orientedStreetNodes.Add(new OrientedNode(streetNodes[i], currentRotation));
                }
                this.streetNodes = orientedStreetNodes.ToArray();
                LineRenderer render = this.GetComponent<LineRenderer>();
                render.useWorldSpace = true;
                render.lightmapIndex = 0;
                render.numCornerVertices = 5;
                render.positionCount = streetNodes.Count - 1;
                render.SetPositions(streetNodes.ToArray());
                
            }
        }

        /// <summary>
        /// This methods instantiate bus stops, at the positions given by the input text file
        /// </summary>
        private void CreateStops()
        {
            string[] stopPositionSet = stopRouteText.text.Split(positionDelimiter,
                StringSplitOptions.RemoveEmptyEntries);

            if (stopPositionSet.Length != 0)
            {
                for (int i = 0; i < stopPositionSet.Length; i += 2)
                {
                    Vector3 position = this.GetStopPosition(stopPositionSet[i]);
                    Quaternion rotation = Quaternion.identity;
                    string name = this.GetStopName(stopPositionSet[i]);
                    foreach(OrientedNode node in streetNodes)
                    {
                        if(Vector3.Distance(position, node.position) <= maxStopDistance)
                        {
                            position = node.position;
                            rotation = node.rotation;
                            break;
                        }
                    }
                    this.CreateStop(position, rotation, name);
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
                position = new Vector3(float.Parse(positionValues[0]), 0.01f, float.Parse(positionValues[2]));

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
        void CreateStop(Vector3 stopPosition, Quaternion rotation, string name)
        {
            float roadWidth = this.GetComponent<LineRenderer>().startWidth;
            //TODO: da vedere la rotazione come esce
            GameObject stop = GameObject.Instantiate(prefabStop, stopPosition, rotation);
            stop.GetComponent<StopState>().Name = name;
            //Setting graphics problems
            stop.transform.Rotate(Vector3.up, 90);
            stop.transform.position = new Vector3(stop.transform.position.x - roadWidth/2f - 4f, stop.transform.position.y + 0.02f, stop.transform.position.z);
        }

    }
    }
