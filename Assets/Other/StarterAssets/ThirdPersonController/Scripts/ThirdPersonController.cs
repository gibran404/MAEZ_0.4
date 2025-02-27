﻿using System;
using System.Collections.Concurrent;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;
        public bool SprintFlag = false;
        public static bool Blocked = false;
        public static bool Attacking = false;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;


        [Header("Other")]
        public GameObject PlayerManager;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // animation IDs
        private int _animIDWalking;
        private int _animIDRunning;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            GroundedCheck();

            if (UIVariables.isUiEnabled)
            {
                MoveUI();
                OtherChecksUI();
            }
            else
            {
                Move();
                OtherChecks();
            }
        }


        private void Move()
        {
            float Gravity = -30f;
            float targetSpeed ; //= _input.sprint ? SprintSpeed : MoveSpeed;

            if (_input.sprint && !Blocked && !Attacking)
            {
                if (PlayerItemsandVitals.stamina > 1 && SprintFlag)
                {
                    targetSpeed = SprintSpeed;
                    PlayerManager.GetComponent<PlayerItemsandVitals>().reduceStamina();
                }
                else
                {
                    //when running and stam not availible, player can't run
                    SprintFlag = false;
                    targetSpeed = MoveSpeed;
                    GetComponent<StarterAssetsInputs>().sprint = false; //controls animation
                    PlayerManager.GetComponent<PlayerItemsandVitals>().regenStamina();
                }
            }
            else if (!Blocked && !Attacking)
            {
                if (PlayerItemsandVitals.stamina > 5)
                {
                    //when not running, and stam availible, player can run again
                    // player must stop running to run again after regen
                    SprintFlag = true;
                }
                targetSpeed = MoveSpeed;
                PlayerManager.GetComponent<PlayerItemsandVitals>().regenStamina();
            }
            else
            {
                targetSpeed = 0.5f;
            }

            PlayFootstepAudio();
            

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // apply gravity
            if (_controller.isGrounded)
            {
                _verticalVelocity = -0.5f;
            }
            else
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }

            // move the player
            _controller.Move((targetDirection.normalized * (_speed * Time.deltaTime) +
                            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDWalking, targetSpeed > 0.1f && !_input.sprint);
                _animator.SetBool(_animIDRunning, _input.sprint && targetSpeed > 0.1f);
            }
        }
        private void OtherChecks()
        {
            //Attack

            // if left mouse button held down
            if (Mouse.current.leftButton.isPressed)
            {
                // face 90 degrees to the right of the direction cinemachine camera is facing
                float targetAngle = CinemachineCameraTarget.transform.rotation.eulerAngles.y + 90.0f;
                float smoothedAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetAngle, Time.deltaTime * 4);
                transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);

                Attacking = true;
                _animator.SetBool("Attacking", true);
            }
            else if (!Mouse.current.leftButton.isPressed && _animator.GetBool("Attacking") == true)
            {
                Attacking = false;
                _animator.SetBool("Attacking", false);
            }



            //Block
            if (Mouse.current.rightButton.isPressed && !Attacking)
            {
                // float targetAngle = CinemachineCameraTarget.transform.rotation.eulerAngles.y + 90.0f;
                // float smoothedAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetAngle, Time.deltaTime * 4);
                // transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);

                Blocked = true;
                _animator.SetBool("Blocked", true);
            }
            else if (!Mouse.current.rightButton.isPressed && _animator.GetBool("Blocked") == true)
            {
                Blocked = false;
                _animator.SetBool("Blocked", false);
            }


            //Interacting
            if (Input.GetKeyDown(KeyCode.E) && !Attacking && !Blocked)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
                bool doorNearby = false;
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Door") || collider.CompareTag("Chest"))
                    {
                        doorNearby = true;
                        break;
                    }
                }
                if (doorNearby)
                {
                    _animator.SetBool("Interacting", true);
                }
            }
            else
            {
                _animator.SetBool("Interacting", false);
            }


            
        }

        public Joystick joystick;
        private void MoveUI()
        {
            float Gravity = -30f;
            float targetSpeed; // Define target speed based on whether sprinting is enabled
            

            float joystickMagnitude = Mathf.Sqrt(joystick.Vertical * joystick.Vertical + joystick.Horizontal * joystick.Horizontal);
            // Debug.Log(joystickMagnitude);

            if (joystickMagnitude > 0.9f && !Blocked && !Attacking)
            {
                if (PlayerItemsandVitals.stamina > 1 && SprintFlag)
                {
                    targetSpeed = SprintSpeed;
                    PlayerManager.GetComponent<PlayerItemsandVitals>().reduceStamina();
                }
                else
                {
                    SprintFlag = false;
                    targetSpeed = MoveSpeed;
                    PlayerManager.GetComponent<PlayerItemsandVitals>().regenStamina();
                }
            }
            else if (!Blocked && !Attacking)
            {
                if (PlayerItemsandVitals.stamina > 5)
                {
                    SprintFlag = true;
                }
                targetSpeed = MoveSpeed;
                PlayerManager.GetComponent<PlayerItemsandVitals>().regenStamina();
            }
            else
            {
                targetSpeed = 0.5f;
            }

            PlayFootstepAudio();

            if (joystick.Horizontal == 0 && joystick.Vertical == 0) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = 1f; // Using full magnitude for joystick movement

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical).normalized;

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                    _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Apply gravity
            if (_controller.isGrounded)
            {
                _verticalVelocity = -0.5f;
            }
            else
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }

            // Move the player
            _controller.Move((targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));

            // Update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDWalking, targetSpeed > 0.1f && !SprintFlag);
                _animator.SetBool(_animIDRunning, SprintFlag && targetSpeed > 0.1f);
            }
        }

        private void OtherChecksUI()
        {
            //Attack
            if (UIVariables.UIAttacking)
            {
                // face 90 degrees to the right of the direction cinemachine camera is facing
                float targetAngle = CinemachineCameraTarget.transform.rotation.eulerAngles.y + 90.0f;
                float smoothedAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetAngle, Time.deltaTime * 4);
                transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);

                Attacking = true;
                _animator.SetBool("Attacking", true);
            }
            else if (!UIVariables.UIAttacking && _animator.GetBool("Attacking") == true)
            {
                Attacking = false;
                _animator.SetBool("Attacking", false);
            }



            //Block
            if (UIVariables.UIBlocked && !Attacking)
            {
                // float targetAngle = CinemachineCameraTarget.transform.rotation.eulerAngles.y + 90.0f;
                // float smoothedAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetAngle, Time.deltaTime * 4);
                // transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);

                Blocked = true;
                _animator.SetBool("Blocked", true);
            }
            else if (!UIVariables.UIBlocked && _animator.GetBool("Blocked") == true)
            {
                Blocked = false;
                _animator.SetBool("Blocked", false);
            }


            //Interacting
            if (UIVariables.UIE && !Attacking && !Blocked)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
                bool doorNearby = false;
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Door") || collider.CompareTag("Chest"))
                    {
                        doorNearby = true;
                        break;
                    }
                }
                if (doorNearby)
                {
                    _animator.SetBool("Interacting", true);
                }
            }
            else
            {
                _animator.SetBool("Interacting", false);
            }


            
        }

        private void LateUpdate()
        {
            if (CinemachineCameraTarget != null)
            {
                CameraRotation();
            }
            else
            {
                Debug.LogWarning("CinemachineCameraTarget is not assigned.");
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDWalking = Animator.StringToHash("Walking");
            _animIDRunning = Animator.StringToHash("Running");

        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            if (_input != null && _input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private float footstepTimer = 0.0f;
        private float footstepInterval = 0.5f; // Adjust this based on desired footstep frequency

        private void PlayFootstepAudio()
        {

            // if (FootstepAudioClips.Length > 0 && _controller.isGrounded && _speed > 0.1f)
            if (FootstepAudioClips.Length > 0 && _speed > 0.1f) 
            {

                // Adjust footstep interval based on sprinting or walking
                footstepInterval = _input.sprint ? 0.3f : 0.5f; // Faster footsteps when sprinting

                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footstepInterval)
                {

                    // Randomly select a footstep sound from the array
                    int index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);

                    // Reset the footstep timer
                    footstepTimer = 0.0f;
                }
            }
            else
            {
                footstepTimer = 0.0f; // Reset if not moving
            }
        }



        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color gizmoColor = Grounded ? new Color(0, 1, 0, 0.35f) : new Color(1, 0, 0, 0.35f);
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}
