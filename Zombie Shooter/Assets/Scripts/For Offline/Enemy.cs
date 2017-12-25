using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity
{

    public enum State { Idle, Chasing, Attacking };
    State currentState;

	public ParticleSystem deathEffect;
    public UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColour;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    public float damage = 10;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;


    bool hasTarget;
    // Use this for initialization

	void Awake() {
		pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
		}
	}
    protected override void Start()
    {
        base.Start();
		if (hasTarget) {
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
    }
	public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		if (damage >= health) {

			//Destroy(Instantiate (deathEffect.gameObject,hitPoint,Quaternion.FromToRotation (Vector3.back,hitDirection))as GameObject,deathEffect.startLifetime);
			Destroy(Instantiate (deathEffect.gameObject,hitPoint,Quaternion.FromToRotation (Vector3.forward,hitDirection))as GameObject,deathEffect.startLifetime);
		}
		base.TakeHit (damage, hitPoint, hitDirection);
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour) {
		pathfinder.speed = moveSpeed;
		if (hasTarget) {
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;

		skinMaterial = GetComponent<Renderer> ().sharedMaterial;
		skinMaterial.color = skinColour;
		originalColour = skinMaterial.color;
	}
    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                /* we can use the Vector3.Distance but it will calculate the sqrRot for bothe vector3 values 
                thats why we are just geting the distance and then perform the sqrRoot*/
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))//colider radius added for measure the distance from edge of the Player and Enemy not from the center of them.
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }

            }
        }
    }
    IEnumerator Attack()
    {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0; //percent of animation

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            //this only for the animation 
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4; //This interpolation value only for goto 0 to 1 and back to 0
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);//if value is 0 then originalpos or if value 1 the attackpos

            yield return null;
        }

        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);//to stop just before player not collide 
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}

