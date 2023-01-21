using UnityEngine;

public class WireOptions : StaticInstanceBehaviour<WireOptions>
{
    public Material WireMaterial = null;
    public int Segments = 16;
    public float Width = 0.035F;
    public float GravityPower = 1.0F;
    public float GravityPerUnit = 1.0F;
    public float MinimalGravity = 0.25F;
    public float MaximalGravity = 1.0F;
}
