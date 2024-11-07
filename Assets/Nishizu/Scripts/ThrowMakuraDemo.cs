using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMakuraDemo : MonoBehaviour
{
    [SerializeField] private GameObject _makuraPrefab;

    private float _pickUpDistance = 3.0f;
    private GameObject _currentMakura;

    private Vector3 _targetPosition;

    private MakuraController _makuraController;


    public enum ThrowType
    {
        Nomal,
        Charge
    }

    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = new Vector3(transform.position.x + -4, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (_currentMakura == null)
        {
            PickUpMakura();
        }
        if (_currentMakura != null)
        {
            StartCoroutine(ThrowMakuraNomal());
        }
    }
    private void PickUpMakura()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _pickUpDistance);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Makura") && !collider.GetComponent<MakuraController>().IsThrow)
            {
                _currentMakura = collider.gameObject;

                _currentMakura.SetActive(false);
                break;
            }
        }
    }
    private void ThrowMakura(ThrowType throwType)
    {
        if (_currentMakura != null)
        {
            Rigidbody rb = _currentMakura.GetComponent<Rigidbody>();
            _makuraController = _currentMakura.GetComponent<MakuraController>();
            if (rb.velocity != Vector3.zero)
            {
                rb.velocity = Vector3.zero;
            }
            rb.isKinematic = false;

            Vector3 throwDirection;
            if (_targetPosition != Vector3.zero)
            {
                Vector3 targetDirection = _targetPosition - transform.position;
                targetDirection.y = 0;
                throwDirection = targetDirection.normalized;
                transform.rotation = Quaternion.LookRotation(throwDirection);
            }
            else
            {
                throwDirection = transform.forward;
            }
            float forwardForce = 0.0f;
            float upwardForce = 0.0f;
            float throwDistance = 0.0f;
            float throwHeight = 0.0f;
            if (_makuraController.CurrentColorType == ColorChanger.ColorType.Nomal)
            {
                switch (throwType)
                {
                    case ThrowType.Nomal:
                        forwardForce = 900.0f;
                        upwardForce = 200.0f;
                        throwDistance = 1.3f;
                        throwHeight = 1.0f;
                        Debug.Log("通常");
                        break;
                    case ThrowType.Charge:
                        forwardForce = 300.0f;
                        upwardForce = 700.0f;
                        throwDistance = 0.5f;
                        throwHeight = 2.0f;
                        Debug.Log("くらえ！爆発まくら");
                        break;
                }
            }
            else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Red)
            {
                rb.useGravity = false;
                forwardForce = 500.0f;
                throwDistance = 1.3f;
                throwHeight = 1.0f;
            }
            else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Blue)
            {
                rb.useGravity = false;
                forwardForce = 300.0f;
                throwDistance = 1.3f;
                throwHeight = 1.0f;

            }
            else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Green)
            {

            }
            else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Black)
            {
                Collider col = _currentMakura.GetComponent<Collider>();
                col.isTrigger = true;
                rb.useGravity = false;
                forwardForce = 500.0f;
                throwDistance = 1.3f;
                throwHeight = 1.0f;
            }

            Vector3 throwPosition = transform.position + throwDirection * throwDistance + Vector3.up * throwHeight;

            _currentMakura.transform.position = throwPosition;
            _currentMakura.SetActive(true);
            _makuraController.IsThrow = true;
            _makuraController.Thrower = gameObject;

            rb.AddForce(throwDirection * forwardForce * 0.5f + Vector3.up * upwardForce);
            rb.maxAngularVelocity = 100;
            rb.AddTorque(Vector3.up * 120.0f);

            _currentMakura = null;
        }
    }

    private IEnumerator ThrowMakuraNomal()
    {
        yield return new WaitForSeconds(5.0f);
        ThrowMakura(ThrowType.Nomal);
    }
}
