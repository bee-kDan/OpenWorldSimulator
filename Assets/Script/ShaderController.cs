using UnityEngine;

public class VegetationShaderController : MonoBehaviour
{
    public static VegetationShaderController Instance;

    [Header("Wind")]
    public float bendStrength = 2f;
    public float bendSpeed = 1f;
    public float bendDistortion = 3f;
    public Vector2 bendDirection = new Vector2(4f, 4f);

    [Header("Wiggle")]
    public float wiggleStrength = 0f;
    public float wiggleSpeed = 0f;

    public bool windEnabled = true;

    private static readonly int BendStrength =
        Shader.PropertyToID("_Bend_Strength");

    private static readonly int BendSpeed =
        Shader.PropertyToID("_Bend_Speed");

    private static readonly int BendDistortion =
        Shader.PropertyToID("_Bend_Distortion");

    private static readonly int BendDirection =
        Shader.PropertyToID("_Bend_Direction");

    private static readonly int WiggleStrength =
        Shader.PropertyToID("_Wiggle_Strength");

    private static readonly int WiggleSpeed =
        Shader.PropertyToID("_Wiggle_Speed");


    private void Awake()
    {
        Instance = this;
        ApplyWind();
    }


    public void ApplyWind()
    {
        float strength = windEnabled ? bendStrength : 0f;
        float speed = windEnabled ? bendSpeed : 0f;
        float distortion = windEnabled ? bendDistortion : 0f;

        float wiggle = windEnabled ? wiggleStrength : 0f;
        float wiggleSpeedValue = windEnabled ? wiggleSpeed : 0f;

        Shader.SetGlobalFloat(BendStrength, strength);
        Shader.SetGlobalFloat(BendSpeed, speed);
        Shader.SetGlobalFloat(BendDistortion, distortion);

        Shader.SetGlobalVector(
            BendDirection,
            new Vector4(
                bendDirection.x,
                bendDirection.y,
                0f,
                0f
            )
        );

        Shader.SetGlobalFloat(WiggleStrength, wiggle);
        Shader.SetGlobalFloat(WiggleSpeed, wiggleSpeedValue);
    }


    public void StopWind()
    {
        windEnabled = false;
        ApplyWind();
    }


    public void StartWind()
    {
        windEnabled = true;
        ApplyWind();
    }


    public void SetWindStrength(float value)
    {
        bendStrength = value;
        ApplyWind();
    }


    public void SetWindSpeed(float value)
    {
        bendSpeed = value;
        ApplyWind();
    }


    public void SetWindDirection(Vector2 value)
    {
        bendDirection = value;
        ApplyWind();
    }
}