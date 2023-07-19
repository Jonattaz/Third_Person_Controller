using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    public AIStateId GetId(){
        return AIStateId.FindWeapon;

    }
    public void Enter(AIAgent agent){
        WeaponPickup pickup = FindClosestWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5;
    }

    public void Update(AIAgent agent){
        if(agent.weapons.HasWeapon()){
            agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
        }
    }

    public void Exit(AIAgent agent){
    }

    public WeaponPickup FindClosestWeapon(AIAgent agent){
        WeaponPickup[] weapons = Object.FindObjectsOfType<WeaponPickup>();
        WeaponPickup closestWeapon = null;
        float closestDistance = float.MaxValue;
        foreach (var weapon in weapons){
            float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDistance){
                closestDistance = distanceToWeapon;
                closestWeapon = weapon;
            }
        }
        return closestWeapon;
    }

}
