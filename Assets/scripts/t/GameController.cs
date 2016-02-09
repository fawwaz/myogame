using System.Collections;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class GameController : MonoBehaviour
{
    public GameObject bulletPrefab;

	public GameObject myo1 = null;
	public GameObject myo2 = null;

	private Pose _lastPose = Pose.Unknown;

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
				var myo = state == GameState.P1Turn ? myo1 : myo2;
				ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
				if (thalmicMyo.pose == Pose.Fist && holdAction == null)
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
		var myo = state == GameState.P1Turn ? myo1 : myo2;
		var strength = 1.0f;
        
		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.

		/*
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			// Vibrate the Myo armband when a fist is made.
			if (thalmicMyo.pose == Pose.Fist) {
				thalmicMyo.Vibrate (VibrationType.Medium);

				ExtendUnlockAndNotifyUserAction (thalmicMyo);

				// Change material when wave in, wave out or double tap poses are made.
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				GetComponent<Renderer>().material = waveInMaterial;

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				GetComponent<Renderer>().material = waveOutMaterial;

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.DoubleTap) {
				GetComponent<Renderer>().material = doubleTapMaterial;

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			}
		}
		*/

        // strength 1 -> 10
		Debug.Log("Myo Position"+ thalmicMyo.pose+ " hai ");
		while (thalmicMyo.pose == Pose.Fist && strength < 20)
        {
            strength += strengthSpeed * Time.deltaTime;
            player.strength = strength;
            
            yield return null;
        }
        
        // Jika ternyata masih ditahan, strength 10 -> 1
		if (thalmicMyo.pose == Pose.Fist)
        {
			while (thalmicMyo.pose == Pose.Fist && strength > 0)
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
