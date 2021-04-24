using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    private MeshRenderer meshRenderer;
    public Material transparentMaterial;
    public static float fadeSpeed = 2f;
    private Material opaqueMaterial;
    private ParticleSystem particleSystem;

    public enum FadeState {
        Fading, Opaque, UnFading
    }

    public FadeState fadeState = FadeState.Opaque;
    
    public float alpha = 0;
    private bool emissionState = false;
    
    // Start is called before the first frame update
    void Start() {
        
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem) {
            var v = particleSystem.emission;
            emissionState = v.enabled;
        }

        meshRenderer = GetComponent<MeshRenderer>();
        opaqueMaterial = meshRenderer.material;
        if (transparentMaterial != null) {
            transparentMaterial = Instantiate(transparentMaterial);
        } else {
            transparentMaterial = Instantiate(meshRenderer.material);
        }

        meshRenderer.material = transparentMaterial;
    }

    public static void setStates(GameObject gameObject, FadeState fadeState) {
        setStates(gameObject.transform, fadeState);
    }
    
    public static void setStates(Transform tf, FadeState fadeState) {
        Fade fade = tf.gameObject.GetComponent<Fade>();
        if (fade != null) {
            fade.fadeState = fadeState;
        }
        foreach (Transform childTf in tf) {
            fade = childTf.gameObject.GetComponent<Fade>();
            if (fade != null) {
                fade.fadeState = fadeState;
            }

            Fade.setStates(childTf, fadeState);
        }

    }

    // Update is called once per frame
    void Update() {
        Color c = transparentMaterial.color;
        ParticleSystem.EmissionModule v;
        
        switch (fadeState) {
            case FadeState.Fading:
                meshRenderer.material = transparentMaterial;
                c.a = Mathf.Clamp01(c.a - (Time.deltaTime * fadeSpeed));
                if (particleSystem) {
                    if (c.a > 0) {
                        enableEmission();
                    } else {
                        if (emissionState) {
                            Invoke(nameof(disableEmission), 1.5f);
                            emissionState = false;
                        }
                    }
                }

                break;
            case FadeState.Opaque:
                meshRenderer.material = opaqueMaterial;
                c.a = 1.0f;
                if (emissionState) {
                    disableEmission();
                }

                break;
            case FadeState.UnFading:
                meshRenderer.material = transparentMaterial;
                c.a = Mathf.Clamp01(c.a + (Time.deltaTime * fadeSpeed * 0.5f));
                if (c.a >= 0.98) {
                    fadeState = FadeState.Opaque;
                }

                if (!emissionState) {
                    enableEmission();
                }

                break;
        }
        
        alpha = c.a;
        transparentMaterial.color = c;
    }

    public void enableEmission() {
        setEmissionState(true);
    }
    public void disableEmission() {
        setEmissionState(false);
    }
    
    public void setEmissionState(bool emission) {
        if (particleSystem) {
            var v = particleSystem.emission;
            v.enabled = emission;
        }
        emissionState = emission;
    }
    
    public void SetAlpha(float alpha) {
        Color c = transparentMaterial.color;
        c.a = alpha;
        transparentMaterial.color = c;
    }
}
