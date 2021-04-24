using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBehavior : MonoBehaviour
{
    public enum BossState {
        FacePlayer, Spin, Attack, Roar, Intro, Waiting, Assembling
    }

    public GameObject[] layers;
    public float[] rotationSpeed;
    public float[] rotationAcc;

    public float reverseSpeed = 0.01f;
    public float accDelta = 0.01f;
    public float maxRotateAcc = 0.05f;
    public float maxRotate = 1;

    public BossState bossState = BossState.Intro;
    public float stateTimer = 0;
    public float statePeriod = 5f;
    
    private FaceBehavior faceBehavior;
    private ExplodeAndAssemble explodeAndAssemble;
    public GameObject target;
    public AudioSource screetchSource;
    public AudioSource shootSource;
    
    
    // Start is called before the first frame update
    void Start() {
        rotationSpeed = new float[layers.Length];
        rotationAcc = new float[layers.Length];
        faceBehavior = GetComponentInChildren<FaceBehavior>();
        explodeAndAssemble = GetComponent<ExplodeAndAssemble>();
        stateTimer = statePeriod;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (bossState) {
            case BossState.Waiting:
                Waiting();
                break;
            case BossState.Assembling:
                Assembling();
                break;
            case BossState.Intro:
                Intro();
                break;
            case BossState.Roar:
                Roar();
                break;
            case BossState.Spin:
                Spin();
                break;
            case BossState.FacePlayer:
                FacePlayer();
                break;
            case BossState.Attack:
                Attack();
                break;
        }

        if (bossState != BossState.Waiting && bossState != BossState.Assembling) {
            for (int i = 0; i < layers.Length; i++) {
                rotationAcc[i] += accDelta * Random.Range(0.1f, 1f) *
                                  Mathf.Sin(Time.time * 2 * Mathf.PI * reverseSpeed);
                rotationAcc[i] = Mathf.Clamp(rotationAcc[i], -maxRotateAcc, maxRotateAcc);

                rotationSpeed[i] += rotationAcc[i];
                rotationSpeed[i] = Mathf.Clamp(rotationSpeed[i], -maxRotate, maxRotate);

                layers[i].transform.Rotate(Vector3.up, rotationSpeed[i] * Time.deltaTime * 360f);
            }
        }

        if (stateTimer <= 0f) {
            stateTimer = statePeriod;
        } else {
            stateTimer -= Time.deltaTime;
        }
    }

    public void Intro() {
        Roar();
        if (stateTimer <= 0) {
            bossState = BossState.FacePlayer;
        }
    }
    
    public void Roar() {
        SlerpLib.LookAtSlerp(faceBehavior.faceTf, transform.position + transform.up * 10 + transform.forward * 10, Time.deltaTime);
        Vector3 facePos = faceBehavior.faceTf.localPosition;
        faceBehavior.faceTf.localPosition = Vector3.Lerp(facePos, Vector3.up * 3 + Vector3.forward * 3, Time.deltaTime);
        faceBehavior.openDistance = 5f + Random.Range(-2f, 2f);
        faceBehavior.mouthState = FaceBehavior.MouthState.Idle;
        faceBehavior.eyeState = FaceBehavior.EyeState.Idle;
        
        if (stateTimer <= 0) {
            screetchSource.loop = false;
            stateTimer = statePeriod * 3f;
            bossState = BossState.Attack;
        }
    }

    public GameObject projectile;
    public float attackPeriod = 2f;
    public int projCount = 3;
    public float projSpread = 35;
    private float attackTimer = 2f;
    
    public void Attack() {
        FacePlayer();

        if (attackTimer <= 0.5f && !shootSource.isPlaying) {
            shootSource.Play();
        }
        
        if (attackTimer <= 0f) {
            for (int i = 0; i < projCount; i++) {
                float ang = (((float) i) - (((float) projCount - 1) * 0.5f)) * (projSpread / (((float) projCount - 1) * 0.5f));
                var obj =  Instantiate(projectile, faceBehavior.faceTf.position,  faceBehavior.faceTf.rotation);
                obj.transform.LookAt(target.transform);
                obj.transform.Rotate(Vector3.up, ang);
                obj.transform.position += obj.transform.forward * 1f;
                var rb = obj.GetComponent<Rigidbody>();
                rb.velocity = obj.transform.forward * 30f;
            }
            
            attackTimer = attackPeriod;
        } else {
            attackTimer -= Time.deltaTime;
        }
        
        if (stateTimer <= 0) {
            bossState = BossState.Spin;
        }
    }
    
    public void Spin() {
        SlerpLib.LookAtSlerp(faceBehavior.faceTf, transform.forward * 10, Time.deltaTime);
        faceBehavior.faceTf.localRotation = Quaternion.Slerp(faceBehavior.faceTf.localRotation, Quaternion.identity, Time.deltaTime);
        faceBehavior.mouthState = FaceBehavior.MouthState.Breathe;
        faceBehavior.eyeState = FaceBehavior.EyeState.Stare;
        if (stateTimer <= 0) {
            stateTimer = statePeriod * 2f;
            bossState = BossState.FacePlayer;
        }
    }
    public void FacePlayer() {
        SlerpLib.LookAtSlerp(transform, target.transform.position, Time.deltaTime * 2f, true);
        
        
        faceBehavior.faceTf.localRotation = Quaternion.Slerp(faceBehavior.faceTf.localRotation, Quaternion.identity, Time.deltaTime);
        faceBehavior.faceTf.localPosition = Vector3.Lerp(faceBehavior.faceTf.localPosition, Vector3.zero, Time.deltaTime);
        faceBehavior.mouthState = FaceBehavior.MouthState.Breathe;
        faceBehavior.eyeState = FaceBehavior.EyeState.Stare;
        
        if (stateTimer <= 0) {
            screetchSource.Play();
            screetchSource.loop = true;
            stateTimer = statePeriod * 2f;
            bossState = BossState.Roar;
        }
    }

    private bool initialRoar = true;
    public void Waiting() {

        if (stateTimer <= 1.5f && initialRoar) {
            screetchSource.Play();
            screetchSource.loop = true;
            initialRoar = false;
        }

        if (stateTimer <= 0) {
            stateTimer = statePeriod * 2f;
            bossState = BossState.Assembling;
            
        }
    }
    
    public void Assembling() {
        explodeAndAssemble.startAssembly = true;
        SlerpLib.LookAtSlerp(transform, target.transform.position, Time.deltaTime * 2f, true);
        Roar();
        
        
        if (explodeAndAssemble.isAssembled) {
            stateTimer = statePeriod;
            bossState = BossState.Roar;
        } else {
            bossState = BossState.Assembling;
        }
    }
}
