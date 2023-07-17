using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerState : AIState{
    
    float timer = 0.0f;

    public AIStateId GetId(){
        return AIStateId.ChasePlayer;
    }

    public void Enter(AIAgent agent){
    }

    public void Update(AIAgent agent){
        if(!agent.enabled){
            return;
        }

        timer -= Time.deltaTime;
        
        if(!agent.navMeshAgent.hasPath){
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if(timer < 0.0f){   
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if(direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance){
                if(agent.navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial){
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIAgent agent){
        
    }
}
