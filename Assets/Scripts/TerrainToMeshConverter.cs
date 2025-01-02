using UnityEngine;

public class TerrainToMeshConverter : MonoBehaviour
{
    public Terrain terrain;
    public int resolution = 256; // Higher resolution = more detail

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No terrain assigned!");
            return;
        }

        Mesh mesh = GenerateMeshFromTerrain(terrain, resolution);
        GameObject meshObject = new GameObject("Rotatable Terrain", typeof(MeshFilter), typeof(MeshRenderer));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = terrain.materialTemplate;
        meshObject.transform.position = terrain.transform.position;

        // Optionally assign a MeshCollider
        meshObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }

    Mesh GenerateMeshFromTerrain(Terrain terrain, int resolution)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        float[,] heights = terrainData.GetHeights(0, 0, resolution, resolution);

        int vertCount = resolution * resolution;
        int triCount = (resolution - 1) * (resolution - 1) * 6;

        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uv = new Vector2[vertCount];
        int[] triangles = new int[triCount];

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int index = z * resolution + x;

                float height = heights[z, x] * terrainSize.y;
                vertices[index] = new Vector3(
                    (float)x / (resolution - 1) * terrainSize.x,
                    height,
                    (float)z / (resolution - 1) * terrainSize.z
                );

                normals[index] = Vector3.up; // Flat normals for simplicity
                uv[index] = new Vector2(
                    (float)x / (resolution - 1),
                    (float)z / (resolution - 1)
                );
            }
        }

        int triangleIndex = 0;
        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int bottomLeft = z * resolution + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + resolution;
                int topRight = topLeft + 1;

                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = topRight;

                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };
        mesh.RecalculateNormals();

        return mesh;
    }
}
