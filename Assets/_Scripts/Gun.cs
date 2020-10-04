using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Vector3 defaultRotation;
    [SerializeField] Vector3 sprintRotation;

    [SerializeField] PlayerController playerController;
    bool gunUp;
    bool transitioning;

    Coroutine rotateGunRoutine;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip fireClip;
    [SerializeField] GameObject muzzleFlashGameObject;
    bool canFire;
    bool firing;

    [SerializeField] Vector3 recoilRot;

    public Transform firePoint;

    public LayerMask layerMask;

    private void Start()
    {
        Vector3 pos = transform.localPosition;
        pos.z = -0.3f;
        transform.localPosition = pos;
        StartCoroutine(MoveTransform(transform, Vector3.zero, 0.5f));

        canFire = true; 
    }


    void Update()
    {
        if (!firing)
        {
            if (playerController.isRunning && !gunUp)
            {

                if (rotateGunRoutine != null)
                    StopCoroutine(rotateGunRoutine);

                rotateGunRoutine = StartCoroutine(RotateGun(sprintRotation, true, 1f));
            }
            else if (!playerController.isRunning && gunUp)
            {
                if (rotateGunRoutine != null)
                    StopCoroutine(rotateGunRoutine);

                rotateGunRoutine = StartCoroutine(RotateGun(defaultRotation, false, 1f));
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!playerController.isRunning && canFire && !ProgressManager.instance.lockPlayerMovement)
            {
                StartCoroutine(FireRoutine());
            }
        }
    }

    IEnumerator FireRoutine()
    {
        canFire = false;
        firing = true;

        if (rotateGunRoutine != null)
            StopCoroutine(rotateGunRoutine);

        audioSource.clip = fireClip;
        audioSource.Play();
        muzzleFlashGameObject.SetActive(true);

        FireRaycast();

        rotateGunRoutine = StartCoroutine(RotateGun(recoilRot, false, 0.05f));
        Vector3 pos = transform.localPosition;
        Vector3 recoilPos = pos;
        recoilPos.x += 0.02f;
        StartCoroutine(MoveTransform(transform, recoilPos, 0.15f));
        yield return new WaitForSeconds(0.1f);
        muzzleFlashGameObject.SetActive(false);
        StartCoroutine(MoveTransform(transform, pos, 0.15f));
        rotateGunRoutine = StartCoroutine(RotateGun(defaultRotation, false, 0.5f));
        yield return new WaitForSeconds(0.15f);
        canFire = true;
        firing = false;
    }

    public void FireRaycast()
    {
        RaycastHit hit;


        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 200, layerMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                MonsterAI ai = hit.transform.GetComponent<MonsterAI>();

                if (ai)
                {
                    ai.Shot();
                }
            }
        }
    }

    IEnumerator MoveTransform(Transform t, Vector3 target, float time)
    {
        float dt = 0;


        while (dt < time)
        {
            t.localPosition = Vector3.Lerp(t.localPosition, target, dt / time);
            dt += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }


    IEnumerator RotateGun(Vector3 targetRot, bool up, float time)
    {
        gunUp = up;

        float t = 0;


        while (t < time)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRot, t/time);
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame() ; 
        }
    }
}
