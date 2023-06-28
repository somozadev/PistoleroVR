using General;
using General.Sound;

namespace VR.Powerups
{
    public class MaxAmmo : PowerUp
    {
        protected override void PerformPowerupAction()
        {
            if (GameManager.Instance.players[0].leftHandItem != null)
            {
                if (GameManager.Instance.players[0].leftHandItem.GetComponent<BaseGun>())
                {
                    GameManager.Instance.players[0].leftHandItem.GetComponent<BaseGun>().FillUpAmmo();
                    GameManager.Instance.players[0].leftHandItem.GetComponent<BaseGun>().Animator.AnimationReload();
                    AudioManager.Instance.PlayOneShot("Reload");
                }
            }
            else if (GameManager.Instance.players[0].rightHandItem != null)
            {
                if (GameManager.Instance.players[0].rightHandItem.GetComponent<BaseGun>())
                {
                    GameManager.Instance.players[0].rightHandItem.GetComponent<BaseGun>().FillUpAmmo();
                    GameManager.Instance.players[0].rightHandItem.GetComponent<BaseGun>().Animator.AnimationReload();
                    AudioManager.Instance.PlayOneShot("Reload");
                }
            }
            Destroy(gameObject);
        }
    }
}