using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _xSensibility = 100f;
    [SerializeField] float _ySensibility = 150f;

    [Header("References")]
    [SerializeField] Transform _player;
    [SerializeField] private bool _disableCursor = true;

    private float _mouseX, _mouseY, _xRotation, _yRotation;

    private Vector3 _offset = new();

    void Start()
    {
        _offset = transform.position;
        Cursor.visible = _disableCursor;
    }

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _mouseX = Input.GetAxis("Mouse X") * _xSensibility * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * _ySensibility * Time.deltaTime;

        transform.position = _player.position + _offset;

        _xRotation -= _mouseY;
        _yRotation += _mouseX;

        _xRotation = Mathf.Clamp(_xRotation, -50f, 75f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        _player.Rotate(Vector3.up * _mouseX); //Rotacion en Y
    }
}
