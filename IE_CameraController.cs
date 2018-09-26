using UnityEngine;
using System.Collections;

namespace Player {

	public class IE_CameraController : MonoBehaviour {


		[Header("Input Settings")]
		public Vector2 v2_Sensitivity;
		public Vector2 v2_Smooth;


		[Header("Control Settings")]
		[Range(-90,90)] public float f_MinY;
		[Range(-90,90)] public float f_MaxY;

		private float f_RefX;
		private float f_RefY;
		private float f_SmoothX;
		private float f_SmoothY;
		private Vector2 v2_Inputs;
		private Quaternion q_C;
		private Transform t_ThisTransform;

		void Start()
		{
			t_ThisTransform = transform;
			q_C = t_ThisTransform.rotation;
		}

		void Update()
		{
			v2_Inputs.x += Input.GetAxis("Mouse X") * v2_Sensitivity.x;
			v2_Inputs.y += -Input.GetAxis("Mouse Y") * v2_Sensitivity.y;


			f_SmoothX = Mathf.SmoothDamp(f_SmoothX, v2_Inputs.x, ref f_RefX, v2_Smooth.x);
			f_SmoothY = Mathf.SmoothDamp(f_SmoothY, v2_Inputs.y, ref f_RefY, v2_Smooth.y);

			v2_Inputs.y = Mathf.Clamp(v2_Inputs.y, f_MinY, f_MaxY);

			Quaternion q_X = Quaternion.AngleAxis(f_SmoothX, Vector3.up);
			Quaternion q_Y = Quaternion.AngleAxis(f_SmoothY, Vector3.right);

			t_ThisTransform.rotation = q_C * q_X * q_Y;
		}




	}
}

