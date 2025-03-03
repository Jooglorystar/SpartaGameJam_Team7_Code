using System.Linq;
using UnityEngine;

public class TowerObjectController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public Transform shootPoint;

    private float baseAnimationLength = 1.0f; // 기본 애니메이션 길이 (Animator에서 가져옴)

    GenericTower _tower;

    public void Inject(GenericTower tower)
    {
        _tower = tower;

        AnimationClip clip = _animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name.Contains("Attack")); // "Attack" 이름 포함된 클립 찾기

        if (clip != null)
            baseAnimationLength = clip.length;
    }

    public void SetTrigger(string parameter)
    {
        _animator.SetTrigger(parameter);

        float attackInterval = 1f / _tower.TowerData.TowerAttackSpeed;
        _animator.speed = baseAnimationLength / attackInterval;
    }
}
