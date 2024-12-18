using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControllerAvatar : MonoBehaviour
{
    private BossController _controller;

    private void Start()
    {
        _controller = GetComponentInParent<BossController>();
    }

    public void Punch()
    {
        _controller.Punch();
    }

    public void Grab()
    {
        _controller.Grab();
    }

    public void FloorAttack()
    {
        _controller.FloorAttack();
    }

    public void ChangePos()
    {
        _controller.ChangePos();
    }

    public void Launch()
    {
        _controller.Launch();
    }

    public void Finish()
    {
        _controller.FinishAttack();
    }

    public void StepAudio()
    {
        _controller.StepAudio();
    }

    public void GroundGrowl()
    {
        _controller.GroundGrowl();
    }

    public void GameOver()
    {
        _controller.GameOver();
    }
}
