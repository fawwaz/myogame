using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, 100)]
    public int health;
    
    /// <summary>
    /// Melempar sebuah bullet ke arah angle dan strength tertentu.
    /// </summary>
    /// <param name="bulletPrefab">Prefab bullet yang dilempar. Harus ada Rigidbody2D.</param>
    /// <param name="angle">Arah lemparan dalam derajat [0..360].</param>
    /// <param name="strength">Kekuatan lemparan awal.</param>
    public void FireBullet(GameObject bulletPrefab, float angle, float strength)
    {
        // Instantiate bullet.
        var bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        var bulletBody = bulletObject.GetComponent<Rigidbody2D>();
        var bullet = bulletObject.GetComponent<Bullet>();

        // (angle, strength) -> velocity untuk bullet.
        var angleRadian = angle * Mathf.Deg2Rad;
        var velocity = strength * new Vector2(Mathf.Cos(angleRadian), Mathf.Sin(angleRadian));

        Debug.Log(angle + " " + velocity);

        bullet.playerOrigin = this;

        bulletBody.velocity = velocity;
        bulletBody.angularVelocity = angle; // tambahkan putaran supaya nggak aneh :3
    }
}
