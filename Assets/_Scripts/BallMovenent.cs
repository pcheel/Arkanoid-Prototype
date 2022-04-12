using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class BallMovenent : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _minSpeed;
    [SerializeField, Min(0f)] private float _maxSpeed;
    [SerializeField, Min(0f)] private float _acceleration;
    [SerializeField, Range(0f, 45f)] private float _minPlatformReflectAngle;
    [SerializeField, Range(45f, 90f)] private float _maxPlatformReflectAngle;
    [SerializeField] private GameObject _platform;

    private Vector3 _moveDirection;
    private Vector3 _rotation;
    private float _speed;
    private float _localScaleX;
    private bool _onPlay = false;
    private Rigidbody _body;
    private const int HALFPIDEG = 90;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        float startAngle = Random.Range(-HALFPIDEG + _minPlatformReflectAngle, HALFPIDEG - _minPlatformReflectAngle);
        float newZ = Mathf.Cos(startAngle * Mathf.Deg2Rad);
        float newX = Mathf.Sin(startAngle * Mathf.Deg2Rad);
        _moveDirection = Vector3.forward * newZ + Vector3.right * newX;
        _speed = _minSpeed;
        _localScaleX = transform.localScale.x;
    }
    private void Update()
    {
        if (!_onPlay)
        {
            Vector3 position = transform.position;
            position.x = _platform.transform.position.x;
            transform.position = position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _onPlay = true;
            }
        }
        else
        {
            _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration * Time.deltaTime);
            _body.velocity = _speed * _moveDirection;
            BallRotation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ChangeDirection(collision);
    }

    private void ChangeDirection(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        Vector3 tangent = Vector3.Cross(normal, new Vector3(0, 1, 0));
        PlatformMovement platform = collision.gameObject.GetComponent<PlatformMovement>();
        if (platform == null)
        {
            _moveDirection = Vector3.Reflect(_moveDirection, normal);
        }
        else
        {
            Vector3 contactPoint = collision.GetContact(0).point;
            Vector3 platformPosition = collision.gameObject.transform.position;

            float platformLenght = collision.gameObject.transform.localScale.x;
            float deltaX = Mathf.Abs(Mathf.Abs(contactPoint.x) - Mathf.Abs(platformPosition.x));
            float deltaAngle = _maxPlatformReflectAngle - _minPlatformReflectAngle;
            float newAngle = ((platformLenght / 2 - deltaX)/ (platformLenght / 2)) * deltaAngle + _minPlatformReflectAngle;
            float newX = Mathf.Cos(newAngle * Mathf.Deg2Rad);
            float newZ = Mathf.Sin(newAngle * Mathf.Deg2Rad);

            if (_moveDirection.x > 0)
            {
                newX *= -1;
            }
            _moveDirection = normal * newZ + tangent * newX;
        }
    }

    private void BallRotation()
    {
        float rotationSpeed = 2 * _speed / _localScaleX;
        _rotation.x += rotationSpeed * Time.deltaTime * Mathf.Rad2Deg;

        if (_moveDirection.z > 0)
        {
            _rotation.y = HALFPIDEG - Mathf.Acos(_moveDirection.x) * Mathf.Rad2Deg;
        }
        else
        {
            _rotation.y = HALFPIDEG + Mathf.Acos(_moveDirection.x) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.Euler(_rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _moveDirection * _minSpeed);
    }
}
