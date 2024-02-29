using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicator : MonoBehaviour
{
    private Transform player;
    private float dashDuration;
    private float dashCooldown;
    private float totalSpeed;

    private Vector3 idlePosition;
    private Vector3 rotation;
    private MeshRenderer meshRenderer;
    private Light pointLight;

    private float lightIntensity;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        pointLight = GetComponentInChildren<Light>();
        lightIntensity = pointLight.intensity;
    }

    public void Dash(Transform player, float dashCooldown, float dashDuration, float totalSpeed)
    {
        if (this.player != null) return;
        gameObject.SetActive(true);
        meshRenderer.enabled = true;
        rotation = player.rotation.eulerAngles;
        this.player = player;
        this.dashCooldown = dashCooldown;
        this.dashDuration = dashDuration;
        this.totalSpeed = totalSpeed;
        transform.position = player.position;
        StartCoroutine(TeleportToDashEndpoint());
    }

    private IEnumerator TeleportToDashEndpoint()
    {
        yield return new WaitForEndOfFrame();
        float range = totalSpeed * dashDuration;
        idlePosition = transform.position + (player.forward * range); 
        transform.position = idlePosition;
    }

    private void Update()
    {
        if (player == null) return;
        transform.position = idlePosition;
        transform.eulerAngles = rotation;
        ReduceLight();
    }

    private void ReduceLight()
    {
        float speed = lightIntensity / dashCooldown;
        pointLight.intensity -= speed * Time.deltaTime;
    }

    public void MoveToPlayer()
    {   
        if (idlePosition == Vector3.zero) return;
        if (player == null) return;
        meshRenderer.enabled = false;
        idlePosition = player.position;
        pointLight.intensity = lightIntensity * 2;
        StartCoroutine(MoveToPlayerInternal());
    }

    public IEnumerator MoveToPlayerInternal()
    {
        yield return new WaitForSeconds(0.1f);
        player = null;
        idlePosition = Vector3.zero;
        pointLight.intensity = lightIntensity;
        gameObject.SetActive(false);
    }
}
