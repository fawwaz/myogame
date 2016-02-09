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
    
    public Player player1;

    public Player player2;

    public GameState state;
    
    void Update()
    {
        switch (state)
        {
            case GameState.P1Turn:
            case GameState.P2Turn:

                // TODO Ganti dengan input yang benar (baca dari myo).
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

                break;
            case GameState.P1Throw:
            case GameState.P2Throw:

                // Perubahan state terjadi di NotifyThrow, dipanggil oleh bullet.

                break;
            case GameState.P1Win:
            case GameState.P2Win:

                // Kembali ke state awal.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = GameState.P1Turn;
                }

                break;
            default: break;
        }
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
