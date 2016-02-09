using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player playerOrigin;

    public float ageLimit = 4;
    
    new Collider2D collider;

    float age;
    bool markedForDestroy;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        // Awalnya isTrigger == true, dimatikan di OnTriggerExit2D.
        collider.isTrigger = true;

        age = 0;
        markedForDestroy = false;
    }

    void Update()
    {
        age += Time.deltaTime;

        // Jika setelah beberapa detik masih belum hancur, berarti tidak kena player.
        if (age > ageLimit && !markedForDestroy)
        {
            NotifyThrowAndDestroy();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Matikan trigger setelah tidak lagi kena playerOrigin.
        if (other.gameObject == playerOrigin.gameObject)
        {
            collider.isTrigger = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // player selain playerOrigin berarti kena player lain.
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null && player != playerOrigin)
        {
            // TODO Ganti efek healthnya, untuk sekarang baru health kurang nilai konstan.
            player.health -= 20;

            // Matikan trigger supaya tidak kena player manapun lagi.
            collider.isTrigger = true;
        }
    }

    void NotifyThrowAndDestroy()
    {
        // Panggil NotifyThrow di GameController.
        GameObject.FindGameObjectWithTag("GameController").SendMessage("NotifyThrow");

        // Destroy setelah beberapa detik.
        Destroy(gameObject, 1);

        markedForDestroy = true;
    }
}
