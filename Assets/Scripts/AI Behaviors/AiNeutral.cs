using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType{
    PATH_TO_TARGET,
	FOLLOW_TARGET,
    ORBIT_TARGET,
    SLOW_ROTATE,
    ATTACK_TARGET,
    DEFEND_SELF,
    RUN_FROM_TARGET,
	CUT_ALL_ENGINES
 }

public class AiNeutral : MonoBehaviour {

	private SpaceFlightController flightControl;
	// public WeaponsManager weaponsManager;
	// public DamageManager damageManager;
	public ScannerManager scannerManager;
	// public Vector3 pointOfInterest;
	private Vector3 travelVector = Vector3.zero;
	private Rigidbody rb;
	public GameObject target;
	public List<GameObject> thingsToAvoid;
	public float maxRadV_path;
	public float maxRadV_orbit;
	public float minimumProximity = 10.0f;
	private float avoidanceMagnitude = 0.0f;
	public ActionType currentAction = ActionType.CUT_ALL_ENGINES;
	public float targetOrbitDistance = 20.0f;
	public float orbitSpeedLimit = 10.0f;
	public float avoidanceRange = 20.0f;

	private float angle_x;
	private float angle_y;

	void OnDrawGizmosSelected() {
		if(currentAction == ActionType.PATH_TO_TARGET || currentAction == ActionType.FOLLOW_TARGET) {
			// Step 1. Find Travel Vector (in world space)
			
			Gizmos.color = Color.blue;
			Vector3 towardTarget = target.transform.position - transform.position;
			Vector3 avoidObsticals = CalculateAvoidanceVector();
			travelVector = towardTarget + avoidObsticals;

			// Step 2. Project the Travel Vector onto the local horizontal plane

			Vector3 travelVector_localHorizontal = Vector3.ProjectOnPlane(travelVector, transform.up);

			// Step 3. Find Angle on local Horizontal Plane between my reference direction and the travel Vector projection

			angle_x = Vector3.SignedAngle(transform.forward, travelVector_localHorizontal, transform.up);


			// Step 4. Project Travel ector onto the local azumith plane

			Vector3 travelVector_localAzumith = Vector3.ProjectOnPlane(travelVector, transform.right);

			// Step 5. Find Angle on local Azumith Plane between my reference direction and the travel Vector projection

			angle_y = Vector3.SignedAngle(transform.forward, travelVector_localAzumith, transform.right);
			if(angle_y > 90) {
				angle_y = 180 - angle_y;
			}
			else if(angle_y < -90) {
				angle_y = -180 - angle_y;
			}


			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + travelVector);
		}
		else if(currentAction == ActionType.ORBIT_TARGET) {
			//Step 1: Find vector from position to target, and then from position to a point a distance from the target on the same line
			//this will guide the ship toward the orbital path (distance)
			float goalDistance = 20.0f;
			Vector3 targetPoint = target.transform.position + goalDistance*Vector3.Normalize(transform.position - target.transform.position);

			//Step 2: Find vector orthogonal the vector between position to target and the local up.
			Vector3 towardTarget = (target.transform.position - transform.position);

			float towardTargetPointWeight = Vector3.Distance(targetPoint, transform.position);
			Vector3 towardTargetPoint = towardTargetPointWeight*Vector3.Normalize(targetPoint - transform.position);

			if(towardTargetPointWeight < 1.0f) {
				towardTargetPointWeight = 1.0f;
			}

			Vector3 perpendicularToTarget = (20.0f/towardTargetPointWeight)*Vector3.Normalize(Vector3.ProjectOnPlane(transform.forward, target.transform.position - transform.position));  

			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + towardTargetPoint);

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + perpendicularToTarget);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + towardTargetPoint + perpendicularToTarget + CalculateAvoidanceVector());
		}
    }

	// Use this for initialization
	void Start () {
		flightControl = transform.GetComponent<SpaceFlightController>();
		// weaponsManager = transform.GetComponent<WeaponsManager>();
		// damageManager = transform.GetComponent<DamageManager>();
		scannerManager = transform.GetComponent<ScannerManager>();

		rb = transform.GetComponent<Rigidbody>();
	}
	
	// void Update() {
	// 	if(damageManager.health < damageManager.maxHealth && weaponsManager.target == null) {
	// 		weaponsManager.SetTarget(target);
	// 		weaponsManager.SetInputFire(1);
	// 	}
	// }

	void Update() {
		thingsToAvoid = scannerManager.GetTrackedObjects();
	}

	void FixedUpdate() {
		switch(currentAction) {
			case ActionType.PATH_TO_TARGET:
				TravelTowardPointBehavior(true);
				break;
			case ActionType.FOLLOW_TARGET:
				TravelTowardPointBehavior(false);
				break;
			case ActionType.ORBIT_TARGET:
				OrbitPointAtDistanceBehavior(20.0f);
				break;
			case ActionType.SLOW_ROTATE:
				break;
			case ActionType.ATTACK_TARGET:
				break;
			case ActionType.DEFEND_SELF:
				break;
			case ActionType.RUN_FROM_TARGET:
				break;
			case ActionType.CUT_ALL_ENGINES:
				CutAllEngines();
				break;
			default:
				CutAllEngines();
				break;
		}
	}

	void CutAllEngines() {
		if(flightControl.inputVertical != 0 || flightControl.inputHorizontal != 0 || flightControl.inputForeback != 0 || flightControl.inputRoll != 0 || flightControl.inputPitch != 0 || flightControl.inputYaw != 0) {
			flightControl.SetVertical(0);
			flightControl.SetHorizontal(0);
			flightControl.SetForeback(0);
			flightControl.SetRoll(0);
			flightControl.SetPitch(0);
			flightControl.SetYaw(0);
		}
	}

	void TravelTowardPointBehavior(bool stopOnArrival) {

		//If we've arrived at the target, cut all engines
		if(Vector3.Distance(transform.position, target.transform.position) < minimumProximity) {
			travelVector = Vector3.zero;
			angle_x = 0;
			angle_y = 0;
			if(stopOnArrival) { 
				currentAction = ActionType.CUT_ALL_ENGINES;
			}
			else {
				flightControl.SetYaw(0);
				flightControl.SetPitch(0);
			}
			return;
		}

		//If we haven't yet arrived at the target, move as required to get there

		/* UPDATE THE TRAVEL VECTOR (START) */

		Vector3 towardTarget = target.transform.position - transform.position;
		Vector3 avoidObsticals = CalculateAvoidanceVector();
		travelVector = towardTarget + avoidObsticals;

		/* TURN TOWARD TRAVEL VECTOR (START) */
		Vector3 travelVector_localHorizontal = Vector3.ProjectOnPlane(travelVector, transform.up);
		Vector3 travelVector_localAzumith = Vector3.ProjectOnPlane(travelVector, transform.right);

		angle_x = Vector3.SignedAngle(transform.forward, travelVector_localHorizontal, transform.up);
		angle_y = Vector3.SignedAngle(transform.forward, travelVector_localAzumith, transform.right);
		if(angle_y > 90) {
			angle_y = 180 - angle_y;
		}
		else if(angle_y < -90) {
			angle_y = -180 - angle_y;
		}

		//Turn if we're not yet aligned with the travel vector (within accceptancce margin)
		if(angle_x > 5 || angle_x < -5 || angle_y > 5 || angle_y < -5) {//our angle isn't within 5 deg of the target vector yet) {

			if(rb.angularVelocity.magnitude > maxRadV_path) {
				flightControl.SetYaw(0);
				flightControl.SetPitch(0);
			}
			else {
				if(angle_x < 5.0f) {
					flightControl.SetYaw(-1);
				}
				else if(angle_x > 5.0f) {
					flightControl.SetYaw(1);
				}
				else {
					flightControl.SetYaw(0);
				}

				if(angle_y < -5.0f) {
					flightControl.SetPitch(-1);
				}
				else if(angle_y > 5.0f) {
					flightControl.SetPitch(1);
				}
				else {
					flightControl.SetPitch(0);
				}
			}

		}
		else {
			flightControl.SetPitch(0);
			flightControl.SetYaw(0);
		}

		/* MOVE ALONG TRAVEL VECTOR (START) */

		if(Vector3.Distance(transform.position, target.transform.position) > minimumProximity && rb.angularVelocity.magnitude < 0.01) {
			if(flightControl.inputVertical <= 0) {
				MoveForward();
			}
		}
		else {
			if(flightControl.inputVertical != 0) {
				StopForwardMotion();
			}
		}
	}

	void OrbitPointAtDistanceBehavior(float goalDistance) {
		Vector3 targetPoint = target.transform.position + targetOrbitDistance*Vector3.Normalize(transform.position - target.transform.position);
		float towardTargetPointWeight = Vector3.Distance(targetPoint, transform.position);
		Vector3 towardTargetPoint = towardTargetPointWeight*Vector3.Normalize(targetPoint - transform.position);

		if(towardTargetPointWeight < 1.0f) {
			towardTargetPointWeight = 1.0f;
		}

		Vector3 perpendicularToTarget = (20.0f/towardTargetPointWeight)*Vector3.Normalize(Vector3.ProjectOnPlane(transform.forward, target.transform.position - transform.position));  

		travelVector = Vector3.Normalize(towardTargetPoint + perpendicularToTarget + CalculateAvoidanceVector());

		Vector3 travelVector_localHorizontal = Vector3.ProjectOnPlane(travelVector, transform.up);
		Vector3 travelVector_localAzumith = Vector3.ProjectOnPlane(travelVector, transform.right);

		angle_x = Vector3.SignedAngle(transform.forward, travelVector_localHorizontal, transform.up);
		angle_y = Vector3.SignedAngle(transform.forward, travelVector_localAzumith, transform.right);
		if(angle_y > 90) {
			angle_y = 180 - angle_y;
		}
		else if(angle_y < -90) {
			angle_y = -180 - angle_y;
		}

		if(angle_x > 5 || angle_x < -5 || angle_y > 5 || angle_y < -5) {//our angle isn't within 5 deg of the target vector yet) {

			if(rb.angularVelocity.magnitude > maxRadV_orbit) {
				flightControl.SetYaw(0);
				flightControl.SetPitch(0);
			}
			else {
				if(angle_x < 5.0f) {
					flightControl.SetYaw(-1);
				}
				else if(angle_x > 5.0f) {
					flightControl.SetYaw(1);
				}
				else {
					flightControl.SetYaw(0);
				}

				if(angle_y < -5.0f) {
					flightControl.SetPitch(-1);
				}
				else if(angle_y > 5.0f) {
					flightControl.SetPitch(1);
				}
				else {
					flightControl.SetPitch(0);
				}
			}

		}
		else {
			flightControl.SetPitch(0);
			flightControl.SetYaw(0);
		}

		if(flightControl.inputVertical <= 0) {
			if(Vector3.Distance(transform.position, target.transform.position) < targetOrbitDistance && rb.velocity.magnitude > orbitSpeedLimit) {
				StopForwardMotion();
			}
			else {
				if(rb.angularVelocity.magnitude < maxRadV_orbit) {
					MoveForward();
				}
			}
		}
		else {
			StopForwardMotion();
		}
	}

	Vector3 CalculateAvoidanceVector() {
		Vector3 avoidanceVector = Vector3.zero;
		for(int i = 0; i < thingsToAvoid.Count; i++) {
			avoidanceMagnitude = Mathf.Pow(50/Vector3.Distance(transform.position, thingsToAvoid[i].transform.position), 2);
			avoidanceVector += avoidanceMagnitude*Vector3.Normalize(transform.position - thingsToAvoid[i].transform.position);
		}
		return(avoidanceVector);
	}

	// void TranslationalCollisionAvoidance() {
	// 	Vector3 avoidObsticals = CalculateAvoidanceVector();

	// 	if(Vector3.Distance(transform.position, thingToAvoid.transform.position) < 30.0f) {
	// 		flightControl.SetHorizontal(avoidObsticals.x);
	// 		flightControl.SetForeback(avoidObsticals.y);
	// 	}
	// 	else {
	// 		if(flightControl.inputHorizontal != 0 || flightControl.inputForeback != 0) {
	// 			flightControl.SetHorizontal(0);
	// 			flightControl.SetForeback(0);
	// 		}
	// 	}

	// }

	void MoveForward() {
		flightControl.SetVertical(1);
	}

	void StopForwardMotion() {
		flightControl.SetVertical(0);
	}

}
