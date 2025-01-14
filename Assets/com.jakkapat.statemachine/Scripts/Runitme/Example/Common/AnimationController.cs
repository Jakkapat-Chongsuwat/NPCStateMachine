namespace Jakkapat.ToppuFSM.Example
{
    using UnityEngine;

    public class AnimationController : MonoBehaviour
    {
        Animator _animator;
        int _animIDSpeed;
        int _animIDGrounded;
        int _animIDJump;
        int _animIDFreeFall;
        int _animIDMotionSpeed;
        int _animIDSurprise;
        int _animIDGreeting;
        int _animIDStopSurprise;

        [SerializeField] private AudioClip[] footstepClips;
        [SerializeField] private AudioClip landingClip;
        [SerializeField] private CharacterController characterController;
        [Range(0, 1)][SerializeField] private float footstepVolume = 0.5f;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDSurprise = Animator.StringToHash("Surprise");
            _animIDStopSurprise = Animator.StringToHash("StopSurprise");
            _animIDGreeting = Animator.StringToHash("Greeting");
        }

        public void SetSpeed(float speed)
        {
            if (_animator) _animator.SetFloat(_animIDSpeed, speed);
        }

        public void SetGrounded(bool grounded)
        {
            if (_animator) _animator.SetBool(_animIDGrounded, grounded);
        }

        public void SetJump(bool jump)
        {
            if (_animator) _animator.SetBool(_animIDJump, jump);
        }

        public void SetFreeFall(bool freeFall)
        {
            if (_animator) _animator.SetBool(_animIDFreeFall, freeFall);
        }

        public void SetMotionSpeed(float motionSpeed)
        {
            if (_animator) _animator.SetFloat(_animIDMotionSpeed, motionSpeed);
        }

        public void SetSurprise()
        {
            if (_animator) SetTrigger(_animIDSurprise);
        }

        public void ResetSurprise()
        {
            if (_animator) ResetTrigger(_animIDSurprise);
        }

        public void SetStopSurprise()
        {
            if (_animator) SetTrigger(_animIDStopSurprise);
        }

        public void SetGreeting()
        {
            if (_animator) SetTrigger(_animIDGreeting);
        }

        public void ResetGreeting()
        {
            if (_animator) ResetTrigger(_animIDGreeting);
        }

        public void SetTrigger(int triggerId)
        {
            if (_animator)
            {
                _animator.ResetTrigger(triggerId);
                _animator.SetTrigger(triggerId);
            }
        }

        public void ResetTrigger(int triggerId)
        {
            if (_animator) _animator.ResetTrigger(triggerId);
        }

        public void OnFootstep(AnimationEvent animEvent)
        {
            if (characterController == null || footstepClips == null || footstepClips.Length == 0)
                return;

            if (animEvent.animatorClipInfo.weight > 0.5f)
            {
                int index = Random.Range(0, footstepClips.Length);
                AudioClip clipToPlay = footstepClips[index];
                Vector3 position = characterController.transform.TransformPoint(characterController.center);

                AudioSource.PlayClipAtPoint(clipToPlay, position, footstepVolume);
            }
        }

        public void OnLand(AnimationEvent animEvent)
        {
            if (characterController == null || landingClip == null)
                return;

            if (animEvent.animatorClipInfo.weight > 0.5f)
            {
                Vector3 position = characterController.transform.TransformPoint(characterController.center);
                AudioSource.PlayClipAtPoint(landingClip, position, footstepVolume);
            }
        }
    }
}
