using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet{
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }
    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    // 25 bullets per second
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public int maxBounces = 0; 
    public float maxLifeTime = 3.0f;
    public ParticleSystem hitEffect;
    public ParticleSystem[] muzzleFlash;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public TrailRenderer tracerEffect;
    public string weaponName;
    public AmmoWidget ammoWidget;
    public int ammoCount;
    public int clipSize; 
    public int damage = 10;

    public WeaponRecoil recoil;
    public GameObject magazine;
    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();

    void Awake(){
        recoil = GetComponent<WeaponRecoil>();
        ammoWidget = FindObjectOfType<AmmoWidget>();
    }

    Vector3 GetPosition(Bullet bullet){
        // p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity){
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounces;
        return bullet;
    }

    public void StartFiring(){
        ammoCount--;
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
        ammoWidget.Refresh(ammoCount);
        recoil.Reset();
       
    }

    public void UpdateFiring(float deltaTime){
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;

        while(accumulatedTime >= 0.0f){
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime){
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime){
        bullets.ForEach(bullet => {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1,bullet);
        });
    }

    void DestroyBullets(){
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet){
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if(Physics.Raycast(ray, out hitInfo, distance)){
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
            
            if(bullet.bounce > 0){  
                bullet.time = 0;
                bullet.initialPosition = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            }


            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if(rb2d){
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }

            var hitbox = hitInfo.collider.GetComponent<HitBox>();
            if(hitbox){
                hitbox.OnRaycastHit(this, ray.direction);
            }


        }else{
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet(){

        if(ammoCount <= 0){
            return;
        }

        
        foreach(var particle in muzzleFlash){
            particle.Emit(1);
        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        recoil.GenerateRecoil(weaponName);
      
    }

    public void StopFiring(){
        isFiring = false;
    }
}
