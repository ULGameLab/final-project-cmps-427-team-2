namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	
	public class shootMagic : IAction
	{
		public Camera cam;
		public Transform leftHand;
		public float projectileSpeed = 30;

		private Vector3 destination;


		public GameObject projectile;
		
		public int example = 0;

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			float screenX = Screen.width / 2;
			float screenY = Screen.height / 2;
			Ray ray = cam.ViewportPointToRay(new Vector3(screenX, screenY, 0));
			RaycastHit HitInfo;
			//check for a hit
			if (Physics.Raycast(ray, out HitInfo))
			{
				destination = HitInfo.point;
			}

            else
            {
				destination = ray.GetPoint(1000);
            }

          
			var projectileObj = Instantiate(projectile, leftHand.position, Quaternion.identity) as GameObject;

			projectileObj.GetComponent<Rigidbody>().velocity = ((destination-leftHand.position).normalized * projectileSpeed);
	

			return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/shootMagic";
		#endif
	}
}
