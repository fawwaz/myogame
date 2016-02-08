using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gamecontroller : MonoBehaviour {
    public enum turns
    {
        P1TURN,P2TURN,P1WIN,P2WIN
    }
    public GameObject pleft; //left player
    public GameObject pright; // right player
    public GameObject bullet;

    public float windstrength = 1;
    public static Vector2 windspeed;
    public turns currturn;
    public Text turntext;
	// Use this for initialization
	void Start () {
        windspeed = new Vector2(windstrength, 0f);
	    pleft = GameObject.FindGameObjectWithTag ("p2");
        pright = GameObject.FindGameObjectWithTag("p1");
        bullet = GameObject.FindGameObjectWithTag("bullet");
	}
	
	// Update is called once per frame
	void Update () {
        switch (currturn)
        {
            case(turns.P1TURN):
                turntext.text = "Player 1's turn";
                break;
            case(turns.P2TURN):
                turntext.text = "Player 2's turn";
                break;
            case(turns.P1WIN):
                turntext.text = "Player 1 has won the game!";
                break;
            case(turns.P2WIN):
                turntext.text = "Player 2 has won the game!";
                break;
        }
	}

    void FireBulletPLeft()
    {
        //Sometimes when we move the mouse is really high in the webplayer, so the mouse cursor ends up outside
        //of the webplayer so we cant fire, despite locking the cursor, so add alternative fire button
        if ((Input.GetKeyDown(KeyCode.F)) && currturn == turns.P2TURN)
        {
            //Create a new bullet
            GameObject newBullet = Instantiate(bullet, pleft.transform.position, pleft.transform.rotation) as GameObject;

            //Give it speed
            //newBullet.GetComponent<TutorialBullet>().currentVelocity = bulletscript.bulletSpeed;
        }
        currturn = turns.P1TURN;
    }
    void FireBulletPRight()
    {
        //Sometimes when we move the mouse is really high in the webplayer, so the mouse cursor ends up outside
        //of the webplayer so we cant fire, despite locking the cursor, so add alternative fire button
        if ((Input.GetKeyDown(KeyCode.F)) && currturn == turns.P2TURN)
        {
            //Create a new bullet
            GameObject newBullet = Instantiate(bullet, pleft.transform.position, pleft.transform.rotation) as GameObject;

            //Give it speed
            //newBullet.GetComponent<TutorialBullet>().currentVelocity = bulletscript.bulletSpeed;
        }
        currturn = turns.P2TURN;
    }
}
