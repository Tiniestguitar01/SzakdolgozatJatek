using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GenerateRoad : MonoBehaviour
{
    public static GenerateRoad Instance;

    public List<Segment> segments = new List<Segment>();
    public int segmentCount = 30;
    public int parts = 10;
    public GameObject segmentPrefab;
    public Material material;
    public MeshOutline meshOutline;


    public List<GameObject> props = new List<GameObject>();
    public List<GameObject> instancedProps = new List<GameObject>();
    public float density = 6;
    public float minDistance = 6;
    public float maxDistance = 10;
    public float layers = 4;

    List<Vector3> vertices;
    List<Vector3> normals;
    List<int> triangles;

    float distanceFromStartToEnd;
    float distanceFromPlayerToEnd;

    float direction;

    Transform player;
    GameObject segmentParent;
    Mesh mesh;
    private void Awake()
    {
        Instance = this;
        mesh = new Mesh();
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        triangles = new List<int>();

        player = GameObject.Find("Player").GetComponent<Transform>();
    }


    void Start()
    {
        mesh.Clear();

        segmentParent = new GameObject("Segments");

        Vector3 startPoint = new Vector3(Random.Range(0f, 10f), 0f, Random.Range(0f, 10f));
        Vector3 startDirPoint = new Vector3(startPoint.x + Random.Range(0f, 10f), 0f, startPoint.z + Random.Range(0f, 10f));
        Vector3 endDirPoint = new Vector3(startDirPoint.x + Random.Range(0f, 10f), 0f, startDirPoint.z + Random.Range(0f, 10f));
        Vector3 endPoint = new Vector3(endDirPoint.x + Random.Range(0f,10f), 0f, endDirPoint.z + Random.Range(0f, 10f));
        CreateSegment(startPoint,startDirPoint,endDirPoint,endPoint);

        direction = Random.Range(15f, 20f);
        for (int i = 1; i < segmentCount; i++)
        {
            Vector3 p1 = Vector3.zero;
            Vector3 p2 = segments[i - 1].bezier.endPoint - segments[i - 1].bezier.endDirectionPoint;
            Vector3 p3 = new Vector3(p2.x + Random.Range(direction/40, direction), 0f, p2.z + Random.Range(direction / 40, direction));
            Vector3 p4 = new Vector3(p3.x + Random.Range(direction / 40, direction), 0f, p3.z + Random.Range(direction / 40, direction));
            CreateSegment(p1, p2, p3, p4, segments[i - 1]);
        }

        distanceFromStartToEnd = Vector3.Distance(vertices[0], vertices[vertices.Count - 1]);

        mesh.SetVertices(vertices);
        AddTriangles();
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().material = material;
    }

    private void Update()
    {
        distanceFromPlayerToEnd = Vector3.Distance(player.position, vertices[vertices.Count - 1]);
        if (distanceFromPlayerToEnd < distanceFromStartToEnd/2 - 30f)
        {
            //Create new Segment
            Vector3 p1 = Vector3.zero;
            Vector3 p2 = segments[segments.Count - 1].bezier.endPoint - segments[segments.Count - 1].bezier.endDirectionPoint;
            Vector3 p3 = new Vector3(p2.x + Random.Range(direction / 40, direction), 0f, p2.z + Random.Range(direction / 40, direction));
            Vector3 p4 = new Vector3(p3.x + Random.Range(direction / 40, direction), 0f, p3.z + Random.Range(direction / 40, direction));
            CreateSegment(p1, p2, p3, p4, segments[segments.Count - 1]);
            distanceFromStartToEnd = Vector3.Distance(vertices[0], vertices[vertices.Count - 1]);

            //Delete the fisrt segment in the list
            Destroy(segments[0].gameObject);
            segments.RemoveAt(0);
            RemovePoints();
            Destroy(instancedProps[0]);
            instancedProps.RemoveAt(0);

            //Update
            UpdateMesh();
        }
    }

    public void CreateSegment(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Segment lastSegment = null)
    {
        Vector3 start = p1;
        Vector3 startDirection = p2;
        Vector3 endDirection = p3;
        Vector3 end = p4;
        BezierCurve bezier = new BezierCurve(start, startDirection, endDirection, end);

        Segment segment = Instantiate(segmentPrefab).GetComponent<Segment>();
        segment.transform.parent = segmentParent.transform;

        if (lastSegment != null)
        {
            segment.transform.position = lastSegment.transform.position + lastSegment.bezier.endPoint;
        }
        segment.bezier = bezier;

        GeneratePoints(segment.transform.position,segment);

        PlaceProps(segment);

        segments.Add(segment);
    }

    public void GeneratePoints(Vector3 position,Segment segment)
    {
        for (int i = 0; i < parts; i++)
        {
            float t = i / (parts - 1f);
            for (int j = 0; j < meshOutline.vertices.Length; j++)
            {
                vertices.Add(position + segment.bezier.ReturnPositionBasedOnPosition(t, meshOutline.vertices[j].point));
                normals.Add(segment.bezier.ReturnPositionBasedOnVector(t, meshOutline.vertices[j].normal));
            }
        }
    }

    public void RemovePoints()
    {
        for (int i = 0; i < parts; i++)
        {
            float t = i / (parts - 1f);
            for (int j = 0; j < meshOutline.vertices.Length; j++)
            {
                vertices.RemoveAt(j);
                normals.RemoveAt(j);
            }
        }
    }

    public void UpdateMesh()
    {
        mesh.vertices = vertices.ToArray();
        AddTriangles();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
    }


    public void AddTriangles()
    {
        triangles.Clear();
        for (int segment = 0; segment < segmentCount + 1; segment++)
        {
            for (int i = (segment * (parts - 1)); i < (segment * (parts - 1)) + (parts - 1); i++)
            {
                int index = i * meshOutline.vertices.Length;

                int nextIndex = (i + 1) * meshOutline.vertices.Length;

                for (int j = 0; j < meshOutline.lineIndicies.Length; j += 2)
                {
                    int firstLine = meshOutline.lineIndicies[j];
                    int secondLine = meshOutline.lineIndicies[j + 1];

                    int currentFirst = index + firstLine;
                    int currentSecond = index + secondLine;
                    int nextFirst = nextIndex + firstLine;
                    int nextSecond = nextIndex + secondLine;

                    triangles.Add(currentFirst);
                    triangles.Add(nextFirst);
                    triangles.Add(nextSecond);

                    triangles.Add(currentFirst);
                    triangles.Add(nextSecond);
                    triangles.Add(currentSecond);
                }
            }
        }
    }

    public void PlaceProps(Segment segment)
    {
        float length = (segment.bezier.endPoint - segment.bezier.startPoint).magnitude / density;
        GameObject parentObject = new GameObject();
        parentObject.transform.position = segment.transform.position;
        instancedProps.Add(parentObject);

        for (int i = 0; i < length; i++)
        {
            for (int j = 1; j < layers + 1; j++)
            {
                Vector3 pos1 = segment.transform.position + segment.bezier.ReturnPositionBasedOnPosition((float)i / length, new Vector3(1f, 0f, j / (layers * 10)) * Random.Range(minDistance, maxDistance) * j);
                InstantiateProp(pos1, new Vector3(0f, Random.Range(0f, 360f), 0f), parentObject.transform);

                Vector3 pos2 = segment.transform.position + segment.bezier.ReturnPositionBasedOnPosition((float)i / length, new Vector3(-1f, 0f, j / (layers * 10)) * Random.Range(minDistance, maxDistance) * j);
                InstantiateProp(pos2, new Vector3(0f, Random.Range(0f, 360f), 0f), parentObject.transform);
            }
        }
    }


    public void InstantiateProp(Vector3 position, Vector3 rotation, Transform parent)
    {
        GameObject prop = Instantiate(props[Random.Range(0, props.Count)]);
        prop.transform.position = position;
        prop.transform.eulerAngles = rotation;
        prop.transform.parent = parent;
    }

    public List<Segment> GetSegments()
    {
        return segments;
    }
    void OnDrawGizmos()
    {
        /*for(int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.5f);

            for (int j = 0; j < triangles.Count - 1; j++)
            {
                Gizmos.DrawLine(vertices[triangles[j]], vertices[triangles[j + 1]]);
            }
        }*/
    }
}
