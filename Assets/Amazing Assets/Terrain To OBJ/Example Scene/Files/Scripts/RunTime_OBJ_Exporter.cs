#if UNITY_EDITOR

using UnityEngine;
using AmazingAssets.TerrainToOBJ;

namespace AmazingAssets.TerrainToOBJ.Examples
{
    public class RunTime_OBJ_Exporter : MonoBehaviour
    {
        public Terrain terrain;

        [Space(5)]
        public int vertexCountHorizontal = 100;
        public int vertexCountVertical = 100;

        [Space(5)]
        public int chunkCountHorizontal = 3;
        public int chunkCountVertical = 3;

        [Space(5)]
        public bool perChunkUV = true;
        public AmazingAssets.TerrainToOBJ.Normal normal = AmazingAssets.TerrainToOBJ.Normal.CalculateFromMesh;
        public bool originOffset = false;


        void Start()
        {
            if (terrain != null && terrain.terrainData != null)
            {
                //Origin position
                Vector3 originPosition = originOffset ? terrain.transform.position : Vector3.zero;



                //Converting terrain into a single OBJ mesh and using StreamWriter save it into a file
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Application.dataPath + "/" + terrain.terrainData.name + ".obj"))
                {
                    terrain.terrainData.ToOBJStreamWriter(streamWriter, vertexCountHorizontal, vertexCountVertical, normal, originPosition);
                }


                //Converting terrain into multi-piece OBJ mesh and using StreamWriter save it into a file
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Application.dataPath + "/" + terrain.terrainData.name + " (Submesh).obj"))
                {
                    terrain.terrainData.ToOBJStreamWriter(streamWriter, vertexCountHorizontal, vertexCountVertical, chunkCountHorizontal, chunkCountVertical, perChunkUV, normal, originPosition);
                }



                UnityEditor.AssetDatabase.Refresh();

                Debug.Log("Two generated OBJ files are saved inside the project root (Assets) folder.\n");
            }
        }
    }
}
#endif