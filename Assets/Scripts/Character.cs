using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] private float _impactForce = 5f;

    [Header("After the kill")]
    [SerializeField] private float _increaseSize = .2f;
    [SerializeField] private float _increaseSizeSpeed = 2f;
    [SerializeField] private float _decreaseImpactForce = .5f;
    [SerializeField] private float _multiplicationImpactForce = 1.3f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _hit;
    [SerializeField] private ParticleSystem _buttonHit;
    [SerializeField] private ParticleSystem _waterSplash;

    protected Rigidbody Rigidbody;

    private readonly int _buttonPressedHash = Animator.StringToHash("ButtonPressed");

    private bool _isColliding;
    private GameObject _lastGameObjectCollision;
    private Vector3 _newScale;
    private Coroutine _smoothlyIncreaseSizeCoroutine;
    private bool _waterEntered;

    public event UnityAction<Character, GameObject> Died;

    public string Nickname { get; protected set; }
    public bool IsDied { get; protected set; }

    public void IncreaseSize()
    {
        _newScale = transform.localScale + new Vector3(_increaseSize, _increaseSize, _increaseSize);

        if (_smoothlyIncreaseSizeCoroutine != null)
        {
            StopCoroutine(SmoothlyIncreaseSize());
        }

        _smoothlyIncreaseSizeCoroutine = StartCoroutine(SmoothlyIncreaseSize());
    }

    public void DecreaseImpactForce()
    {
        _impactForce -= _decreaseImpactForce;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if (_isColliding || collisionGameObject.TryGetComponent(out Platform _))
        {
            return;
        }

        _isColliding = true;
        _lastGameObjectCollision = collisionGameObject;
        Collider contactCollider = collision.GetContact(0).thisCollider;

        float currentImpactForce;

        if (contactCollider.TryGetComponent(out CharacterButton characterButton))
        {
            var buttonAnimator = characterButton.GetComponent<Animator>();
            buttonAnimator.SetTrigger(_buttonPressedHash);

            _buttonHit.Play();
            currentImpactForce = _impactForce * _multiplicationImpactForce;
        }
        else
        {
            Instantiate(_hit, collision.GetContact(0).point, Quaternion.identity);
            currentImpactForce = _impactForce;
        }

        Vector3 direction = transform.position - collision.transform.position;
        Vector3 force = direction.normalized * currentImpactForce;

        Rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private void OnCollisionExit()
    {
        _isColliding = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Water _) && _waterEntered == false)
        {
            _waterEntered = true;
            Instantiate(_waterSplash, transform.position, _waterSplash.transform.rotation);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Water _))
        {
            IsDied = true;
            Died?.Invoke(this, _lastGameObjectCollision);
        }
    }

    private IEnumerator SmoothlyIncreaseSize()
    {
        var waitForSeconds = new WaitForEndOfFrame();

        while (Vector3.Distance(transform.localScale, _newScale) < 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _newScale, _increaseSizeSpeed * Time.deltaTime);
            yield return waitForSeconds;
        }
    }
}