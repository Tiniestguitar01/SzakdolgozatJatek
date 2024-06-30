using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MeshOutline: ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        public float uv;
    }

    public Vertex[] vertices;
    public int[] lineIndicies;
}
