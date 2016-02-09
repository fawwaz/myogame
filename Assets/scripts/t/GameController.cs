using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bulletPrefab;

    public enum GameState
    {
        P1Turn = 0,
        P2Turn = 1,
        P1Win = 2,
        P2Win = 3,
        P1Throw = 4,
        P2Throw = 5,
    }
    
    public float dummyInputAngle;
    public float dummyInputStrength;

    public float strengthSpeed;

    public Player player1;

    public Player player2;

    public Wind windZone;

    public GameState state;

    Coroutine holdAction;
    
    void Update()
    {
        // Jika nyawa habis, ganti ke state GameOver.
        if (player1.health <= 0)
        {
            state = GameState.P2Win;
        }
        else if (player2.health <= 0)
        {
            state = GameState.P1Win;
        }

        switch (state)
        {
            case GameState.P1Turn:
            case GameState.P2Turn:

                // Reset strength player.
                player1.strength = player2.strength = 1;

                // TODO Bikin update angin yang lebih baik.
                windZone.strength = Mathf.Clamp(windZone.strength + Random.Range(-0.1f, 0.1f), -1, 1);

                // TODO Ganti dengan input yang benar (baca dari myo).
                /*
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Untuk P2, jangan lupa angle dibalik.
                    var inputAngle = state == GameState.P1Turn ? dummyInputAngle : 180 - dummyInputAngle;

                    var inputStrength = dummyInputStrength;
                    
                    var player = state == GameState.P1Turn ? player1 : player2;
                    player.FireBullet(bulletPrefab, inputAngle, inputStrength);

                    // Ganti state.
                    state = state == GameState.P1Turn ? GameState.P1Throw : GameState.P2Throw;
                }
                */

                if (Input.GetKeyDown(KeyCode.Space) && holdAction == null)
                {
                    holdAction = StartCoroutine(HoldAction());
                }

                break;
            case GameState.P1Throw:
            case GameState.P2Throw:

                // Perubahan state terjadi di NotifyThrow, dipanggil oleh bullet.

                break;
            case GameState.P1Win:
            case GameState.P2Win:

                // Reset game; kembali ke state awal.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player1.health = player2.health = 100;
                    state = GameState.P1Turn;
                }

                break;
            default: break;
        }
    }

    IEnumerator HoldAction()
    {
        // TODO Minimum dan maximum strength masih di-hardcode.

        var player = state == GameState.P1Turn ? player1 : player2;
        var strength = 1.0f;
        
        // strength 1 -> 10
        while (Input.GetKey(KeyCode.Space) && strength < 20)
        {
            strength += strengthSpeed * Time.deltaTime;
            player.strength = strength;
            
            yield return null;
        }
        
        // Jika ternyata masih ditahan, strength 10 -> 1
        if (Input.GetKey(KeyCode.Space))
        {
            while (Input.GetKey(KeyCode.Space) && strength > 0)
            {
                strength -= strengthSpeed * Time.deltaTime;
                player.strength = strength;

                yield return null;
            }
        }
        
        // Untuk P2, jangan lupa angle dibalik.
        var angle = state == GameState.P1Turn ? 45 : 135;

        Throw(angle, strength);

        holdAction = null;
    }

    void Throw(float angle, float strength)
    {
        // Lempar bullet.
        var player = state == GameState.P1Turn ? player1 : player2;
        player.FireBullet(bulletPrefab, angle, strength);

        // Ganti state.
        state = state == GameState.P1Turn ? GameState.P1Throw : GameState.P2Throw;
    }

    public void NotifyThrow()
    {
        StartCoroutine(ThrowCompleteCoroutine());
    }

    IEnumerator ThrowCompleteCoroutine()
    {
        // TODO mungkin ada pesan tambahan (misalnya nyawa yang berkurang berapa, dll.).
        // Sekarang baru nunggu 1 detik lalu ganti state.

        yield return new WaitForSeconds(1);

        // Asumsi: state == P1Throw atau P2Throw.
        state = state == GameState.P1Throw ? GameState.P2Turn : GameState.P1Turn;
    }
}
