using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Header("ParameterNames")] 
    [SerializeField] private string onStartName = "onStart";
    [SerializeField] private string onPunchName = "onPunch";
    [SerializeField] private string onFloorAttackName = "onFloorAttack";
    [SerializeField] private string onGrabName = "onGrab";
    [SerializeField] private string isRunningName = "isRunning";

    public void Punch()
    {
        _animator.SetTrigger(onPunchName);
    }
    
    public void OnStart()
    {
        _animator.SetTrigger(onStartName);
    }
    
    public void FloorAttack()
    {
        _animator.SetTrigger(onFloorAttackName);
    }

    public void Grab()
    {
        _animator.SetTrigger(onGrabName);
    }

    public void Run()
    {
        _animator.SetBool(isRunningName, true);
    }
}