using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Vector3 _minScale = new Vector3(5, 5, 5);
    [SerializeField] private float _scaleSpeed = 2;

    private Coroutine _changeSizeCoroutine;
    private Vector3 _startlocalScale;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startlocalScale = transform.localScale;
    }

    public void StartChangeSize()
    {
        _changeSizeCoroutine = StartCoroutine(ChangeSize());
    }

    public void ResetPlatform()
    {
        if (_changeSizeCoroutine != null)
        {
            StopCoroutine(_changeSizeCoroutine);
        }

        transform.localScale = _startlocalScale;
        transform.SetPositionAndRotation(_startPosition, _startRotation);
    }

    private IEnumerator ChangeSize()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();

        while (transform.localScale != _minScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, _minScale, _scaleSpeed * Time.deltaTime);
            yield return waitForFixedUpdate;
        }
    }
}