using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : SingletonWithoutDonDestroy<VFXManager>
{
    [SerializeField] ParticleSystem[] _vfx;

    Dictionary<string, ParticleSystem> _dic = new Dictionary<string, ParticleSystem>();
    Dictionary<string, Queue<ParticleSystem>> _pool = new Dictionary<string, Queue<ParticleSystem>>();


    private void Awake()
    {
        foreach(ParticleSystem particleSystem in _vfx)
        {
            _dic[particleSystem.gameObject.name] = particleSystem;
        }
    }

    public ParticleSystem GetPool(string vfxName)
    {
        if(_pool.TryGetValue(vfxName, out var queue))
        {
            if(queue.Count <= 0)
            {
                queue.Enqueue(CreateVfx(vfxName));
            }
        }
        else
        {
            _pool[vfxName] = new Queue<ParticleSystem>();
            _pool[vfxName].Enqueue(CreateVfx(vfxName));

            queue = _pool[vfxName];
        }

        ParticleSystem result = queue.Dequeue();
        result.transform.position = Vector3.one;
        result.gameObject.SetActive(true);
        return result;
    }

    public void OutPool(ParticleSystem vfx)
    {
        vfx.gameObject.SetActive(false);
        _pool[vfx.name].Enqueue(vfx);
    }

    ParticleSystem CreateVfx(string vfxName)
    {
        if(_dic.TryGetValue(vfxName, out ParticleSystem vfx))
        {
            ParticleSystem instance = Instantiate(vfx);
            instance.name = vfxName;
            instance.gameObject.SetActive(false);
            return instance;
        }
        return null;
    }
}
