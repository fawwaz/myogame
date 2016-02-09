using UnityEngine;

[RequireComponent(typeof(AreaEffector2D))]
public class Wind : MonoBehaviour
{
    public float maximumStrength;

    [Range(-1, 1)]
    public float strength;

    AreaEffector2D effector;

    void Awake()
    {
        effector = GetComponent<AreaEffector2D>();
    }

    void Update()
    {
        effector.forceAngle = strength > 0 ? 0 : -180;
        effector.forceMagnitude = maximumStrength * Mathf.Abs(strength);
    }
}
