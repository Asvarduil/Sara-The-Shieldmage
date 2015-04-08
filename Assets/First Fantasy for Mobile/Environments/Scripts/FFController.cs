// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFController class.
* 
* 	This class handles
* 		- Switch Camera and Player control
* 		- Init Render Setting
* 		- Display GUIs
* 
***************/

public class FFController : MonoBehaviour
{

#region Variables
	
	public Material m_SkyBoxMaterial = null;
	
	public GameObject m_FirstPerson = null;
	public GameObject m_OrbitCamera = null;
	
#endregion {Variables}
	
// ######################################################################
// MonoBehaviour Functions
// ######################################################################

#region Component Segments

	// Use this for initialization
	void Start ()
	{
		InitCamera();
		InitRenderSetting();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_FirstPerson!=null)
		{
			if(Input.GetKeyUp(KeyCode.C))
			{
				SwitchCamera();
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{		
		// Reset player position when user move it away from terrain
		this.transform.localPosition = new Vector3(0,1,0);
    }
	
	// OnGUI is called for rendering and handling GUI events.
	void OnGUI () {
		
		// Show version number
		GUI.Window(1, new Rect((Screen.width-220), 5, 210, 80), InfoWindow, "Info");
		
		// Show Help GUI window
		GUI.Window(2, new Rect((Screen.width-220), Screen.height-130, 210, 125), HelpWindow, "Help");
		
		// Show Scenes window
		GUI.Window(3, new Rect(5, Screen.height-110, 340, 105), DemoScenesWindow, "Demo Scenes");
		
	}
	
#endregion Component Segments
	
// ######################################################################
// Functions Functions
// ######################################################################

#region Functions
	
	void InitCamera()
	{
		if(m_FirstPerson!=null)
		{
			m_FirstPerson.SetActive(false);
		}
		m_OrbitCamera.SetActive(true);
	}
	
	void InitRenderSetting()
	{
		if(Application.loadedLevelName=="9) Arctic")
		{
			RenderSettings.skybox = m_SkyBoxMaterial;
		}
	}

	void SwitchCamera()
	{
		if(m_FirstPerson!=null)
		{
			m_FirstPerson.SetActive(!m_FirstPerson.activeSelf);
		}
		m_OrbitCamera.SetActive(!m_OrbitCamera.activeSelf);
		
		if(m_OrbitCamera.activeSelf==true)
		{
			FFOrbitCamera pFFOrbitCamera = (FFOrbitCamera) Object.FindObjectOfType(typeof(FFOrbitCamera));
			pFFOrbitCamera.TargetLookAt.transform.localPosition = new Vector3(0,0,0);
		}
	}

	// Show Help window
	void HelpWindow(int id)
	{
		if(m_FirstPerson!=null)
		{
			if(m_FirstPerson.activeSelf==true)
			{
				GUI.Label(new Rect(12, 25, 240, 20), "W/S/A/D: Move player");
				GUI.Label(new Rect(12, 50, 240, 20), "Mouse: Look around");
				if(m_OrbitCamera!=null)
				{
					GUI.Label(new Rect(12, 75, 240, 20), "C: Switch to Orbit Camera");
				}
			}
			else if(m_OrbitCamera.activeSelf==true)
			{
				GUI.Label(new Rect(12, 25, 240, 20), "Mouse drags: Orbit");
				GUI.Label(new Rect(12, 50, 240, 20), "Mouse wheel: Zoom");
				if(m_FirstPerson!=null)
				{
					GUI.Label(new Rect(12, 75, 240, 20), "C: Switch to First Person Camera");
				}
			}
		}
		else if(m_OrbitCamera!=null)
		{
			GUI.Label(new Rect(12, 25, 240, 20), "Mouse drags: Orbit");
			GUI.Label(new Rect(12, 50, 240, 20), "Mouse wheel: Zoom");
			if(m_FirstPerson!=null)
			{
				GUI.Label(new Rect(12, 75, 240, 20), "E: Switch to First Person Camera");
			}
		}
		
	}

	// Show Demo Scenes window
	void DemoScenesWindow(int id)
	{
		string[] DemoScenesName = {"1) Forest", "2) Grassland", "3) Ruin", "4) Wasteland", "5) Lagoon", "6) Cave", "7) Crater", "8) Particle Test", "9) Arctic"};
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.alignment = TextAnchor.MiddleLeft;
		int columnCount = 0;
		for(int i=0;i<DemoScenesName.Length;i++)
		{
			// Disable current scene load button
			if(Application.loadedLevelName == DemoScenesName[i])
			{
				GUI.enabled=false;
			}
			if(GUI.Button(new Rect(10 + (110*columnCount), 25*((i/3)+1), 100, 20), DemoScenesName[i], buttonStyle))
			{
				Application.LoadLevel(DemoScenesName[i]);
			}
			if(GUI.enabled==false)
			{
				GUI.enabled=true;
			}

			// reset column
			columnCount++;
			if(columnCount==3)
			{
				columnCount = 0;
			}
		}
	}

	// Show Info window
	void InfoWindow(int id)
	{
		GUI.Label(new Rect(15, 25, 240, 20), "First Fantasy for Mobile 1.3.2");
		GUI.Label(new Rect(15, 50, 240, 20), "www.ge-team.com/pages");
	}

#endregion Functions
	
}
