// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFFollowObject class
* Convert easetype between Hotween, Leantween and iTween tweeners.
**************/

public class FFFollowObject : MonoBehaviour {	
	
	#region Variables
		// Target object to follow
		public Transform m_Target = null;
		
		// Directions to enable/disable
		public bool m_TranslateXPosition = true;
		public bool m_TranslateYPosition = false;
		public bool m_TranslateZPosition = true;
	
		Bounds m_Bounds;
		Vector3 m_DifCenter;
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
	
		// Use this for initialization
		void Start () {		
			if(m_Target!=null)
			{
				// Target object has renderer mesh
				if(m_Target.renderer!=null)
				{
					// Find a center of bounds
					m_Bounds = m_Target.renderer.bounds;
					foreach (Transform child in m_Target)
					{
					    m_Bounds.Encapsulate(child.gameObject.renderer.bounds);
					}
				}
				// No renderer mesh in target object
				else
				{
					// Find a center of bounds
					Vector3 center = Vector3.zero;				
					foreach (Transform child in m_Target)
					{
					    center += child.gameObject.renderer.bounds.center;
					}
					
					// Center is average center of children
					center /= m_Target.childCount;
					
					// Calculate the bounds by creating a zero sized 'Bounds'
					m_Bounds = new Bounds(center,Vector3.zero); 				
					foreach (Transform child in m_Target)
					{
					    m_Bounds.Encapsulate(child.gameObject.renderer.bounds);   
					}
				}
				
				// Set m_DifCenter
				m_DifCenter = m_Bounds.center - m_Target.transform.position;
			}
		}
		
		// Update is called once per frame
		void Update () {		
			// Make a move if any TranslatePosition is set
			if(m_TranslateXPosition==true || m_TranslateYPosition==true || m_TranslateZPosition==true)
			{
				// Keep current position
				float x = transform.position.x;
				float y = transform.position.y;
				float z = transform.position.z;
				
				// Set position to target object
				if(m_TranslateXPosition==true)
					x = m_Target.transform.position.x + m_DifCenter.x;
				if(m_TranslateYPosition==true)
					y = m_Target.transform.position.y + m_DifCenter.y;
				if(m_TranslateZPosition==true)
					z = m_Target.transform.position.z + m_DifCenter.z;
					
				// Move this object
				transform.position = new Vector3(x, y, z);
			}
		}
	
	#endregion {Component Segments}
}

				
