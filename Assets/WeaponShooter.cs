using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class WeaponShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField, Min(1f)] private float range = 100f;
    [SerializeField, Min(0f)] private float damage = 25f;
    [SerializeField, Min(0.02f)] private float fireRate = 0.2f;
    [SerializeField] private LayerMask hitLayers = ~0;

    [Header("Ammunition")]
    [SerializeField, Min(1)] private int magazineSize = 12;
    [SerializeField, Min(0)] private int reserveAmmo = 100;
    [SerializeField, Min(0.1f)] private float reloadTime = 1.5f;
    [SerializeField] private TMP_Text ammoText;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;
    private TMP_Text crosshairText;
    private Color crosshairColor = Color.white;
    private Coroutine hitMarkerRoutine;

    private void Awake()
    {
        if (fpsCamera == null) fpsCamera = GetComponentInParent<Camera>();

        GameObject crosshair = GameObject.Find("Crosshair");
        if (crosshair != null)
        {
            crosshairText = crosshair.GetComponent<TMP_Text>();
            if (crosshairText != null) crosshairColor = crosshairText.color;
        }
    }

    private void Start()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (GameRuntime.IsPaused || fpsCamera == null) return;

        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        if (keyboard.rKey.wasPressedThisFrame) StartReload();
        if (isReloading) return;

        if (mouse.leftButton.isPressed && Time.time >= nextFireTime)
        {
            if (currentAmmo <= 0)
            {
                StartReload();
                return;
            }

            nextFireTime = Time.time + fireRate;
            currentAmmo--;
            Shoot();
            UpdateAmmoUI();
        }
    }

    private void Shoot()
    {
        ProceduralAudio.PlayShot();
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out RaycastHit hit, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        TargetHealth target = hit.collider.GetComponentInParent<TargetHealth>();
        if (target != null && target.TakeDamage(damage))
        {
            ProceduralAudio.PlayHit();
            ShowHitMarker();
        }
    }

    private void ShowHitMarker()
    {
        if (crosshairText == null) return;
        if (hitMarkerRoutine != null) StopCoroutine(hitMarkerRoutine);
        hitMarkerRoutine = StartCoroutine(HitMarkerRoutine());
    }

    private IEnumerator HitMarkerRoutine()
    {
        crosshairText.color = Color.white;
        crosshairText.transform.localScale = Vector3.one * 1.35f;
        yield return new WaitForSeconds(0.08f);
        crosshairText.color = crosshairColor;
        crosshairText.transform.localScale = Vector3.one;
        hitMarkerRoutine = null;
    }

    private void StartReload()
    {
        if (isReloading || currentAmmo >= magazineSize || reserveAmmo <= 0) return;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        ProceduralAudio.PlayReload();
        if (ammoText != null) ammoText.text = "RELOADING...";
        yield return new WaitForSeconds(reloadTime);

        int ammoToLoad = Mathf.Min(magazineSize - currentAmmo, reserveAmmo);
        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;
        isReloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null) ammoText.text = $"AMMO: {currentAmmo} / {reserveAmmo}";
    }

    private void OnDisable()
    {
        isReloading = false;
        if (crosshairText != null)
        {
            crosshairText.color = crosshairColor;
            crosshairText.transform.localScale = Vector3.one;
        }
    }

    private void OnValidate()
    {
        range = Mathf.Max(1f, range);
        damage = Mathf.Max(0f, damage);
        fireRate = Mathf.Max(0.02f, fireRate);
        magazineSize = Mathf.Max(1, magazineSize);
        reserveAmmo = Mathf.Max(0, reserveAmmo);
        reloadTime = Mathf.Max(0.1f, reloadTime);
    }
}
