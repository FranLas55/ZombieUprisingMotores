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
    [SerializeField] private string onDeathName = "onDeath";
    [SerializeField] private string canMoveName = "canMove";
    [SerializeField] private string onStopName = "onStop";
    [SerializeField] private string hasGrabbedName = "hasGrabbed";

    public void Punch()
    {
        print("OnPunch triggered");
        _animator.SetTrigger(onPunchName);
    }
    
    public void OnStart()
    {
        print("OnStart triggered");
        _animator.SetTrigger(onStartName);
    }
    
    public void FloorAttack()
    {
        print("OnFloorAttack triggered");
        _animator.SetTrigger(onFloorAttackName);
    }

    public void Grab()
    {
        print("OnGrab triggered");
        _animator.SetTrigger(onGrabName);
    }

    public void Run()
    {
        print("Run true");
        _animator.SetBool(isRunningName, true);
    }

    public void OnDeath()
    {
        _animator.SetTrigger(onDeathName);
    }

    public void SetMove(bool can)
    {
        _animator.SetBool(canMoveName, can);
    }

    public void OnStop()
    {
        _animator.SetTrigger(onStopName);
    }

    public void SetGrabbed(bool has)
    {
        _animator.SetBool(hasGrabbedName, has);
    }
}