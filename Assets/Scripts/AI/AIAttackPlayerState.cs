using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState{
   
    public AIStateId GetId(){
        return AIStateId.AttackPlayer;
    }

    public void Enter(AIAgent agent){
        agent.weapons.ActiveWeapon();
        agent.weapons.SetTarget(agent.playerTransform);
        agent.navMeshAgent.stoppingDistance = 5.0f;
    }

    public void Update(AIAgent agent){  
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }

    public void Exit(AIAgent agent){
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
