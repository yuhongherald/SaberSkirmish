using UnityEngine;

/// <summary>
/// An animated arrow in 3D space to guide the player in a direction. Arrow curve
/// is modelled and controlled as an editable bezier curve.
/// </summary>
[ExecuteInEditMode]
public class DirectionalArrow : MonoBehaviour
{
    [SerializeField]
    private Vector3[] controlPoints = { new Vector3(-1, -1f, 0), new Vector3(0.6f, -0.8f, 0),
        new Vector3(1, -0.2f, 0), new Vector3(1, 1, 0)};
    [SerializeField]
    private float minLength = 0.3f;
    [SerializeField]
    private float maxLength = 0.4f;

    [SerializeField]
    private float minWidth = 0.4f;
    [SerializeField]
    private float maxWidth = 0.8f;

    [SerializeField]
    private float tailWidth = 1.0f;
    [SerializeField]
    private float headWidth = 0.2f;

    [SerializeField]
    private Color color1 = new Color(255.0f / 255.0f, 196 / 255.0f, 131.0f / 255.0f);
    [SerializeField]
    private Color color2 = new Color(255.0f / 255.0f, 158 / 255.0f, 0.0f / 255.0f);

    private const int NUM_FRAMES = 40;
    private const int HALF_FRAMES = NUM_FRAMES / 2;

    private const int RESOLUTION = 20;
    private const int ARROW_RESOLUTION = 4;
    private const string VERTEX_SHADER = "Custom/VertexColor";
    private int currentFrame = 0;
    private float startT = 0;
    private float endT = 1;
    private Mesh mesh;
    private bool animate = true;

    /// <summary>
    /// Resets the animation frame of the arrow.
    /// </summary>
    public void Reset()
    {
        currentFrame = 0;
    }

    /// <summary>
    /// Sets the current animation state.
    /// </summary>
    /// <param name="animate">Animation state to be set.</param>
    public void SetAnimate(bool animate)
    {
        this.animate = animate;
    }

    private void Start()
    {
        InitShader();
        InitMesh();
        InitFilter();
        InitStartParametric();
    }

    private void InitStartParametric()
    {
        startT = minLength / 2;
        endT = 1.0f - minLength / 2;
    }

    private void InitFilter()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = mesh;
    }

    private void InitMesh()
    {
        mesh = new Mesh();
        InitVertices();
        InitTriangles();
        InitColors();
    }

    private void InitColors()
    {
        Color[] colors = new Color[2 * RESOLUTION + 2];
        for (int i = 0; i < 2 * RESOLUTION + 2; i++)
        {
            colors[i] = Color.red;
        }
        mesh.colors = colors;
    }

    private void InitTriangles()
    {
        int[] triangles = new int[3 * 2 * RESOLUTION];
        for (int i = 0; i < RESOLUTION; i++)
        {
            triangles[6 * i] = 2 * i;
            triangles[6 * i + 1] = 2 * i + 1;
            triangles[6 * i + 2] = 2 * i + 2;
            triangles[6 * i + 3] = 2 * i + 1;
            triangles[6 * i + 4] = 2 * i + 3;
            triangles[6 * i + 5] = 2 * i + 2;
        }
        mesh.triangles = triangles;
    }

    private void InitVertices()
    {
        Vector3[] vertices = new Vector3[2 * RESOLUTION + 2];
        for (int i = 0; i < RESOLUTION + 1; i++)
        {
            vertices[2 * i] = new Vector3(0, 0, 0);
            vertices[2 * i + 1] = new Vector3(0, 0, 0);
        }

        mesh.vertices = vertices;
    }

    private void InitShader()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        Shader shader = Shader.Find(VERTEX_SHADER);
        renderer.material = new Material(shader);
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (!animate) return;
        currentFrame = (currentFrame + 1) % NUM_FRAMES;

        float modulatedFrame = Util.Pyramid(HALF_FRAMES, currentFrame);
        float frameFactor = modulatedFrame / (float)HALF_FRAMES;

        float halfHalfFrames = HALF_FRAMES / 2;
        float lengthFactor = Util.Pyramid(halfHalfFrames, modulatedFrame) / (float)halfHalfFrames;

        float baseT = Util.Lerp(startT, endT, frameFactor);
        float currentLength = Util.Lerp(minLength, maxLength, lengthFactor);
        float currentWidth = Util.Lerp(maxWidth, minWidth, lengthFactor);

        UpdateArrowMesh(baseT, currentLength, currentWidth);
    }

    private void UpdateArrowMesh(float baseT, float currentLength, float currentWidth)
    {
        float localStartT = baseT - currentLength / 2;
        float localEndT = baseT + currentLength / 2;
        Vector3[] vertices = new Vector3[2 * RESOLUTION + 2];
        Color[] colors = new Color[2 * RESOLUTION + 2];

        for (int i = 0; i < RESOLUTION + 1; i++)
        {
            float arrowFactor = (float)i / (float)(RESOLUTION + 1);
            float t = Util.Lerp(localStartT, localEndT, arrowFactor);
            Vector3[] velocity = Util.ComputeSpeed(controlPoints, t);
            Vector3 basePoint = Util.ComputeBezier(velocity, t);
            float localWidth;
            if (i < RESOLUTION - ARROW_RESOLUTION)
            {
                localWidth = currentWidth * Mathf.Lerp(tailWidth, headWidth, arrowFactor);
            }
            else
            {
                localWidth = currentWidth * Mathf.Lerp(1, 0,
                    (float)(i - RESOLUTION + ARROW_RESOLUTION) / (float)(ARROW_RESOLUTION + 1));
            }
            vertices[2 * i] = Util.ComputePoint(velocity[1] - velocity[0], basePoint, true, localWidth);
            vertices[2 * i + 1] = Util.ComputePoint(velocity[1] - velocity[0], basePoint, false, localWidth);
            colors[2 * i] = Color.Lerp(color1, color2, t);
            colors[2 * i + 1] = Color.Lerp(color1, color2, t);
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
    }
}
