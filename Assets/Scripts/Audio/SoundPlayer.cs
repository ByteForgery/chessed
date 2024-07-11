using Chessed.Logic;
using UnityEngine;

namespace Chessed
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip moveClip, captureClip, promoteClip, castleClip, checkClip, gameEndClip, tenSecondsClip;

        [SerializeField] private AudioSource moveSrc, extraSrc, statusSrc;

        public void OnMove(MoveResult result, MoveType type)
        {
            AudioClip selectedMoveClip = type switch
            {
                MoveType.Promotion => promoteClip,
                MoveType.CastleKS or MoveType.CastleQS => castleClip,
                _ => result.IsCapture ? captureClip : moveClip
            };
            moveSrc.clip = selectedMoveClip;

            AudioClip extraClip = null;
            if (result.IsCheck) extraClip = checkClip;
            extraSrc.clip = extraClip;

            moveSrc.Play();

            if (extraClip != null)
                extraSrc.Play();
        }

        public void OnTenSecondsRemaining()
        {
            statusSrc.clip = tenSecondsClip;
            statusSrc.Play();
        }

        public void OnGameEnd()
        {
            statusSrc.clip = gameEndClip;
            statusSrc.Play();
        }
    }
}
