using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Scripts;
using Unicam.AgentSimulator.Scripts.Bus.Model;
using Unicam.AgentSimulator.Utility;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    /// <summary>
    /// This class specifying a node, giving it an orientation.
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

    /// <summary>
    /// The streetmaker script creates a road at start of the scene, creating bus stops using the input
    /// texts. Basically it gives a shape to the Edinburgo bus scene.
    /// </summary>
    public class StreetMaker : MonoBehaviour
    {
        //The edimburgh origin in UTM x,y coordinates. It will be used to adapt coordinates to the scene
        [NonSerialized]
        [HideInInspector]
        public static Vector2 EdinburghUTMOrigin = new Vector2(
                    383695.7f,
                    9647538f
                );

        [Header("Scene build objects")]
        [SerializeField]
        [Tooltip("The material used to build the road")]
        Material roadMaterial;
        [SerializeField]
        [Tooltip("The bus stop prefab")]
        GameObject prefabStop;

        [Header("Text files input")]
        [SerializeField]
        [Tooltip("Input text file with bus route")]
        TextAsset busRouteText;

        [SerializeField]
        [Tooltip("Input text file with stop route")]
        TextAsset stopRouteText;

        //constant used in parsing the input text file
        char[] positionDelimiter = Environment.NewLine.ToCharArray();

        //Street nodes, used to instantiate bus stops
        OrientedNode[] streetNodes;
        LineRenderer streetRenderer;


        private void Awake()
        {
            streetRenderer = this.GetComponent<LineRenderer>();
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
                this.streetNodes = this.GetStreetNodes(roadNodesSet);
                streetRenderer.useWorldSpace = true;
                streetRenderer.lightmapIndex = 0;
                streetRenderer.numCornerVertices = 5;
                List<Vector3> positions = new List<Vector3>();
                foreach(OrientedNode node in this.streetNodes)
                {
                    positions.Add(node.position);
                }
                streetRenderer.positionCount = positions.Count - 1;
                streetRenderer.SetPositions(positions.ToArray());
            }
        }

        /// <summary>
        /// This methods instantiate bus stops, at the positions given by the input text file
        /// </summary>
        private void CreateStops()
        {
            string[] stopPositionSet = stopRouteText.text.Split(positionDelimiter,
                StringSplitOptions.RemoveEmptyEntries);
            //Used as a max distance, between the stop position of the input text file and
            //the next node in the bus route input text file - to make the bus stop close to the road
            float maxStopDistance = 100f;

            if (stopPositionSet.Length != 0)
            {
                for (int i = 0; i < stopPositionSet.Length; i += 2)
                {
                    //Getting name
                    string name = this.GetStopName(stopPositionSet[i]);
                    //Getting position and rotation
                    Vector3 position = this.GetStopPosition(stopPositionSet[i]);
                    Quaternion rotation = Quaternion.identity;
                    foreach (OrientedNode node in streetNodes)
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
        /// Get an array of oriented nodes from <paramref name="positionsNodeSet"/>
        /// </summary>
        /// <param name="positionsNodeSet"> an array of string, each defining the node position</param>
        /// <returns></returns>
        OrientedNode[] GetStreetNodes(string[] positionsNodeSet)
        {
            List<Vector3> positions = new List<Vector3>();
            List<OrientedNode> orientedStreetNodes = new List<OrientedNode>();
            float maxNodeDistance = 100f;

            for (int i = 0; i < positionsNodeSet.Length; i++)
            {
                Vector3 currentPosition = this.GetNodePosition(positionsNodeSet[i]);
                foreach (Vector3 position in positions)
                {
                    //Solving overlapping textures problem - texture height trick
                    if (Vector3.Distance(position, currentPosition) < maxNodeDistance)
                        currentPosition.y += position.y;
                }
                positions.Add(currentPosition);
            }
            Quaternion currentRotation;
            for (int i = 0; i < positions.Count; i++)
            {
                if (i == positions.Count - 1)
                    currentRotation = Quaternion.identity;
                else
                    currentRotation = Quaternion.LookRotation(positions[i + 1]);
                orientedStreetNodes.Add(new OrientedNode(positions[i], currentRotation));
            }
            return  orientedStreetNodes.ToArray();
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
            GameObject stop = GameObject.Instantiate(prefabStop, stopPosition, rotation);
            stop.GetComponent<StopState>().Name = name;
            //Setting graphics problems - hardcoded
            stop.transform.Rotate(Vector3.up, 90);
            stop.transform.position = new Vector3(stop.transform.position.x - roadWidth/2f - 4f,
                stop.transform.position.y + 0.02f, 
                stop.transform.position.z);
        }

    }
    }
