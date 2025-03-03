using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
public class Vfx : MonoBehaviour
{
    ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        VFXManager.Instance?.OutPool(_particleSystem);
    }
}
