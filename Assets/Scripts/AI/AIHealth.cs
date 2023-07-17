using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    public float maxHealth;
    public float currenthealth;
    AIAgent agent;
    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Start is called before the first frame update
    void Start(){
        agent = GetComponent<AIAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        currenthealth = maxHealth;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies){
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction){
        currenthealth -= amount;
        healthBar.SetHealthBarPercentage(currenthealth / maxHealth);
        if(currenthealth <= 0.0f){      
            Die(direction);
        }
        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction){
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateId.Death);
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update(){
        blinkTimer -= Time.deltaTime; 
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        if(currenthealth < maxHealth)
            skinnedMeshRenderer.material.color = Color.red * intensity;    
    }
}
