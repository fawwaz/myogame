using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bulletscript : MonoBehaviour {
    public GameObject bulletObj;
    public Transform bulletParent;
    public static float bulletSpeed = 20f;
    public static int bulletDamage = 10;

    public Slider throwpowerp1;
    public Slider throwpowerp2;

    void Start()
    {
        StartCoroutine(FireBullet());
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
    }

    public IEnumerator FireBullet()
    {
        while (true)
        {
            //Create a new bullet
            GameObject newBullet = Instantiate(bulletObj, transform.position, transform.rotation) as GameObject;

            //Parent it to get a less messy workspace
            newBullet.transform.parent = bulletParent;

            //Add velocity to the bullet with a rigidbody
            newBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * transform.forward;

            yield return new WaitForSeconds(2f);
        }
    }
}
