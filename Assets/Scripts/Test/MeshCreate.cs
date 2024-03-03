using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Test
{
    public class MeshCreate : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, FolderPath] private string path;

        [SerializeField] private Mesh cubeMesh;


        // Start is called before the first frame update
        void Start()
        {
            //八面体
            var verts = new List<Vector3>();
            for (int i = 0; i < 4; i++)
            {
                verts.Add(new Vector3(0, -1, 0));
                verts.Add(new Vector3(-1, 0, 0));
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(0, 0, -1));
                verts.Add(new Vector3(0, 0, 1));
            }

            var triangles = new List<int>();
            triangles.AddRange(new[] { 0, 4, 3 });
            triangles.AddRange(new[] { 6, 1, 10 });
            triangles.AddRange(new[] { 7, 2, 16 });
            triangles.AddRange(new[] { 8, 9, 22 });

            triangles.AddRange(new[] { 12, 5, 13 });
            triangles.AddRange(new[] { 19, 11, 14 });
            triangles.AddRange(new[] { 20, 17, 15 });
            triangles.AddRange(new[] { 21, 23, 18 });


            /*
            //四面体
            var verts = new List<Vector3>();
            for (int i = 0; i < 3; i++)
            {
                verts.Add(new Vector3(-1, -1, 1));
                verts.Add(new Vector3(-1, 1, -1));
                verts.Add(new Vector3(1, 1, 1));
                verts.Add(new Vector3(1, -1, -1));
            }


            var triangles = new List<int>();
            triangles.AddRange(new[] { 0, 1, 3 });
            triangles.AddRange(new[] { 5, 2, 7 });
            triangles.AddRange(new[] { 4, 6, 9 });
            triangles.AddRange(new[] { 8, 11, 10 });
            */


            var meshFilter = GetComponent<MeshFilter>();

            var mesh = new Mesh();


            mesh.SetVertices(verts);
            mesh.SetTriangles(triangles, 0);
            //
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            SaveMesh(mesh);
        }

        private void SaveMesh(Mesh mesh)
        {
            AssetDatabase.CreateAsset(mesh, path + "/Octahedron.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}