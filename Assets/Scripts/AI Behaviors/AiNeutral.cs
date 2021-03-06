﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    PATH_TO_TARGET,
    FOLLOW_TARGET,
    ORBIT_TARGET,
    CIRCLE_ORBIT_TARGET,
    SLOW_ROTATE,
    ATTACK_TARGET,
    DEFEND_SELF,
    RUN_FROM_TARGET,
    CUT_ALL_ENGINES
}

public class AiNeutral : MonoBehaviour
{

    private SpaceFlightController flightControl;
    public WeaponsManager weaponsManager;
    public ScannerManager scannerManager;
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

    public int orbitStep = 0;

    public List<Vector3> orbitStepPositions;
    public float avoidanceRange = 20.0f;

    public float safeDistance = 100.0f;

    private float angle_x;
    private float angle_y;

    void OnDrawGizmosSelected()
    {
        if (currentAction == ActionType.PATH_TO_TARGET || currentAction == ActionType.FOLLOW_TARGET)
        {
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
            if (angle_y > 90)
            {
                angle_y = 180 - angle_y;
            }
            else if (angle_y < -90)
            {
                angle_y = -180 - angle_y;
            }


            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + travelVector);
        }
        else if (currentAction == ActionType.ATTACK_TARGET)
        {
            //Step 1: Find vector from position to target, and then from position to a point a distance from the target on the same line
            //this will guide the ship toward the orbital path (distance)
            float goalDistance = 20.0f;
            Vector3 targetPoint = target.transform.position + goalDistance * Vector3.Normalize(transform.position - target.transform.position);

            //Step 2: Find vector orthogonal the vector between position to target and the local up.
            Vector3 towardTarget = (target.transform.position - transform.position);

            float towardTargetPointWeight = Vector3.Distance(targetPoint, transform.position);
            Vector3 towardTargetPoint = towardTargetPointWeight * Vector3.Normalize(targetPoint - transform.position);

            if (towardTargetPointWeight < 1.0f)
            {
                towardTargetPointWeight = 1.0f;
            }

            Vector3 perpendicularToTarget = (20.0f / towardTargetPointWeight) * Vector3.Normalize(Vector3.ProjectOnPlane(transform.forward, target.transform.position - transform.position));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + towardTargetPoint);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + perpendicularToTarget);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + towardTargetPoint + perpendicularToTarget + CalculateAvoidanceVector());
        }
        else if (currentAction == ActionType.ORBIT_TARGET) {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(target.transform.position, Vector3.up, targetOrbitDistance);
        }
        else if (currentAction == ActionType.CIRCLE_ORBIT_TARGET) {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(target.transform.position, Vector3.up, targetOrbitDistance);

            if(orbitStep == 2) {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(targetOrbitDistance, 0, 0), Vector3.up, 2);
            }
            else {
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(targetOrbitDistance, 0, 0), Vector3.up, 2);
            }

            if(orbitStep == 3) {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(0, 0, targetOrbitDistance), Vector3.up, 2);
            }
            else {
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(0, 0, targetOrbitDistance), Vector3.up, 2);
            }

            if(orbitStep == 0) {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(-targetOrbitDistance, 0, 0), Vector3.up, 2);
            }
            else {
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(-targetOrbitDistance, 0, 0), Vector3.up, 2);
            }

            if(orbitStep == 1) {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(0, 0, -targetOrbitDistance), Vector3.up, 2);
            }
            else {
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(target.transform.position - new Vector3(0, 0, -targetOrbitDistance), Vector3.up, 2);
            }
            
        }
    }

    void OnDestroy()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        GameController gameController = null;

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        else
        {
            gameController.ShipDestroyed(this);
        }
    }


    // Use this for initialization
    void Start()
    {
        flightControl = transform.GetComponent<SpaceFlightController>();
        weaponsManager = transform.GetComponent<WeaponsManager>();
        // damageManager = transform.GetComponent<DamageManager>();
        scannerManager = transform.GetComponent<ScannerManager>();

        rb = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        thingsToAvoid = scannerManager.GetTrackedObjects();
    }

    void FixedUpdate()
    {
        switch (currentAction)
        {
            case ActionType.PATH_TO_TARGET:
                TravelTowardPointBehavior(true);
                break;
            case ActionType.FOLLOW_TARGET:
                TravelTowardPointBehavior(false);
                break;
            case ActionType.ORBIT_TARGET:
                OrbitPointAtDistanceBehavior(20.0f);
                break;
            case ActionType.CIRCLE_ORBIT_TARGET:
                CircularOrbitTargetBehavior();
                break;
            case ActionType.SLOW_ROTATE:
                SlowRotateBehavior();
                break;
            case ActionType.ATTACK_TARGET:
                AttackTargetBehavior();
                break;
            case ActionType.RUN_FROM_TARGET:
                RunFromTargetBehavior(false);
                break;
            case ActionType.CUT_ALL_ENGINES:
                CutAllEngines();
                break;
            default:
                CutAllEngines();
                break;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        weaponsManager.SetTarget(target);
        weaponsManager.SetInputFire(1);
    }

    public void SetAction(ActionType newAction)
    {
        currentAction = newAction;
    }

    void RotateTowardVector(Vector3 alignmentVector) {
        Vector3 alignmentVector_localHorizontal = Vector3.ProjectOnPlane(alignmentVector, transform.up);
        Vector3 alignmentVector_localAzumith = Vector3.ProjectOnPlane(alignmentVector, transform.right);

        float angle_x = Vector3.SignedAngle(transform.forward, alignmentVector_localHorizontal, transform.up);
        float angle_y = Vector3.SignedAngle(transform.forward, alignmentVector_localAzumith, transform.right);

        if (angle_y > 90)
        {
            angle_y = 180 - angle_y;
        }
        else if (angle_y < -90)
        {
            angle_y = -180 - angle_y;
        }

        if (angle_x > 5 || angle_x < -5 || angle_y > 5 || angle_y < -5)
        {//our angle isn't within 5 deg of the target vector yet) {

            if (rb.angularVelocity.magnitude > maxRadV_orbit)
            {
                flightControl.SetYaw(0);
                flightControl.SetPitch(0);
            }
            else
            {
                if (angle_x < 5.0f)
                {
                    flightControl.SetYaw(-1);
                }
                else if (angle_x > 5.0f)
                {
                    flightControl.SetYaw(1);
                }
                else
                {
                    flightControl.SetYaw(0);
                }

                if (angle_y < -5.0f)
                {
                    flightControl.SetPitch(-1);
                }
                else if (angle_y > 5.0f)
                {
                    flightControl.SetPitch(1);
                }
                else
                {
                    flightControl.SetPitch(0);
                }
            }

        }
        else
        {
            flightControl.SetPitch(0);
            flightControl.SetYaw(0);
        }
    }

    void CutAllEngines()
    {
        if (flightControl.inputVertical != 0 || flightControl.inputHorizontal != 0 || flightControl.inputForeback != 0 || flightControl.inputRoll != 0 || flightControl.inputPitch != 0 || flightControl.inputYaw != 0)
        {
            flightControl.SetVertical(0);
            flightControl.SetHorizontal(0);
            flightControl.SetForeback(0);
            flightControl.SetRoll(0);
            flightControl.SetPitch(0);
            flightControl.SetYaw(0);
        }
    }

    void AttackTargetBehavior()
    {
        if (target == null && weaponsManager)
        {
            weaponsManager.SetTarget(null);
            weaponsManager.SetInputFire(0);

            SetAction(ActionType.CUT_ALL_ENGINES);
        }
        else
        {
            if (weaponsManager)
            {
                weaponsManager.SetTarget(target);
                weaponsManager.SetInputFire(1);
            }

            if (Random.Range(0.0f, 1.0f) > 0.5f)
            {
                TravelTowardPointBehavior(false);
            }
            else
            {
                OrbitPointAtDistanceBehavior(20.0f);
            }
        }
    }

    void TravelTowardPointBehavior(bool stopOnArrival)
    {

        //If we've arrived at the target, cut all engines
        if (Vector3.Distance(transform.position, target.transform.position) < minimumProximity)
        {
            travelVector = Vector3.zero;
            angle_x = 0;
            angle_y = 0;
            if (stopOnArrival)
            {
                currentAction = ActionType.CUT_ALL_ENGINES;
            }
            else
            {
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

        RotateTowardVector(travelVector);

        /* MOVE ALONG TRAVEL VECTOR (START) */

        if (Vector3.Distance(transform.position, target.transform.position) > minimumProximity && rb.angularVelocity.magnitude < 0.01)
        {
            if (flightControl.inputVertical <= 0)
            {
                MoveForward();
            }
        }
        else
        {
            if (flightControl.inputVertical != 0)
            {
                StopForwardMotion();
            }
        }
    }

    void RunFromTargetBehavior(bool stopWhenSafe)
    {

        //If we're far enough away from the target, power down engines
        if (Vector3.Distance(transform.position, target.transform.position) > safeDistance)
        {
            travelVector = Vector3.zero;
            angle_x = 0;
            angle_y = 0;
            if (stopWhenSafe)
            {
                currentAction = ActionType.CUT_ALL_ENGINES;
            }
            else
            {
                StopForwardMotion();
                flightControl.SetYaw(0);
                flightControl.SetPitch(0);
            }
            return;
        }

        //If we're still too close to the target, try to get away from it

        /* UPDATE THE TRAVEL VECTOR (START) */

        Vector3 awayFromTarget = transform.position - target.transform.position;
        Vector3 avoidObsticals = CalculateAvoidanceVector();
        travelVector = awayFromTarget + avoidObsticals;

        /* TURN TOWARD TRAVEL VECTOR (START) */
        RotateTowardVector(travelVector);

        /* MOVE ALONG TRAVEL VECTOR (START) */

        if (Vector3.Distance(transform.position, target.transform.position) > minimumProximity && rb.angularVelocity.magnitude < 0.01)
        {
            if (flightControl.inputVertical <= 0)
            {
                MoveForward();
            }
        }
        else
        {
            if (flightControl.inputVertical != 0)
            {
                StopForwardMotion();
            }
        }
    }

    void OrbitPointAtDistanceBehavior(float goalDistance)
    {
        Vector3 targetPoint = target.transform.position + targetOrbitDistance * Vector3.Normalize(transform.position - target.transform.position);
        float towardTargetPointWeight = Vector3.Distance(targetPoint, transform.position);
        Vector3 towardTargetPoint = towardTargetPointWeight * Vector3.Normalize(targetPoint - transform.position);

        if (towardTargetPointWeight < 1.0f)
        {
            towardTargetPointWeight = 1.0f;
        }

        Vector3 perpendicularToTarget = (20.0f / towardTargetPointWeight) * Vector3.Normalize(Vector3.ProjectOnPlane(transform.forward, target.transform.position - transform.position));

        travelVector = Vector3.Normalize(towardTargetPoint + perpendicularToTarget + CalculateAvoidanceVector());

        RotateTowardVector(travelVector);

        if (flightControl.inputVertical <= 0)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < targetOrbitDistance && rb.velocity.magnitude > orbitSpeedLimit)
            {
                StopForwardMotion();
            }
            else
            {
                if (rb.angularVelocity.magnitude < maxRadV_orbit)
                {
                    MoveForward();
                }
            }
        }
        else
        {
            StopForwardMotion();
        }
    }

    void CircularOrbitTargetBehavior() {
        //Set up a set of 4 waypoints around our target object, each one equidistant and at the target distance
        //TODO: Refator this for code efficiency and scalability (preferably a map or hash of standard positions)
        if(orbitStepPositions.Count == 0) {
            orbitStep = 0;
            orbitStepPositions.Add(target.transform.position + new Vector3(targetOrbitDistance, 0, 0));
            orbitStepPositions.Add(target.transform.position + new Vector3(0, 0, targetOrbitDistance));
            orbitStepPositions.Add(target.transform.position + new Vector3(-targetOrbitDistance, 0, 0));
            orbitStepPositions.Add(target.transform.position + new Vector3(0, 0, -targetOrbitDistance));
        }

        //then move from point to point around the object

        float distanceToOrbitStep = Vector3.Distance(transform.position, orbitStepPositions[orbitStep]);

        if(distanceToOrbitStep < 5) {       //If we're close enough to the current orbit step point, target the next one
            if(orbitStep < orbitStepPositions.Count - 1) {
                orbitStep++;
            }
            else {
                orbitStep = 0;
            }
        }
        else {      //If we're not at the current orbit step point yet, get there.
            travelVector = Vector3.Normalize(orbitStepPositions[orbitStep] - transform.position + CalculateAvoidanceVector());
        }
        //generalize this algorithm and add more points to better approximate a circle

        RotateTowardVector(travelVector);

        if (flightControl.inputVertical <= 0)
        {
            //If we're close to the orbit and we're travelling too quikly, stop the engines
            if (Vector3.Distance(transform.position, target.transform.position) < 1.5f*targetOrbitDistance && rb.velocity.magnitude > orbitSpeedLimit)
            {
                StopForwardMotion();
            }
            else
            {
                if (rb.angularVelocity.magnitude < maxRadV_orbit)
                {
                    MoveForward();
                }
            }
        }
        else
        {
            StopForwardMotion();
        }
    }

    void SlowRotateBehavior()
    {
        if (flightControl.inputVertical != 0)
        {
            StopForwardMotion();
        }

        if (rb.angularVelocity.magnitude > 0.1f)
        {
            flightControl.SetYaw(0);
        }
        else
        {
            flightControl.SetYaw(1);
        }
    }

    Vector3 CalculateAvoidanceVector()
    {
        Vector3 avoidanceVector = Vector3.zero;
        for (int i = 0; i < thingsToAvoid.Count; i++)
        {
            if (thingsToAvoid[i] != null)
            {
                avoidanceMagnitude = Mathf.Pow(50 / Vector3.Distance(transform.position, thingsToAvoid[i].transform.position), 2);
                avoidanceVector += avoidanceMagnitude * Vector3.Normalize(transform.position - thingsToAvoid[i].transform.position);
            }
        }
        return (avoidanceVector);
    }

    void MoveForward()
    {
        flightControl.SetVertical(1);
    }

    void StopForwardMotion()
    {
        flightControl.SetVertical(0);
    }

}
