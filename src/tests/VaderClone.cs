using UnityEngine;

[ExecuteInEditMode]
public class VaderClone : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        int children = gameObject.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            SkinnedMeshRenderer renderer = obj.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null)
            {
                continue;
            }
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mr.sharedMaterial = renderer.sharedMaterial;
            MeshFilter filter = obj.GetComponent<MeshFilter>();
            if (filter == null)
            {
                filter = obj.AddComponent<MeshFilter>();
            }
            filter.mesh = renderer.sharedMesh;
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
