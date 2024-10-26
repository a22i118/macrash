using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _hutonLayer;
    private Rigidbody _rb;
    private Animator _animator;
    private CapsuleCollider _col;
    private float _speed = 5.0f;
    private float _jumpForce = 40.0f;
    private float _groundCheckRadius = 0.15f;
    private float _pickUpDistance = 1.0f;
    private float _playerSerchDistance = 5.0f;
    private GameObject _currentMakura;
    private GameObject _incomingMakura;
    private bool _isSleep = false;
    private bool _isHitStop = false;
    private bool _isJumping = false;
    private bool _chargeTime = false;
    private bool _canCatch = false;
    private bool _invincibilityTime = false;
    private Vector3 _beforeSleepPosition;
    private Vector3 _targetPosition;
    private Quaternion _beforeSleepRotation;
    private Quaternion _lastDirection;
    private HutonController _currentHuton;
    private MakuraController _makuraController;
    private PlayerStatus _playerStatus;
    private float _keyHoldTime;
    private float _keyLongPressTime = 0.5f;

    public enum ThrowType
    {
        Nomal,
        Explosion
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _col = GetComponent<CapsuleCollider>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        JumpForce(Jump());
        CheckPlayer();
        if (OnHuton() || _chargeTime)
        {
            _speed = 3.0f;
        }
        else
        {
            _speed = 4.0f;
        }
        if (_currentMakura == null && _incomingMakura != null && _canCatch && Input.GetAxis("L_R_Trigger") < -0.5f)
        {
            CatchMakura();
        }
        if (_isSleep)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire3"))
            {
                WakeUp();
            }
        }
        else
        {
            if (!_isHitStop)
            {
                Walk();
                Jump();
                if (_currentMakura != null && OnHuton() && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire3")))
                {
                    Debug.Log("寝るぜ！");
                    Sleep();
                }
                if (_currentMakura == null && CheckMakura() && (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("L_R_Trigger") < -0.5f))
                {
                    PickUpMakura();
                }
                else if (_currentMakura != null)
                {
                    ThrowDecide();
                }
            }
        }
    }
    /// <summary>
    /// 入力で投げ方を変える
    /// </summary>
    private void ThrowDecide()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _keyHoldTime = Time.time;
            _chargeTime = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _chargeTime = false;
            float holdTime = Time.time - _keyHoldTime;
            if (holdTime < _keyLongPressTime)
            {
                ThrowMakura(ThrowType.Nomal);
            }
            else
            {
                ThrowMakura(ThrowType.Explosion);
            }
        }
        if (Input.GetAxis("L_R_Trigger") > 0.5f)
        {
            if (!_chargeTime) // 新しく押し始めたとき
            {
                _keyHoldTime = Time.time;
                _chargeTime = true;
            }
        }
        else if (_chargeTime) // トリガーが離されたら処理
        {
            _chargeTime = false;
            float holdTime = Time.time - _keyHoldTime;
            if (holdTime < _keyLongPressTime)
            {
                ThrowMakura(ThrowType.Nomal);
            }
            else
            {
                ThrowMakura(ThrowType.Explosion);
            }
        }

    }
    // private void FixedUpdate()
    // {
    //     JumpForce(Jump());
    // }

    /// <summary>
    /// 歩く
    /// </summary>
    private void Walk()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical);

        _rb.velocity = new Vector3(movement.x * _speed, _rb.velocity.y, movement.z * _speed);

        if (movement.magnitude > 0.01f)
        {
            _animator.SetBool("Walk", true);
            transform.rotation = Quaternion.LookRotation(movement);
            _lastDirection = transform.rotation;
        }
        else
        {
            _animator.SetBool("Walk", false);
            transform.rotation = _lastDirection;
        }
    }

    /// <summary>
    /// 足元がGroundかどうか
    /// </summary>
    /// <returns>足元がGroundならtrueを返す</returns>
    private bool OnGround()
    {
        Vector3 groundCheckPosition = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        return Physics.CheckSphere(groundCheckPosition, _groundCheckRadius, _groundLayers);
    }
    /// <summary>
    /// 足元がHutonかどうか
    /// </summary>
    /// <returns>足元がHutonならtrueを返す</returns>
    private bool OnHuton()
    {
        Vector3 groundCheckPosition = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        return Physics.CheckSphere(groundCheckPosition, _groundCheckRadius, _hutonLayer);
    }
    /// <summary>
    /// ジャンプの入力を返す
    /// </summary>
    private bool Jump()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButton("Fire2")) && OnGround() && !_isJumping)
        {
            _isJumping = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// プレイヤーに上向きの力を加える
    /// </summary>
    /// <param name="jump">ジャンプ入力</param>
    private void JumpForce(bool jump)
    {
        if (jump)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
        if (!OnGround())
        {
            _animator.SetBool("Jump", true);
        }
        else
        {
            _animator.SetBool("Jump", false);
            _isJumping = false;
        }
    }

    /// <summary>
    /// 近くに枕があることを返す
    /// </summary>
    /// <returns>近くに枕があればtrueを返す</returns>
    private bool CheckMakura()
    {
        Vector3 playerPosition = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, _pickUpDistance);
        foreach (var makura in hitColliders)
        {
            if (makura.CompareTag("Makura"))
            {
                // Debug.Log("近くに枕があるぜ！");
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 近くの枕を拾う
    /// </summary>
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
    /// <summary>
    /// throwTypeに応じて投げ方を変える
    /// </summary>
    /// <param name="throwType">投げ方の種類</param>
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
                targetDirection.y -= 1.0f;
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
            switch (throwType)
            {
                case ThrowType.Nomal:
                    forwardForce = 900.0f;
                    upwardForce = 200.0f;
                    throwDistance = 1.3f;
                    throwHeight = 1.0f;
                    Debug.Log("通常");
                    break;
                case ThrowType.Explosion:
                    forwardForce = 200.0f;
                    upwardForce = 700.0f;
                    throwDistance = 0.5f;
                    throwHeight = 2.0f;
                    Debug.Log("くらえ！爆発まくら");
                    break;
            }
            Vector3 throwPosition = transform.position + throwDirection * throwDistance + Vector3.up * throwHeight;

            _currentMakura.transform.position = throwPosition;
            _currentMakura.SetActive(true);
            _makuraController.IsThrow = true;
            _makuraController.Thrower = gameObject;

            rb.AddForce(throwDirection * forwardForce + Vector3.up * upwardForce);
            rb.AddTorque(Vector3.up * 10000.0f);

            _currentMakura = null;
        }
    }
    private void CatchMakura()
    {
        if (_incomingMakura != null)
        {
            _currentMakura = _incomingMakura;
            _incomingMakura.SetActive(false);
            _incomingMakura = null;
            _canCatch = false;
            _invincibilityTime = true;
            StartCoroutine(JustChachMakuraInvincibilityTime());
            Debug.Log("枕をキャッチした！");
        }
    }
    private IEnumerator JustChachMakuraInvincibilityTime()
    {
        yield return new WaitForSeconds(0.3f);
        _invincibilityTime = false;
    }
    private void OnTriggerEnter(Collider collider)
    {
        MakuraController makuraController = collider.GetComponent<MakuraController>();
        if (collider.CompareTag("Makura") && makuraController.IsThrow && makuraController.Thrower != gameObject)
        {
            _canCatch = true;
            _incomingMakura = collider.gameObject;
            Debug.Log("情報を記憶");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Makura"))
        {
            _canCatch = false;
            _incomingMakura = null;
        }
    }


    /// <summary>
    /// 布団の上で寝る
    /// </summary>
    private void Sleep()
    {
        _animator.SetBool("Walk", false);
        _rb.isKinematic = true;
        _isSleep = true;
        _beforeSleepPosition = transform.position;
        _beforeSleepRotation = transform.rotation;

        Vector3 hutonPosition = _currentHuton.GetCenterPosition();
        transform.position = new Vector3(hutonPosition.x, hutonPosition.y + 0.0f, hutonPosition.z - 0.75f);

        if (_currentHuton != null)
        {
            _currentHuton.Makura.SetActive(true);
        }

        transform.rotation = _currentHuton.GetRotation();

        transform.rotation = Quaternion.Euler(-81.0f, transform.rotation.eulerAngles.y, 0.0f);
    }
    private void WakeUp()
    {
        _rb.isKinematic = true;
        _col.enabled = false;

        if (_currentHuton != null)
        {
            _currentHuton.Makura.SetActive(false);
        }
        else
        {
            Debug.LogWarning("現在の布団がnullだってよまじかよ");
        }

        transform.position = _beforeSleepPosition + Vector3.up * 0.04f;

        StartCoroutine(PhysicsAndColliderDelay());
    }
    /// <summary>
    /// プレイヤーの物理とコライダーの無効化
    /// </summary>
    /// <returns>0.1秒後に有効化</returns>
    private IEnumerator PhysicsAndColliderDelay()
    {
        // transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        transform.rotation = _beforeSleepRotation;
        yield return new WaitForSeconds(0.1f);

        _rb.isKinematic = false;
        _col.enabled = true;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _isSleep = false;
    }
    /// <summary>
    /// 近くにプレイヤーがいるか
    /// </summary>
    /// <returns>近くにプレイヤーがいたらtrueを返す</returns>
    private bool CheckPlayer()
    {
        Vector3 playerPosition = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, _playerSerchDistance);
        bool foundOpponent = false;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject != gameObject)
            {
                _targetPosition = collider.gameObject.transform.position;
                // Debug.Log("Playerを発見したぜ");
                foundOpponent = true;
                return true;
            }
        }

        if (!foundOpponent && _targetPosition != Vector3.zero)
        {
            _targetPosition = Vector3.zero;
        }

        return false;
    }
    /// <summary>
    /// 接触したオブジェクトの情報をもとに操作
    /// </summary>
    /// <param name="collision">接触したオブジェクトの情報</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Huton"))
        {
            Debug.Log("布団に入ったぜ");
            _currentHuton = collision.gameObject.GetComponent<HutonController>();
            Debug.Log("現在の布団はこれだ：" + _currentHuton);
        }
        MakuraController makuraController = collision.gameObject.GetComponent<MakuraController>();
        if (collision.gameObject.CompareTag("Makura") && makuraController.Thrower != gameObject)
        {
            if (makuraController.IsThrow && !_invincibilityTime)
            {
                _canCatch = false;
                _animator.SetBool("Walk", false);
                Debug.Log("う、動けない！");
                HitMotion();
            }
        }
    }
    /// <summary>
    /// 枕が当たったときのモーション
    /// </summary>
    private void HitMotion()
    {
        _rb.velocity = Vector3.zero;
        _isHitStop = true;
        _playerStatus.SpUp();

        StartCoroutine(HitStopCoroutine());
        Debug.Log("動けるぜ");
    }
    /// <summary>
    /// 枕を当てられると2秒制止する
    /// </summary>
    /// <returns>2秒後に解除</returns>
    private IEnumerator HitStopCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        _isHitStop = false;
    }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Huton"))
    //     {
    //         Debug.Log("布団から出たぜ");

    //         _currentHuton = null;
    //     }
    // }
}
