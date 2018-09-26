using UnityEngine;
using System.Collections;

namespace IE.Player {

	public class IE_CharacterController : MonoBehaviour {

		public Vector4 v4_WALK_MovementSpeeds;
		public Vector4 v4_RUN_MovementSpeeds;
		public float f_WalkRunTransitionUpSpeed;
		public float f_WalkRunTransitionDownTime;


		private Vector3 v3_MoveDirection = Vector3.zero;

		private Vector4 v4_CurrentSpeeds;

		private CharacterController cc_Player;

		private float f_YMovement;
		private float f_RefWalkRunTransition;


		private Coroutine co_ReturnToWalkSpeed;

		void Start()
		{
			v4_CurrentSpeeds = v4_WALK_MovementSpeeds;
			cc_Player = IE_PlayerGlobal.instance.cc_Player;
		}

		void Update() {

			if(Input.GetKey(KeyCode.LeftShift))
			{
				if(co_ReturnToWalkSpeed != null)
					StopCoroutine(co_ReturnToWalkSpeed);

				v4_CurrentSpeeds = Vector4.Lerp(v4_CurrentSpeeds, v4_RUN_MovementSpeeds, Time.deltaTime * f_WalkRunTransitionUpSpeed);

			}
			if(Input.GetKeyUp (KeyCode.LeftShift))
				co_ReturnToWalkSpeed = StartCoroutine(IEReturnToWalkSpeed());


			v3_MoveDirection = new Vector3(Input.GetAxis("Horizontal") * v4_CurrentSpeeds.x, f_YMovement, Input.GetAxis("Vertical") * v4_CurrentSpeeds.z);

			if (cc_Player.isGrounded) {
				if (Input.GetButton("Jump"))
					f_YMovement = v4_CurrentSpeeds.y;
			}
			else
				f_YMovement -= v4_CurrentSpeeds.w * Time.deltaTime;

			v3_MoveDirection = transform.TransformDirection(v3_MoveDirection);
			cc_Player.Move(v3_MoveDirection* Time.deltaTime);

		}

		IEnumerator IEReturnToWalkSpeed()
		{
			float i = 0.0f;
			float rate = 1.0f / f_WalkRunTransitionDownTime;
			Vector4 v4_CurrentSpeed = v4_CurrentSpeeds;

			while(i < 1.0f)
			{
				i += Time.deltaTime * rate;
				v4_CurrentSpeeds = Vector4.Lerp(v4_CurrentSpeed, v4_WALK_MovementSpeeds, i);
				yield return null;
			}
		}
}

}

