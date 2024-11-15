using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private LayerMask _hutonLayer;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private GameObject _showMakura;
        [SerializeField] private GameObject _alterEgoMakura;

        private GameObject _currentMakuraDisplay;
        private Rigidbody _rb;
        private Animator _animator;
        private CapsuleCollider _col;
        private float _speed = 5.0f;//プレイヤーの移動速度
        private float _jumpForce = 40.0f;//ジャンプの強さ
        private float _groundCheckRadius = 0.01f;//足元が地面か判定する球の半径
        private float _pickUpDistance = 1.0f;//まくらを拾うことができる距離
        private float _playerSerchDistance = 3.0f;//敵プレイヤーの捜索範囲
        private GameObject _currentMakura;//手持ちのまくら
        private GameObject _thrownMakura;//投げられたまくら
        private bool _isSleep = false;//寝ているか
        private bool _isHitStop = false;//止まっているか
        private bool _isJumping = false;//ジャンプ中か
        private bool _isChargeTime = false;//ため攻撃中か
        private bool _isCanCatch = false;//ジャストキャッチ可能か
        private bool _isInvincibilityTime = false;//無敵中か
        private bool isKeyboardOperation = false;//キーボード操作かどうか
        private Vector3 _beforeSleepPosition;//布団で寝る前の位置
        private Vector3 _targetPosition;//敵プレイヤーの位置
        private Quaternion _beforeSleepRotation;//布団で寝る前の向き
        private Quaternion _lastDirection;//移動入力の最後に向いている向き
        private HutonController _currentHuton;//布団のスクリプト
        private MakuraController _makuraController;//まくらのスクリプト
        private PlayerStatus _playerStatus;//プレイヤーのスクリプト
        private float _keyHoldTime;//長押ししている時間
        private float _keyLongPressTime = 0.5f;//ため攻撃にかかる時間

        private float _rotationSpeed = 200.0f;//持っているまくらの回転速度
        private float _showRadius = 0.6f;//プレイヤーからのまくらの距離
        private float _rotationAngle;
        ShowMakuraController _showMakuraController;
        private float _vibrationStrength = 0.3f;//振動の強さ
        private float _vibrationTime = 0.3f;//振動する時間
        private bool _isVibrating = false;
        private bool _isHitCoolTime = false;
        private bool _isCounterAttackTime = false;

        public enum ThrowType
        {
            Nomal,
            Charge
        }
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _col = GetComponent<CapsuleCollider>();
            _playerStatus = GetComponent<PlayerStatus>();
            if (_showMakura != null)
            {
                _currentMakuraDisplay = Instantiate(_showMakura);
            }
            _showMakuraController = _currentMakuraDisplay.GetComponent<ShowMakuraController>();
        }

        void Update()
        {
            JumpForce(IsJump());
            IsCheckPlayer();
            MakuraDisplayColorChange();

            // if (_currentMakura != null && !_isSleep && _playerStatus.ChargeMax && Input.GetButtonDown("Jump"))
            // {
            //     _makuraController = _currentMakura.GetComponent<MakuraController>();
            //     _makuraController.CurrentType = MakuraController.ScaleType.Second;
            //     _showMakuraController.CurrentType = MakuraController.ScaleType.First;
            //     _playerStatus.CurrentSP = 0;
            // }
            if (_currentMakura != null && !_isSleep && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.B)))//デバッグ用のif文。本来は一つ上のif文
            {
                _playerStatus.CurrentSP = 0;
            }
            if (_currentMakura != null && !_isSleep)
            {
                RotateShowMakura();

                _currentMakuraDisplay.SetActive(true);
            }
            else
            {
                _currentMakuraDisplay.SetActive(false);
            }
            if (OnHuton() || _isChargeTime)
            {
                _speed = 2.0f;
            }
            else
            {
                _speed = 4.0f;
            }
            if (_currentMakura == null && _thrownMakura != null && _isCanCatch && Input.GetAxis("L_R_Trigger") < -0.5f)
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
                    IsJump();
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
                isKeyboardOperation = true;
                _keyHoldTime = Time.time;
                _isChargeTime = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isKeyboardOperation = false;
                _isChargeTime = false;
                float holdTime = Time.time - _keyHoldTime;
                if (holdTime < _keyLongPressTime)
                {
                    ThrowMakura(ThrowType.Nomal);
                }
                else
                {
                    ThrowMakura(ThrowType.Charge);
                }
            }
            if (!isKeyboardOperation)
            {
                if (Input.GetAxis("L_R_Trigger") > 0.5f)
                {
                    if (!_isChargeTime)
                    {
                        _keyHoldTime = Time.time;
                        _isChargeTime = true;
                    }
                }
                else if (_isChargeTime)
                {
                    _isChargeTime = false;
                    float holdTime = Time.time - _keyHoldTime;
                    if (holdTime < _keyLongPressTime)
                    {
                        ThrowMakura(ThrowType.Nomal);
                    }
                    else
                    {
                        ThrowMakura(ThrowType.Charge);
                    }
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
            return Physics.CheckSphere(groundCheckPosition, _groundCheckRadius, _groundLayers, QueryTriggerInteraction.Collide);
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
        private bool IsJump()
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButton("Fire2")) && OnGround() && !_isJumping && !_isHitStop)
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
        private void RotateShowMakura()
        {
            if (_currentMakuraDisplay != null)
            {

                // if (_showMakuraController.CurrentType == ShowMakuraController.ScaleType.First || _showMakuraController.CurrentType == ShowMakuraController.ScaleType.Second)
                // {
                //     _showRadius = 1.0f;
                // }
                // else
                // {
                //     _showRadius = 0.6f;
                // }
                _rotationAngle += _rotationSpeed * Time.deltaTime;

                Vector3 offset = new Vector3(Mathf.Cos(_rotationAngle * Mathf.Deg2Rad) * _showRadius, 1.0f, Mathf.Sin(_rotationAngle * Mathf.Deg2Rad) * _showRadius);
                _currentMakuraDisplay.transform.position = transform.position + offset;

                _currentMakuraDisplay.transform.LookAt(transform.position);
            }
        }
        private void MakuraDisplayColorChange()
        {
            if (_currentMakura != null)
            {
                _makuraController = _currentMakura.GetComponent<MakuraController>();
                if (_makuraController.CurrentColorType == ColorChanger.ColorType.Nomal)
                {
                    _showMakuraController.CurrentColorType = ColorChanger.ColorType.Nomal;
                }
                else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Red)
                {
                    _showMakuraController.CurrentColorType = ColorChanger.ColorType.Red;
                }
                else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Blue)
                {
                    _showMakuraController.CurrentColorType = ColorChanger.ColorType.Blue;
                }
                else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Green)
                {
                    _showMakuraController.CurrentColorType = ColorChanger.ColorType.Green;
                }
                else if (_makuraController.CurrentColorType == ColorChanger.ColorType.Black)
                {
                    _showMakuraController.CurrentColorType = ColorChanger.ColorType.Black;
                }
            }
        }
        private void SpecialAttack()
        {
            //Q
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

        private bool IsWallThrowCheck()
        {
            Vector3 throwDirection = transform.forward;
            _makuraController = _currentMakura.GetComponent<MakuraController>();

            float throwDistance = _makuraController.CurrentScaleType == MakuraController.ScaleType.Second ? 5.0f : 1.5f;

            return Physics.Raycast(transform.position, throwDirection, throwDistance, _wallLayer);
        }
        /// <summary>
        /// まくらをジャストキャッチする
        /// </summary>
        private void CatchMakura()
        {
            if (_thrownMakura != null)
            {
                _currentMakura = _thrownMakura;
                _thrownMakura.SetActive(false);
                _thrownMakura = null;
                _isCanCatch = false;
                _isInvincibilityTime = true;
                _playerStatus.CurrentSP += 5000;
                StartCoroutine(CounterAttackCoroutine());
                StartCoroutine(JustChachMakuraInvincibilityTime());
                // Debug.Log("枕をキャッチした！");
            }
        }
        /// <summary>
        /// ジャストキャッチ後0.3秒無敵
        /// </summary>
        /// <returns>0.3秒後解除</returns>
        private IEnumerator JustChachMakuraInvincibilityTime()
        {
            yield return new WaitForSeconds(0.3f);
            _isInvincibilityTime = false;
        }
        private void OnTriggerEnter(Collider collider)
        {
            MakuraController makuraController = collider.GetComponent<MakuraController>();
            if (collider.CompareTag("Makura") && makuraController.IsThrow && makuraController.Thrower != gameObject && makuraController.CurrentScaleType == MakuraController.ScaleType.Nomal && !makuraController.IsAlterEgo)
            {
                _isCanCatch = true;
                _thrownMakura = collider.gameObject;
                // Debug.Log("情報を記憶");
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("Makura"))
            {
                _isCanCatch = false;
                _thrownMakura = null;
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
        /// <summary>
        /// 布団から起きる
        /// </summary>
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
        private bool IsCheckPlayer()
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
        private Vector3 TargetPosition(GameObject targetPlayer)
        {
            Vector3 playerPosition = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(playerPosition, 10.0f);

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Player") && collider.gameObject == targetPlayer)
                {
                    return collider.gameObject.transform.position;
                }
            }
            return Vector3.zero;
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
            }
            MakuraController makuraController = collision.gameObject.GetComponent<MakuraController>();

            if (collision.gameObject.CompareTag("Makura") && makuraController.Thrower != gameObject && makuraController.IsThrow && !_isInvincibilityTime && !_isHitCoolTime)
            {
                _isCanCatch = false;
                _isHitCoolTime = true;
                _animator.SetBool("Walk", false);
                Debug.Log("う、動けない！");
                HitMotion();
                if (!_isVibrating)
                {
                    StartCoroutine(HitStopVibration(makuraController.IsCounterAttack));
                }
            }
            if (collision.gameObject.CompareTag("Meteor"))
            {
                _isCanCatch = false;
                _isHitCoolTime = true;
                _animator.SetBool("Walk", false);
                Debug.Log("う、動けない！");
                HitMotion();
                if (!_isVibrating)
                {
                    StartCoroutine(HitStopVibration(false));
                }
            }
        }
        private IEnumerator HitStopVibration(bool isCounterAttack)
        {
            _isVibrating = true;
            Vector3 hitPosition = transform.position;

            float elapsedTime = 0.0f;

            while (elapsedTime < _vibrationTime)
            {
                float strength = Mathf.Lerp(_vibrationStrength * (isCounterAttack ? 2.0f : 1.0f), 0, elapsedTime / _vibrationTime);

                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-strength, strength),
                    OnGround() ? 0 : UnityEngine.Random.Range(-strength, strength),
                    UnityEngine.Random.Range(-strength, strength)
                );

                transform.position = Vector3.Lerp(transform.position, hitPosition + randomOffset, Time.deltaTime * 100);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = hitPosition;

            _isVibrating = false;
            _isHitCoolTime = false;
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
        /// 枕を当てられると1秒制止する
        /// </summary>
        /// <returns>2秒後に解除</returns>
        private IEnumerator HitStopCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            _isHitStop = false;
        }

        private IEnumerator CounterAttackCoroutine()
        {
            _isCounterAttackTime = true;
            yield return new WaitForSeconds(1.0f);
            _isCounterAttackTime = false;
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

}
