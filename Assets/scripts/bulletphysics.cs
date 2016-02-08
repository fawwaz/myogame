using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bulletphysics : MonoBehaviour {
    public Slider windslider;
    public int windmultiplier;
	// Use this for initialization
	void Start () {
	
	}

    public static void Heuns(
    float h,
    Vector2 currentPosition,
    Vector2 currentVelocity,
    out Vector2 newPosition,
    out Vector2 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector2 acceleartionFactorEuler = Physics.gravity;
        Vector2 acceleartionFactorHeun = Physics.gravity;


        //Init velocity
        //Current velocity
        Vector2 velocityFactor = currentVelocity;
        //Wind velocity
        velocityFactor += new Vector2(2f, 0f);


        //
        //Main algorithm
        //
        //Euler forward
        Vector2 pos_E = currentPosition + h * velocityFactor;

        acceleartionFactorEuler += CalculateDrag(currentVelocity);

        Vector2 vel_E = currentVelocity + h * acceleartionFactorEuler;


        //Heuns method
        Vector2 pos_H = currentPosition + h * 0.5f * (velocityFactor + vel_E);

        acceleartionFactorHeun += CalculateDrag(vel_E);

        Vector2 vel_H = currentVelocity + h * 0.5f * (acceleartionFactorEuler + acceleartionFactorHeun);


        newPosition = pos_H;
        newVelocity = vel_H;
    }

    public static Vector2 CalculateDrag(Vector2 velocityVec)
    {
        //F_drag = k * v^2 = m * a
        //k = 0.5 * C_d * rho * A 

        float m = 0.2f; // kg
        float C_d = 0.5f;
        float A = Mathf.PI * 0.05f * 0.05f; // m^2
        float rho = 1.225f; // kg/m3

        float k = 0.5f * C_d * rho * A;

        float vSqr = velocityVec.sqrMagnitude;

        float aDrag = (k * vSqr) / m;

        //Has to be in a direction opposite of the bullet's velocity vector
        Vector2 dragVec = aDrag * velocityVec.normalized * -1f;

        return dragVec;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
