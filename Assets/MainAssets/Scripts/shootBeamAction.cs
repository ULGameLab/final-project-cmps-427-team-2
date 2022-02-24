namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class shootBeamAction : IAction
	{
        public Camera cam;
        public Transform leftHand;

        private Vector3 destination;

        [Header("Prefabs")]
        public GameObject[] beamLineRendererPrefab;
        public GameObject[] beamStartPrefab;
        public GameObject[] beamEndPrefab;

        private int currentBeam = 0;

        private GameObject beamStart;
        private GameObject beamEnd;
        private GameObject beam;
        private LineRenderer line;

        [Header("Adjustable Variables")]
        public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
        public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
        public float textureLengthScale = 3; //Length of the beam texture
        public int example = 0;


        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            beamStart = Instantiate(beamStartPrefab[currentBeam], leftHand.position, Quaternion.identity) as GameObject;
            beamEnd = Instantiate(beamEndPrefab[currentBeam], leftHand.position, Quaternion.identity) as GameObject;
            beam = Instantiate(beamLineRendererPrefab[currentBeam], leftHand.position, Quaternion.identity) as GameObject;
            line = beam.GetComponent<LineRenderer>();

            float screenX = Screen.width / 2;
            float screenY = Screen.height / 2;
            Ray ray = cam.ViewportPointToRay(new Vector3(screenX, screenY, 0));
            Debug.DrawRay(ray.origin, ray.direction);
            RaycastHit hit;
            //check for a hit
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Vector3 tdir = hit.point - transform.position;
                ShootBeamInDir(leftHand.position, tdir);
            }

            return true;
        }

        void ShootBeamInDir(Vector3 start, Vector3 dir)
        {
            line.positionCount = 2;
            line.SetPosition(0, start);
            beamStart.transform.position = start;

            Vector3 end = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(start, dir, out hit))
                end = hit.point - (dir.normalized * beamEndOffset);
            else
                end = transform.position + (dir * 100);

            beamEnd.transform.position = end;
            line.SetPosition(1, end);

            beamStart.transform.LookAt(beamEnd.transform.position);
            beamEnd.transform.LookAt(beamStart.transform.position);

            float distance = Vector3.Distance(start, end);
            line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
            line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
        }

#if UNITY_EDITOR
        public static new string NAME = "Custom/shootBeamAction";
		#endif
	}
}
