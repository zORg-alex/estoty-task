using UnityEngine;

public class BallDestroyer : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Destroy(other.gameObject);
	}
}