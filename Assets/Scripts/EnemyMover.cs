using System.Linq;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private int _rotateSpeed = 6;
    [SerializeField] private float _secondsBeetwenChangeTarget = 5;

    private Character _target;

    private void Start()
    {
        InvokeRepeating(nameof(ChangeTarget), 0, _secondsBeetwenChangeTarget);
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = GetRandomTarget();
        }

        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

        Vector3 lookDirection = _target.transform.position - transform.position;
        lookDirection.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotateSpeed * Time.deltaTime);
    }

    private void ChangeTarget()
    {
        _target = GetRandomTarget();
    }

    private Character GetRandomTarget()
    {
        Character[] characters = FindObjectsOfType<Character>()
            .Where(character => character.transform.position != transform.position).ToArray();

        return characters[Random.Range(0, characters.Length)];
    }
}