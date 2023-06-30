using System.Linq;
using System.Net.NetworkInformation;
using General;
using General.Damageable;
using General.Sound;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public abstract class BaseGun : MonoBehaviour
    {
        [SerializeField] private GameObject selector;
        [Space(20)] private XRGrabInteractable _interactable;
        [SerializeField] protected float _bulletSpeed = 1000.0f;
        [SerializeField] private float _bulletDrop = 0.0f;
        [SerializeField] protected float _bulletDamage;

        [SerializeField] protected ParticleSystem _muzzleParticles;
        [SerializeField] private ParticleSystem _impactParticles;
        [SerializeField] protected Transform _raycastOrigin;
        [SerializeField] private ShopInstance _shopInstance;
        private Ray _ray;
        private RaycastHit _hit;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] protected int currentBullets = 6;
        [SerializeField] protected int currentTotalBullets = 30;
        [SerializeField] protected int maxTotalBullets = 30;
        [SerializeField] protected int maxBullets = 6;

        [SerializeField] private TMP_Text _bulletsText;
        [SerializeField] private GameObject _bulletPrefab;

        [SerializeField] private Transform _orientation;
        [SerializeField] protected ObjectPooling _bulletsPooling;

        [SerializeField] private bool isShopGun;
        [SerializeField] private bool gunBought;
        [SerializeField] private int price;
        private Vector3 startPos;
        private Quaternion startRot;

        [SerializeField] protected WeaponScriptAnimator _animator;
        public bool canShoot = true;

        protected string poolingName;
        public WeaponScriptAnimator Animator => _animator;
        public float BulletDamage => _bulletDamage;


        private void Start()
        {
            _animator = GetComponent<WeaponScriptAnimator>();
            _interactable = GetComponent<XRGrabInteractable>();
            _bulletsText = GetComponentInChildren<TMP_Text>();
            if (GetComponentInParent<ShopInstance>())
                _shopInstance = GetComponentInParent<ShopInstance>();
            if (GetComponentInParent<ShopInstance>() != null)
            {
                isShopGun = true;
                price = GetComponentInParent<ShopInstance>().ShopItem.GetPrice;
            }

            _interactable.selectEntered.AddListener(SelectEnter);
            _interactable.selectExited.AddListener(SelectExit);
            _interactable.hoverEntered.AddListener(HoverEnter);
            _interactable.hoverExited.AddListener(HoverExit);
            _interactable.activated.AddListener(PerformShoot);
            _interactable.deactivated.AddListener(EndShoot);

            startPos = transform.localPosition;
            startRot = transform.localRotation;

            _bulletsPooling =
                GameManager.Instance.objectPoolingManager.GetNewObjectPool(poolingName, ref _bulletPrefab, 5);
        }

        #region GrabbableEventsArgs

        private void SelectEnter(SelectEnterEventArgs args)
        {
            selector = args.interactorObject.transform.gameObject;
            if (isShopGun)
            {
                if (!gunBought)
                {
                    if (Vector3.Distance(transform.position, selector.transform.position) >= _shopInstance.Range)
                        return;
                    BuyGun();
                }
                else
                    GetGun();
            }
            else
                GetGun();
        }

        private bool TryBuyAmmo()
        {
            Player p = GameManager.Instance.players[0];

            if (p.leftHandItem != null)
            {
                if (p.leftHandItem.GetComponent<BaseGun>().currentBullets ==
                    p.leftHandItem.GetComponent<BaseGun>().maxBullets &&
                    p.leftHandItem.GetComponent<BaseGun>().currentTotalBullets ==
                    p.leftHandItem.GetComponent<BaseGun>().maxTotalBullets) return false;

                if (Utils.CheckTypes(typeof(RevolverVR), p.leftHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(ShotgunVR), p.leftHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(MachinegunVR), p.leftHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(SniperVR), p.leftHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(HollyGunVR), p.leftHandItem, _shopInstance.ShopItem.GetPrefab))
                {
                    p.leftHandItem.GetComponent<BaseGun>().FillUpAmmo();
                    return true;
                }
            }
            else if (p.rightHandItem != null)
            {
                if (p.rightHandItem.GetComponent<BaseGun>().currentBullets ==
                    p.rightHandItem.GetComponent<BaseGun>().maxBullets &&
                    p.rightHandItem.GetComponent<BaseGun>().currentTotalBullets ==
                    p.rightHandItem.GetComponent<BaseGun>().maxTotalBullets) return false;

                if (Utils.CheckTypes(typeof(RevolverVR), p.rightHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(ShotgunVR), p.rightHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(MachinegunVR), p.rightHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(SniperVR), p.rightHandItem, _shopInstance.ShopItem.GetPrefab) ||
                    Utils.CheckTypes(typeof(HollyGunVR), p.rightHandItem, _shopInstance.ShopItem.GetPrefab))
                {
                    p.rightHandItem.GetComponent<BaseGun>().FillUpAmmo();
                    return true;
                }
            }

            return false;
        }


        private void SelectExit(SelectExitEventArgs args)
        {
            if (isShopGun)
                if (gunBought)
                    DropGun();
                else
                    ResetGunPos();
            else
                DropGun();
        }

        private void HoverEnter(HoverEnterEventArgs args)
        {
            if (!isShopGun || gunBought) return;
            if (_shopInstance == null) return;
            SetInteractableLayerBasedOnPrice();
            UnableInteractableLayer();
        }

        private void HoverExit(HoverExitEventArgs args)
        {
            if (gunBought) return;
            ResetInteractableLayer();
        }

        #region SelectEnter

        protected virtual void GetGun()
        {
            _orientation.localRotation = Quaternion.Euler(0, -90, 0);
            if (selector.name == "Left Hand")
                GameManager.Instance.players[0].leftHandItem = gameObject;
            else
                GameManager.Instance.players[0].rightHandItem = gameObject;
        }

        private void BuyGun()
        {
            // if()
            if (GameManager.Instance.players.First().PlayerData._economy >= price / 3)
            {
                if (TryBuyAmmo())
                {
                    GameManager.Instance.players.First().PlayerData.Buy(price / 3);
                    return;
                }
            }

            if (GameManager.Instance.players.First().PlayerData._economy >= price)
            {
                GameManager.Instance.players.First().PlayerData.Buy(price);
                gunBought = true;
                GetGun();
                _shopInstance.RefillGun();
            }
        }

        #endregion

        #region SelectExit

        protected virtual void DropGun()
        {
            _interactable.interactionLayers = 2;
            GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = null;
            if (selector.name == "Left Hand")
                GameManager.Instance.players[0].leftHandItem = null;
            else
                GameManager.Instance.players[0].rightHandItem = null;
            selector = null;
        }

        private void ResetGunPos()
        {
            transform.localPosition = startPos;
            transform.localRotation = startRot;
        }

        #endregion

        #region HoverEnter

        private void SetInteractableLayerBasedOnPrice()
        {
            var currentEconomy = GameManager.Instance.players.First().PlayerData._economy;
            var currentPrice = _shopInstance.ShopItem.GetPrice;
            _interactable.interactionLayers = currentEconomy >= currentPrice ? 2 : 0;
        }

        private void UnableInteractableLayer()
        {
            _interactable.interactionLayers = 0;
        }

        #endregion

        #region HoverExit

        private void ResetInteractableLayer()
        {
            _interactable.interactionLayers = 2;
        }

        #endregion

        #endregion

        private void Update()
        {
            MoveBulletVR();
            DeleteBullets();
        }

        private void DeleteBullets()
        {
            _bulletsPooling.GetPool().ForEach(
                bullet =>
                {
                    if (!bullet.activeSelf) return;
                    BulletVR bvr = bullet.GetComponent<BulletVR>();
                    if (bvr._time >= bvr._waitTime)
                        bullet.SetActive(false);
                }
            );
        }

        private void PerformShoot(ActivateEventArgs args)
        {
            if(Time.timeScale < 1)return;
            Shoot();
        }

        private void EndShoot(DeactivateEventArgs args) => NoShoot();

        private Vector3 GetBulletWorldPosition(BulletVR bullet)
        {
            //p + v*t + 0.5*g*t*t
            Vector3 gravity = Vector3.down * _bulletDrop;
            return bullet._initialPos + bullet._initialVel * bullet._time +
                   gravity * (0.5f * bullet._time * bullet._time);
        }

        private void MoveBulletVR()
        {
            if (_bulletsPooling == null) return;
            _bulletsPooling.GetPool().ForEach(bullet =>
            {
                if (!bullet.activeSelf) return;

                var bulletVR = bullet.GetComponent<BulletVR>();
                Vector3 pos0 = GetBulletWorldPosition(bulletVR);
                bulletVR._time += Time.deltaTime;
                Vector3 pos1 = GetBulletWorldPosition(bulletVR);
                RayCastBulletSegment(pos0, pos1, bulletVR);
            });
        }

        private void RayCastBulletSegment(Vector3 start, Vector3 end, BulletVR bullet)
        {
            Vector3 direction = end - start;
            _ray.origin = start;
            _ray.direction = direction;

            if (Physics.Raycast(_ray, out _hit, direction.magnitude, _layerMask))
            {
                bullet._trail.transform.position = _hit.point;
                bullet._time = 3f;
                if (!CheckForForce(_hit.transform.gameObject, _hit.point, _ray.direction))
                {
                    var trf = _impactParticles.transform;
                    trf.position = _hit.point;
                    trf.forward = _hit.normal;
                    _impactParticles.Emit(1);
                }

                CheckForDamage(_hit.transform.gameObject);
            }
            else
            {
                bullet._trail.transform.position = end;
            }
        }

        protected virtual void Shoot()
        {

            if (currentBullets == 0)
            {
                if ((currentBullets == 0 && currentTotalBullets == 0))
                {
                    PlaySound();
                    return;
                }

                PlaySound();
                AudioManager.Instance.PlayOneShot("Reload");
                _animator.AnimationReload();
                if ((currentTotalBullets - maxBullets < 0))
                    currentTotalBullets = 0;
                else
                    currentTotalBullets -= maxBullets;
                currentBullets = maxBullets;
                return;
            }

            PlaySound();
            _animator.AnimationShoot();
        }

        protected bool CheckAmmo()
        {
            if (currentBullets <= 0 && currentTotalBullets == 0)
            {
                //no ammo, mayb puff sound or fart or smth
                currentBullets = 0;
                currentTotalBullets = 0;
                UpdateText();
                _animator.AnimationShoot();
                AudioManager.Instance.PlayOneShot("NoAmmo");
                canShoot = false;
                return false;
            }

            return true;
        }

        protected abstract void PlaySound();
        protected abstract void NoShoot();

        private bool CheckForForce(GameObject hitObject, Vector3 hitPosition, Vector3 direction)
        {
            bool hasRb = false;
            if (hitObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(1500f, hitPosition, 0.2f);
                hasRb = true;
            }

            return hasRb;
        }

        private void CheckForDamage(GameObject hitObject)
        {
            if (hitObject.TryGetComponent(out Damageable damageable))
                damageable.Damage(this);
        }

        protected void UpdateText() => _bulletsText.text =
            $"{currentBullets.ToString()}" + "\n" + "/" + "\n" + $"{currentTotalBullets.ToString()}";

        public void FillUpAmmo()
        {
            currentBullets = maxBullets;
            currentTotalBullets = maxTotalBullets;
            canShoot = true;
            UpdateText();
        }
    }
}