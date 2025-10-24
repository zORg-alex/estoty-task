using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ContactSounds : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip[] impactClips;

    [Header("Impact thresholds")]
    [SerializeField] private float minImpactToPlay = 1f;
    [SerializeField] private float maxImpactForFullVolume = 10f;

    [Header("Volume & Pitch")]
    [SerializeField, Range(0f,1f)] private float minVolume = 0.1f;
    [SerializeField, Range(0f,1f)] private float maxVolume = 1f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    [Header("Filtering")]
    [SerializeField] private LayerMask collisionLayerMask = ~0; // default: everything
    [SerializeField] private bool useCollisionImpulse = true;   // uses collision.impulse / fixedDeltaTime when available

    private AudioSource _audio;
    private Rigidbody _rb;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();

        // Ensure sensible AudioSource defaults
        _audio.playOnAwake = false;
        _audio.spatialBlend = 1f; // 3D sound
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collisionLayerMask.value & (1 << collision.gameObject.layer)) == 0) return;

        float impact = 0f;

        if (useCollisionImpulse && collision.impulse.sqrMagnitude > 0f)
        {
            // impulse is force applied during collision; divide by fixedDeltaTime to get approximate instantaneous "velocity-like" measure
            impact = collision.impulse.magnitude / Mathf.Max(Time.fixedDeltaTime, 0.0001f);
        }
        else
        {
            impact = collision.relativeVelocity.magnitude;
        }

        if (impact < minImpactToPlay) return;

        float t = Mathf.InverseLerp(minImpactToPlay, maxImpactForFullVolume, impact);
        float volume = Mathf.Lerp(minVolume, maxVolume, t);
        float pitch = Random.Range(pitchRange.x, pitchRange.y);

        var clip = PickRandomClip();
        if (clip == null) return;

        _audio.pitch = pitch;
        _audio.PlayOneShot(clip, volume);
    }

    private AudioClip PickRandomClip()
    {
        if (impactClips == null || impactClips.Length == 0) return null;
        return impactClips[Random.Range(0, impactClips.Length)];
    }
}
