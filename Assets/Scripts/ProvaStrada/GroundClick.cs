using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundClick : MonoBehaviour {

    [SerializeField]
    GameObject prefabRoad;
    [SerializeField]
    GameObject prefabNode;

    GameObject nodeStart;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 roadStart;
            ClickLocation(out roadStart);
            if(ClickLocation(out roadStart))
            {
                nodeStart = GameObject.Instantiate(prefabNode, roadStart, Quaternion.identity);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            Vector3 roadEnd;
            if(nodeStart != null && ClickLocationForced(out roadEnd))
            {
                GameObject nodeEnd = GameObject.Instantiate(prefabNode, roadEnd, Quaternion.identity);
                CreateRoad(nodeStart.transform.position, nodeEnd.transform.position);
            }
            nodeStart = null;
        }
    }

    bool ClickLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(ray);
        RaycastHit rayHit;
        if (this.GetComponent<Collider>().Raycast(ray, out rayHit, 100f))
        {
            point = rayHit.point;
            return true;
        } else
        {
            point = Vector3.zero;
            return false;
        }
    }

    bool ClickLocationForced(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if(GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            point = hitInfo.point;
            return true;
        }
        point = Vector3.zero;
        return false;
    }

    public void SetNodeStart(GameObject n)
    {
        nodeStart = n;
    }



    void CreateRoad(Vector3 roadStart, Vector3 roadEnd)
    {
        GameObject road = GameObject.Instantiate(prefabRoad);
        road.transform.position = roadStart + new Vector3(0, 0.01f, 0);

        road.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, roadStart - roadEnd);
        float width = 1f;
        float length = Vector3.Distance(roadStart, roadEnd);
        if(length < 1)
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
