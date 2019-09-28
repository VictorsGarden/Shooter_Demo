using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class Weapons : MonoBehaviour {

    public GameObject weaponSlot;
    public Camera mainCamera;
    public GameObject torso;
    public Transform cameraOrbit;
    public LayerMask layermask;
    // Crosshair
    private Rect crosshairPlace;
    private Texture crosshairTexture;
    private float crosshairSize = Screen.width * 0.03f;
    private float crosshairRegulator = (float)(Screen.height / 8);
    // Weapons
    private AimIK aimScript;
    private GameObject currentWeaponObject;
    private Weapon currentWeaponInstance;
    private RangedWeapon currentRangedWeapon;
    private MeleeWeapon currentMeleeWeapon;
    private Animator animator;
    private GameObject weaponAim;
    private int currentWeaponNumber;
    private int weaponsAmount;
    private Weapon[] weapons;
    private float scroll;
    private string[] weaponsNames;
    // Firing
    private GameObject bullet;
    private Bullet bulletScript;
    private bool isFiring;
    private Vector3 fireDirection;
    private bool notSerialFire;
    private bool peaceModeBreaker;
    // Aiming
    private byte zoomCondition;
    private const byte NOT_ZOOM = 0;
    private const byte ZOOM_IN = 1;
    private const byte ZOOM_OUT = 2;
    private const float AIMING_SPEED = 8f;
    private float fieldOfViewNormal;
    private const float FIELD_OF_VIEW_ZOOMED = 18f;
    private const float CAM_VERTICAL_NORMAL = 1.2f;
    private const float CAM_VERTICAL_ZOOMED = 1.6f;
    private const float CAM_HORIZONTAL_DEFAULT = 0.6f;
    private const float CAM_HORIZONTAL_SWITCHED = -0.68f;
    private const float PERMISSIBLE_DIFFERENCE = 0.011f;
    // Reloading
    private bool isReloading;
    // Changing
    private bool isChangingWeapon;
    // Audio
    private AudioSource audioSource;
    private bool fightingMode;
    // Fighting mode
    private const float FIGHTING_OFF_TIMER = 3.5f;
    // Scripts;
    private Movements movementsScript;
    private CameraController camerasScript;

    public bool FightingMode {
        get { return fightingMode; }
        set { fightingMode = value; }
    }

    public byte getAimingMarker {
        get { return this.zoomCondition; }
    }

    // Basic class for all weapons
    public abstract class Weapon {
        public int weaponIndex;
        public string weaponName;
        public GameObject weaponType;
        public int damage;
        public float equipRate;
        public float holsterRate;
        public AudioClip attackSound;
    }

    public class RangedWeapon : Weapon {

        public int magazineCapacity;
        public float range;
        public float serialRate;
        public float reloadRate;
        public int chargers;
        public AudioClip reloadingSound;
        public ParticleSystem shootingEffects;
        public int ammo;

        public RangedWeapon(
            int weaponIndex,
            string weaponName,
            GameObject weaponType,
            int magazineCapacity,
            float range,
            int damage,
            float serialRate,
            float reloadRate,
            float equipRate,
            float holsterRate,
            int chargers,
            AudioClip attackSound,
            AudioClip reloadingSound,
            ParticleSystem shootingEffects) {

            this.weaponIndex = weaponIndex;
            this.weaponName = weaponName;
            this.weaponType = weaponType;
            this.magazineCapacity = magazineCapacity;
            this.chargers = chargers;
            this.range = range;
            this.damage = damage;
            this.serialRate = serialRate;
            this.reloadRate = reloadRate;
            this.equipRate = equipRate;
            this.holsterRate = holsterRate;
            this.ammo = magazineCapacity;
            this.attackSound = attackSound;
            this.reloadingSound = reloadingSound;
            this.shootingEffects = shootingEffects;
        }
    }

    public class MeleeWeapon : Weapon {

        public MeleeWeapon(
            int weaponIndex,
            string weaponName,
            GameObject weaponType,
            int damage,
            float equipRate,
            float holsterRate,
            AudioClip attackSound) {

            this.weaponIndex = weaponIndex;
            this.weaponName = weaponName;
            this.weaponType = weaponType;
            this.damage = damage;
            this.equipRate = equipRate;
            this.holsterRate = holsterRate;
            this.attackSound = attackSound;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
        aimScript = GetComponent<AimIK>();
        movementsScript = GetComponent<Movements>();
        camerasScript = Camera.main.GetComponent<CameraController>();

        // Aiming
        fieldOfViewNormal = Camera.main.fieldOfView;
        // Audio
        audioSource = GetComponent<AudioSource>();

        #region Weapons
        // Fighting mode
        SetFightingModeOff();

        //Sword initiating
        Weapon sword = new MeleeWeapon(
            0,          // index
            "Sword",    // name
            weaponSlot.transform.GetChild(0).gameObject,
            20,         // damage
            2f,       // equip rate
            0.8f,       // holster rate
            (AudioClip)Resources.Load("Sounds/sword_sounds/sword_swish_1")
        );

        // Pistol initiating
        Weapon pistol = new RangedWeapon(
            1,          // index
            "Pistol",   // name
            weaponSlot.transform.GetChild(1).gameObject,
            10,         // magazine capacity
            250f,       // range
            30,         // damage
            0.4f,       // serial rate
            2.167f,     // reload rate
            0.867f,     // equip rate
            1.1f,       // holster rate
            15,         // chargers
            (AudioClip)Resources.Load("Sounds/pistol_sounds/pistol_shot"),
            (AudioClip)Resources.Load("Sounds/pistol_sounds/pistol_reload"),
            weaponSlot.transform.GetChild(1).GetChild(1).GetComponent<ParticleSystem>()
        );

        // Rifle initiating
        Weapon rifle = new RangedWeapon(
            2,          // index
            "Rifle",    // name
            weaponSlot.transform.GetChild(2).gameObject,
            30,         // magazine capacity
            500f,      // range
            50,         // damage
            0.12f,      // serial rate
            2.167f,     // reload rate
            1.8f,       // equip rate
            1.2f,       // holster rate
            10,         // chargers
            (AudioClip)Resources.Load("Sounds/rifle_sounds/m4a4_shot"),
            (AudioClip)Resources.Load("Sounds/rifle_sounds/m4a4_reload"),
            weaponSlot.transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>()
        );

        currentWeaponObject = new GameObject();
        weapons = new Weapon[] { sword, pistol, rifle };

        weaponsNames = new string[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
            weaponsNames[i] = weapons[i].weaponName;

        weaponsAmount = weapons.Length - 1;

        if (weapons.Length > 1)
            for (int i = 0; i <= weaponsAmount; i++)
                weaponSlot.transform.GetChild(i).gameObject.SetActive(false);

        // Default weapon
        currentWeaponNumber = rifle.weaponIndex;
        currentWeaponInstance = weapons[currentWeaponNumber];
        StartCoroutine(SetWeapon(currentWeaponInstance.equipRate));
        #endregion
    }

    private void Update() {
        scroll = Input.GetAxis("Mouse ScrollWheel");

        if (!isChangingWeapon && weaponSlot.transform.childCount > 1 && !isFiring && !isReloading && scroll != 0)
            StartCoroutine(ChangeWeapon(scroll, currentWeaponInstance.holsterRate));

        #region Attack conditions
        if (!isChangingWeapon && !isReloading && movementsScript.getRun < 1) {

            if (currentWeaponInstance.GetType() == typeof(RangedWeapon)) {

                if (Input.GetMouseButtonDown(0)) {
                    notSerialFire = true;

                    if (!FightingMode) {
                        peaceModeBreaker = true;
                        Invoke(("SetFightingModeOn"), 0);
                        Invoke("Fire", 0.3f);
                        Invoke(("ClearFiringState"), currentRangedWeapon.serialRate);

                    } else {
                        Invoke("ClearFiringState", currentRangedWeapon.serialRate);
                        Fire();
                    }

                } else if (Input.GetMouseButton(0) && !IsInvoking("Fire") && !notSerialFire) {
                    Invoke("SetFightingModeOn", 0);
                    Invoke("Fire", currentRangedWeapon.serialRate);

                } else if (Input.GetMouseButtonUp(0)) {

                    if (!peaceModeBreaker)
                        CancelInvoke("Fire");

                    ResetFightingModeCloser();
                    notSerialFire = true;
                }

                if (Input.GetKeyDown(KeyCode.R) && currentRangedWeapon.chargers > 0
                    && currentRangedWeapon.ammo != currentRangedWeapon.magazineCapacity)
                    StartCoroutine(SetReloadCondition(true, currentRangedWeapon.reloadRate));

            } else if (currentWeaponInstance.GetType() == typeof(MeleeWeapon)) {
                if (Input.GetMouseButtonDown(0)) {

                }
            }
        }
        #endregion

        #region Aiming conditions
        if (currentWeaponInstance.GetType() == typeof(RangedWeapon)) {

            if (Input.GetMouseButtonDown(1)) {
                Invoke("SetFightingModeOn", 0);
            }

            if (Input.GetMouseButton(1))
                this.zoomCondition = ZOOM_IN;

            else if (Input.GetMouseButtonUp(1)) {
                this.zoomCondition = ZOOM_OUT;

                if (IsInvoking("ResetZoomOut"))
                    CancelInvoke("ResetZoomOut");

                Invoke("ResetZoomOut", 0.6f);
                ResetFightingModeCloser();
            }
        }

        if (this.zoomCondition == ZOOM_IN || this.zoomCondition == ZOOM_OUT)
            Aiming(this.zoomCondition);
        #endregion
    }

    private void LateUpdate() {
        if ((movementsScript.getGroundedMarker && (Input.GetButton("Run"))) || isChangingWeapon || !FightingMode || currentWeaponInstance.GetType() == typeof(MeleeWeapon))
            ResetAimCondition();
        else if (FightingMode && !isChangingWeapon)
            StartCoroutine(SetAimCondition(1f, 0.35f));
    }

    private void ResetZoomOut() {
        this.zoomCondition = NOT_ZOOM;
    }

    private IEnumerator SetAimCondition(float aimWeight, float waitingTimer) {
        yield return new WaitForSeconds(waitingTimer);
        aimScript.solver.IKPositionWeight = aimWeight;
    }

    private void ResetAimCondition() {
        aimScript.solver.IKPositionWeight = 0;
    }

    private void ResetFightingModeCloser() {
        if (IsInvoking("SetFightingModeOff"))
            CancelInvoke("SetFightingModeOff");

        Invoke("SetFightingModeOff", FIGHTING_OFF_TIMER);
    }

    private void SetFightingModeOn() {
        FightingMode = true;
        animator.SetBool("FightingMode", true);
    }

    private void SetFightingModeOff() {
        if (!isFiring && zoomCondition != ZOOM_IN && !isReloading && !isChangingWeapon) {
            FightingMode = false;
            animator.SetBool("FightingMode", false);
        }
    }

    private void ClearFiringState() {
        notSerialFire = false;
    }

    // Crosshair logic
    private void OnGUI() {
        if (currentWeaponInstance.GetType() == typeof(RangedWeapon)) {
            crosshairTexture = Resources.Load("Textures/crosshair") as Texture;
            crosshairPlace = new Rect(
                                    Screen.width / 2 - crosshairSize / 2,
                                    Screen.height / 2 - crosshairSize / 2 - crosshairRegulator,
                                    crosshairSize, crosshairSize
                                    );
            GUI.DrawTexture(crosshairPlace, crosshairTexture);
        }
    }

    // Change weapon
    private IEnumerator ChangeWeapon(float scroll, float waitingTimer) {
        this.isChangingWeapon = true;

        animator.SetBool("HolsterWeapon", true);
        // Waiting, when animation of holstering will be finished
        yield return new WaitForSeconds(waitingTimer);
        animator.SetBool("HolsterWeapon", false);
        weaponSlot.transform.GetChild(currentWeaponInstance.weaponIndex).gameObject.SetActive(false);

        if (scroll > 0)

            if (currentWeaponNumber == weaponsAmount)
                currentWeaponNumber = 0;
            else
                currentWeaponNumber++;

        else if (scroll < 0)

            if (currentWeaponNumber == 0)
                currentWeaponNumber = weaponsAmount;
            else
                currentWeaponNumber--;

        currentWeaponInstance = weapons[currentWeaponNumber];
        StartCoroutine(SetWeapon(currentWeaponInstance.equipRate));
    }

    // Putting weapon to the weapon slot
    private IEnumerator SetWeapon(float waitingTimer) {
        currentRangedWeapon = null;
        currentMeleeWeapon = null;

        if (currentWeaponInstance.GetType() == typeof(RangedWeapon)) {
            currentRangedWeapon = (RangedWeapon)currentWeaponInstance;

        } else if (currentWeaponInstance.GetType() == typeof(MeleeWeapon)) {
            aimScript.solver.IKPositionWeight = 0;
            currentMeleeWeapon = (MeleeWeapon)currentWeaponInstance;
        }

        aimScript.solver.transform = weaponSlot.transform.GetChild(currentWeaponNumber).GetChild(0).transform;
        currentWeaponObject = currentWeaponInstance.weaponType;
        // Put into the hand the next weapon model
        SetWeaponConditionForAnimator(currentWeaponNumber);
        animator.SetBool("EquipWeapon", true);
        Invoke("PickUpWeapon", currentWeaponInstance.equipRate / 4);

        // Waiting, when animation of picking up new weapon will be finished
        yield return new WaitForSeconds(waitingTimer);
        animator.SetBool("EquipWeapon", false);
        this.isChangingWeapon = false;
    }

    private void PickUpWeapon() {
        currentWeaponObject.SetActive(true);
    }

    // Reset all weapons indexes except current
    private void SetWeaponConditionForAnimator(int currentWeaponNumber) {
        for (int i = 0; i < weaponsNames.Length; i++) {
            if (currentWeaponNumber == i) {
                animator.SetBool(weaponsNames[i], true);
            } else {
                animator.SetBool(weaponsNames[i], false);
            }
        }
    }

    private void Fire() {
        if (currentRangedWeapon.ammo > 0) {
            isFiring = true;
            // Init bullet trace
            audioSource.PlayOneShot(currentRangedWeapon.attackSound);
            currentRangedWeapon.shootingEffects.Play();
            // bullet trace
            Transform currentAimTransform = weaponSlot.transform.GetChild(currentWeaponNumber).GetChild(0).transform;
            // Logic
            currentRangedWeapon.ammo--;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(
                    Screen.width / 2,
                    Screen.height / 2 + crosshairRegulator
                    ));

            Vector3 startingPoint = ray.origin + ray.direction * (camerasScript.getDistance + 0.3f);

            RaycastHit hit;

            // if there is some object on the weapon range distance - it became a target and rotation point
            if (Physics.Raycast(startingPoint, ray.direction, out hit, currentRangedWeapon.range, layermask)) {
                fireDirection = (hit.point - currentAimTransform.position).normalized;

            // otherwise take target and rotation point from max possibility of current weapon range
            } else {
                Vector3 endPoint = startingPoint + ray.direction * currentRangedWeapon.range;
                fireDirection = (endPoint - currentAimTransform.position).normalized;
            }

            // Get Bullet from pool
            bullet = PoolManager.GetObject("Bullet", currentAimTransform.position, Quaternion.LookRotation(fireDirection));
            bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetWeaponRange = currentRangedWeapon.range;
            bulletScript.SetWeaponDamage = currentRangedWeapon.damage;
            // Arrange options in script, attached to current bullet object
            bulletScript.SetStartingPoint = bullet.transform.position;
            bulletScript.SetTraveledDistance = 0;
            bulletScript.BulletFlyMarker = true;

            animator.SetInteger("FiringWeapon", currentWeaponNumber);
            Invoke("ResetFiringWeapon", currentRangedWeapon.serialRate);

            peaceModeBreaker = false;

            if (currentRangedWeapon.ammo == 0)
                StartCoroutine(SetReloadCondition(true, currentRangedWeapon.reloadRate));
        }
    }

    private void Attack() {
        animator.SetInteger("FiringWeapon", currentWeaponNumber);
    }

    private IEnumerator SetReloadCondition(bool reload, float waitingTimer) {
        if (reload) {
            Invoke("SetFightingModeOn", 0);
            isReloading = true;
            animator.SetBool("Reload", true);

            audioSource.PlayOneShot(currentRangedWeapon.reloadingSound);
            currentRangedWeapon.chargers--;
            currentRangedWeapon.ammo = currentRangedWeapon.magazineCapacity;

            // Reqursively calls itself for clear the state
            yield return new WaitForSeconds(waitingTimer);
            StartCoroutine(SetReloadCondition(false, 0));
        } else {
            yield return new WaitForSeconds(waitingTimer);
            animator.SetBool("Reload", false);
            isReloading = false;
            ResetFightingModeCloser();
        }
    }

    private void ResetFiringWeapon() {
        animator.SetInteger("FiringWeapon", -1);
        isFiring = false;
    }

    // Zooming camera for aiming the target
    private void Aiming(float zoomCondition) {
        float cameraOrbitX = transform.InverseTransformPoint(cameraOrbit.position).x;
        float cameraOrbitY = transform.InverseTransformPoint(cameraOrbit.position).y;

        float newFieldOfView = zoomCondition == ZOOM_IN ? FIELD_OF_VIEW_ZOOMED : fieldOfViewNormal;
        float newVerticalPosition = zoomCondition == ZOOM_IN ? CAM_VERTICAL_ZOOMED : CAM_VERTICAL_NORMAL;
        float newHorizontalPosition = cameraOrbitX >= 0 ? CAM_HORIZONTAL_DEFAULT : CAM_HORIZONTAL_SWITCHED;

        if (Mathf.Abs(cameraOrbitY - newVerticalPosition) < PERMISSIBLE_DIFFERENCE)
            cameraOrbit.position = transform.TransformPoint(new Vector3(newHorizontalPosition, newVerticalPosition));

        if (Mathf.Abs(Camera.main.fieldOfView - newFieldOfView) < PERMISSIBLE_DIFFERENCE)
            Camera.main.fieldOfView = newFieldOfView;

        if (cameraOrbit.position.y != newVerticalPosition || Camera.main.fieldOfView != newFieldOfView) {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFieldOfView, Time.deltaTime * AIMING_SPEED);
            cameraOrbit.position = Vector3.Lerp(cameraOrbit.position, transform.TransformPoint(new Vector3(newHorizontalPosition, newVerticalPosition)), Time.deltaTime * AIMING_SPEED);
        }
    }
}
