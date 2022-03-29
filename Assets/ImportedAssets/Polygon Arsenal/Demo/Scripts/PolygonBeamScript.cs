using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameCreator.Variables;

namespace PolygonArsenal
{

public class PolygonBeamScript : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;
    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;

        

    private int currentBeam = 0;

    public Transform instantiatedBeamPosition;


        int enemyLayer;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    public GameObject Player;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
	public float textureLengthScale = 3; //Length of the beam texture

    [Header("Put Sliders here (Optional)")]
    public Slider endOffSetSlider; //Use UpdateEndOffset function on slider
    public Slider scrollSpeedSlider; //Use UpdateScrollSpeed function on slider

    [Header("Put UI Text object here to show beam name")]
    public Text textBeamName;

    // Use this for initialization
    void Start()
    {
        
        if (textBeamName)
            textBeamName.text = beamLineRendererPrefab[currentBeam].name;
        if (endOffSetSlider)
            endOffSetSlider.value = beamEndOffset;
        if (scrollSpeedSlider)
            scrollSpeedSlider.value = textureScrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
            enemyLayer = 1 << 3;
            enemyLayer = ~enemyLayer;
    
     var isAiming = VariablesManager.GetLocal(Player, "isAiming").ToString();
    var isDodging = VariablesManager.GetLocal(Player, "isDodging").ToString();


            float screenX = Screen.width / 2;
     float screenY = Screen.height / 2;

        if (Input.GetMouseButtonDown(0) && isAiming == "True" && isDodging == "False")
        {
            beamStart = Instantiate(beamStartPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beamEnd = Instantiate(beamEndPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beam = Instantiate(beamLineRendererPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            line = beam.GetComponent<LineRenderer>();
        }
        if (Input.GetMouseButtonUp(0))
        {
                if (beamStart != null && beamEnd != null && beam != null)
                {
                    Destroy(beamStart);
                    Destroy(beamEnd);
                    Destroy(beam);
                }

            }

        if(isDodging == "True")
            {
                if (beamStart != null && beamEnd != null && beam != null)
                {
                    Destroy(beamStart);
                    Destroy(beamEnd);
                    Destroy(beam);
                }
            }


        if (Input.GetMouseButton(0) && isAiming == "True" && isDodging == "False")
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, enemyLayer))
            {
                Vector3 tdir = hit.point - transform.position;
                if(beam != null)
                    {
                        ShootBeamInDir(instantiatedBeamPosition.position, tdir);
                    }
                
            }
                else
                {
                    var position = ray.GetPoint(100f);
                    ShootBeamInDir(instantiatedBeamPosition.position, position);
                }
        }
		
		
		
    }

   
	

    public void UpdateEndOffset()
    {
        beamEndOffset = endOffSetSlider.value;
    }

    public void UpdateScrollSpeed()
    {
        textureScrollSpeed = scrollSpeedSlider.value;
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit, 100f, enemyLayer))
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
}
}
