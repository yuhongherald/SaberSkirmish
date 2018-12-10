using UnityEngine;

public class TestShader : MonoBehaviour
{
    public Material material;
    void Start()
    {
        int numChild = gameObject.transform.childCount;
        Material copyMaterial = new Material(material);

        MeshRenderer renderer;
        for (int i = 0; i < numChild; i++)
        {
            renderer = gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>();
            if (renderer)
            {
                renderer.sharedMaterial = copyMaterial;
            }
        }
    }
}
