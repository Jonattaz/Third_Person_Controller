using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState{
   
    public Vector2 direction;

    public AIStateId GetId(){
        return AIStateId.Death;
    }

    public void Enter(AIAgent agent){
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction *agent.config.dieForce);
        agent.ui.gameObject.SetActive(false);
        agent.mesh.updateWhenOffscreen = true; 
        agent.weapons.DropWeapon();       
    }

    public void Update(AIAgent agent){

    }

    public void Exit(AIAgent agent){

    }
}
