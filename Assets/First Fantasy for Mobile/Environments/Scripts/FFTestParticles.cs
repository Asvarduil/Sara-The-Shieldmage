// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFTestParticles class
* This class handles user key inputs, instantiate and destroy the particle effects.
* 
* User presses Up/Down key to switch Element; Fire, Water, Wind, Earth, Light, Darkness.
* Left/Right key to switch particle in current Element.
**************/

public class FFTestParticles : MonoBehaviour {
	
	#region Variables
	
		// Prefabs of particles for each element
		public GameObject[] m_PrefabListFire;
		public GameObject[] m_PrefabListWater;
		public GameObject[] m_PrefabListWind;
		public GameObject[] m_PrefabListEarth;
		public GameObject[] m_PrefabListLight;
		public GameObject[] m_PrefabListDarkness;
		
		// Current element index
		int m_CurrentElementIndex = -1;
	
		// Current particle index
		int m_CurrentParticleIndex = -1;	
		
		// Name of current element
		string m_ElementName = "";	
	
		// Name of particle
		string m_ParticleName = "";
		
		// Current particle list
		GameObject[] m_CurrentElementList = null;
	
		// GameObject of current particle that is showing in the scene
		GameObject m_CurrentParticle = null;
	
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
		
		// Use this for initialization
		void Start () {
	
			// Check if there is any particle in prefab list
			if(m_PrefabListFire.Length>0 ||
				m_PrefabListWater.Length>0 ||
				m_PrefabListWind.Length>0 ||
				m_PrefabListEarth.Length>0 ||
				m_PrefabListLight.Length>0 ||
				m_PrefabListDarkness.Length>0)
			{
				// reset indices of element and particle
				m_CurrentElementIndex = 0;
				m_CurrentParticleIndex = 0;
			
				// Show particle
				ShowParticle();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
			// Check if there is any particle in prefab list
			if(m_CurrentElementIndex!=-1 && m_CurrentParticleIndex!=-1)
			{
				// User released Up arrow key
				if(Input.GetKeyUp(KeyCode.UpArrow))
				{
					m_CurrentElementIndex++;
					m_CurrentParticleIndex = 0;
					ShowParticle();
				}
				// User released Down arrow key
				else if(Input.GetKeyUp(KeyCode.DownArrow))
				{
					m_CurrentElementIndex--;
					m_CurrentParticleIndex = 0;
					ShowParticle();
				}
				// User released Left arrow key
				else if(Input.GetKeyUp(KeyCode.LeftArrow))
				{
					m_CurrentParticleIndex--;
					ShowParticle();
				}
				// User released Right arrow key
				else if(Input.GetKeyUp(KeyCode.RightArrow))
				{
					m_CurrentParticleIndex++;
					ShowParticle();
				}
			}
		}
	
		// OnGUI is called for rendering and handling GUI events.
		void OnGUI () {
		
			// Show Information GUI window
			GUI.Window(12, new Rect(350, Screen.height-110, 385, 105), ParticleInfoWindow, "Current Particle");
		}
	
	#endregion Component Segments
	
	// ######################################################################
	// Functions Functions
	// ######################################################################

	#region Functions
		
		// Remove old Particle and do Create new Particle GameObject
		void ShowParticle()
		{
			// Make m_CurrentElementIndex be rounded
			if(m_CurrentElementIndex>5)
			{
				m_CurrentElementIndex = 0;
			}
			else if(m_CurrentElementIndex<0)
			{
				m_CurrentElementIndex = 5;
			}
			
			// update current m_CurrentElementList and m_ElementName
			if(m_CurrentElementIndex==0)
			{
				m_CurrentElementList = m_PrefabListFire;
				m_ElementName = "FIRE";
			}
			else if(m_CurrentElementIndex==1)
			{
				m_CurrentElementList = m_PrefabListWater;
				m_ElementName = "WATER";
			}
			else if(m_CurrentElementIndex==2)
			{
				m_CurrentElementList = m_PrefabListWind;
				m_ElementName = "WIND";
			}
			else if(m_CurrentElementIndex==3)
			{
				m_CurrentElementList = m_PrefabListEarth;
				m_ElementName = "EARTH";
			}
			else if(m_CurrentElementIndex==4)
			{
				m_CurrentElementList = m_PrefabListLight;
				m_ElementName = "LIGHT";
			}
			else if(m_CurrentElementIndex==5)
			{
				m_CurrentElementList = m_PrefabListDarkness;
				m_ElementName = "DARKNESS";
			}
	
			// Make m_CurrentParticleIndex be rounded
			if(m_CurrentParticleIndex>=m_CurrentElementList.Length)
			{
				m_CurrentParticleIndex = 0;
			}
			else if(m_CurrentParticleIndex<0)
			{
				m_CurrentParticleIndex = m_CurrentElementList.Length-1;
			}
	
			// update current m_ParticleName
			m_ParticleName = m_CurrentElementList[m_CurrentParticleIndex].name;
	
			// Remove Old particle
			if(m_CurrentParticle!=null)
			{
				DestroyObject(m_CurrentParticle);
			}
	
			// Create new particle
			m_CurrentParticle = (GameObject) Instantiate(m_CurrentElementList[m_CurrentParticleIndex]);
		}
		
		// Show Information window
		void ParticleInfoWindow(int id)
		{
		GUI.Label(new Rect(12, 25, 300, 20), "Element:\t\t" + m_ElementName);
		GUI.Label(new Rect(12, 45, 300, 20), "Particle:\t\t" + m_ParticleName);
		GUI.Label(new Rect(240, 25, 300, 20), "(Press Up/Down Key)");
		GUI.Label(new Rect(240, 45, 300, 20), "(Press Left/Right Key)");
		}
		
	#endregion {Functions}
}
