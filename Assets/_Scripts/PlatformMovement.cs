using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour 
{
    [SerializeField, Min(0f)] private float _maxPlatformPositionX;

    private Vector3 _platformPosition;
    private float _localScaleX;
    private Ray _ray;
    private Camera _camera;

    private void Awake()
    {
        _platformPosition = transform.position;
        _camera = Camera.main;
        _localScaleX = transform.localScale.x;
    }
    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        float distance = Vector3.Distance(_camera.transform.position, _platformPosition);
        _platformPosition.x = _ray.GetPoint(distance).x;

        if (_platformPosition.x + _localScaleX / 2 > _maxPlatformPositionX)
        {
            _platformPosition.x = _maxPlatformPositionX - _localScaleX / 2;
        }
        if (_platformPosition.x - _localScaleX / 2 < -_maxPlatformPositionX)
        {
            _platformPosition.x = -_maxPlatformPositionX + _localScaleX / 2;
        }
        transform.position = _platformPosition;
    }
}
