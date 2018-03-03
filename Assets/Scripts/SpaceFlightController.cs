using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFlightController : MonoBehaviour {

	private Rigidbody rb;

	//Speed multiplier for translational motion in 6 directions (so that damage states can effect each one separately)
	private float speedLeft = 10000.0f;
	private float speedRight = 10000.0f;
	private float speedUp = 10000.0f;
	private float speedDown = 10000.0f;
	private float speedForward = 50000.0f;
	private float speedBackward = 25000.0f;

	//Rate multiplier for rotational motion in 3 directions
	private float pitchRate = 2000.0f;
	private float yawRate = 2000.0f;
	private float rollRate = 2000.0f;

	//Inertial dampeners, which can be broken to create instances of uncontrolled rotation
	private float inertialTranslationDampening = 0.5f;
	private float inertialRotationDampening = 3.0f;

	public float inputHorizontal = 0;
	public float inputVertical = 0;
	public float inputForeback = 0;
	public float inputRoll = 0;
	public float inputPitch = 0;
	public float inputYaw = 0;

	public void SetHorizontal(float value) {
		inputHorizontal = value;
	}

	public void SetVertical(float value) {
		inputVertical = value;
	}

	public void SetForeback(float value) {
		inputForeback = value;
	}

	public void SetRoll(float value) {
		inputRoll = value;
	}

	public void SetPitch(float value) {
		inputPitch = value;
	}

	public void SetYaw(float value) {
		inputYaw = value;
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.drag = inertialTranslationDampening;
		rb.angularDrag = inertialRotationDampening;
	}
	
	void Update() {

	}

	void FixedUpdate () {
		HandleTranslationInput();
		HandleRotationInput();
	}

	void HandleTranslationInput() {
		if(inputHorizontal != 0 || inputVertical != 0 || inputForeback != 0) {
			if(rb.drag != 0) {
				rb.drag = 0;
			}
		}
		else {
			if(rb.drag != inertialTranslationDampening) {
				rb.drag = inertialTranslationDampening;
			}
		}

		LateralTranslation(inputHorizontal);
		ForwardBackwardTranslation(inputVertical);
		VerticalTranslation(inputForeback);
	}

	void ForwardBackwardTranslation(float motionInput) {
		if(motionInput != 0) {

			if(motionInput > 0) {
				rb.AddForce(transform.forward * speedForward);
			}
			else {
				rb.AddForce(-transform.forward * speedBackward);
			}
		}
	}

	void LateralTranslation(float motionInput) {
		if(motionInput != 0) {
			if(motionInput > 0) {
				rb.AddForce(transform.right * speedRight);
			}
			else {
				rb.AddForce(-transform.right * speedLeft);
			}
		}
	}

	void VerticalTranslation(float motionInput) {
		if(motionInput != 0) {
			if(motionInput > 0) {
				rb.AddForce(transform.up * speedUp);
			}
			else {
				rb.AddForce(-transform.up * speedDown);
			}
		}
	}

	void HandleRotationInput() {
		if(inputRoll != 0 || inputPitch != 0 || inputYaw != 0) {
			if(rb.angularDrag != 0) {
				rb.angularDrag = 0;
			}
		}
		else {
			if(rb.angularDrag != inertialRotationDampening) {
				rb.angularDrag = inertialRotationDampening;
			}
		}

		RollRotation(inputRoll);
		PitchRotation(inputPitch);
		YawRotation(inputYaw);
	}

	void RollRotation(float motionInput) {
		if(motionInput != 0) {
			if(motionInput > 0) {
				rb.AddTorque(-transform.forward*rollRate);
			}
			else {
				rb.AddTorque(transform.forward*rollRate);
			}
		}
	}

	void PitchRotation(float motionInput) {
		if(motionInput != 0) {
			if(motionInput > 0) {
				rb.AddTorque(transform.right*pitchRate);
			}
			else {
				rb.AddTorque(-transform.right*pitchRate);
			}
		}
	}

	void YawRotation(float motionInput) {
		if(motionInput != 0) {
			if(motionInput > 0) {
				rb.AddTorque(transform.up*yawRate);
			}
			else {
				rb.AddTorque(-transform.up*yawRate);
			}
		}
	}

	public void SetInertialTranslationDampening(float value) {
		inertialTranslationDampening = value;
	}

	public void SetInertialRotationDampening(float value) {
		inertialRotationDampening = value;
	}

}
