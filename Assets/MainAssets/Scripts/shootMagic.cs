namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	
	public class shootMagic : IAction
	{
        //bullet 
        public GameObject bullet;
        public float manaCost = 1;


        //Gun stats
        public float spread;

        //Reference
        public Camera fpsCam;
        public Transform attackPoint;

        private ManaBar manaBar;

        private void Awake()
        {
            manaBar = GameObject.Find("ManaBar").GetComponent<ManaBar>();
        }

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Shoot();
            manaBar.usingMana = true;
            manaBar.UseMana(manaCost);
			return true;
        }
        private void Shoot()
    {

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;
        var layerMask = 1 << 3 | 1 << 7;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, ~layerMask))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player
            

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);




    }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/ShootMagic";
		#endif
	}
}
