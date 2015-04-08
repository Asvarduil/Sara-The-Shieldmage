// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFRandomMoveOnTerrain class.
* This class checks terrain height and randomly move object on terrain.
**************/

public class FFRandomMoveOnTerrain : MonoBehaviour {
	
	#region Variables
		public Terrain m_Terrain = null;
		
		public float m_Speed = 1.0f;
		public float m_SpeedSpread = 0.25f;
		
		public float m_MoveDistance = 3.0f;
		
		float m_currentSpeed = 0;
		float m_TimeRound = 1;
		float m_TimeCount = 0;
		Vector3	m_StartPosition;
		Vector3 m_EndPosition;
		Bounds m_LimitArea;
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
	
		// Use this for initialization
		void Start () {
			// Keep first position to use for forward/backward floating
			m_StartPosition = this.transform.position;
			m_EndPosition = m_StartPosition;
			
			// Setup m_LimitArea if terrain is set
			if(m_Terrain!=null)
			{
				m_LimitArea.min = m_Terrain.transform.position;
				m_LimitArea.max = m_LimitArea.min + m_Terrain.terrainData.size;
				m_LimitArea.center = (m_LimitArea.min + m_LimitArea.max)/2;
			}
			
			// Setup for first move
			SetupNewMove();
		}
	
		// Update is called once per frame
		void Update () {
			if(m_Terrain!=null)
			{
				if(m_TimeCount>=m_TimeRound)
				{
					// Setup next move
					SetupNewMove();
				}
				else
				{
					// Move object along the way
					float CalTime = m_TimeCount/m_TimeRound;				
					transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, CalTime);
					m_TimeCount += Time.deltaTime;
					
					// Update object y position if terrain is set
					if(m_Terrain!=null)
					{
						// update object y position
						float fTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
						transform.position = new Vector3(transform.position.x, fTerrainHeight+m_LimitArea.min.y, transform.position.z);
					}
				}
			}
		}
	
	#endregion {Component Segments}
	
	// ######################################################################
	// Functions
	// ######################################################################

	#region Component Segments
		
		void SetupNewMove()
		{
			// Random next position
			m_StartPosition = this.transform.position;
			m_EndPosition = m_StartPosition + new Vector3(Random.Range(-m_MoveDistance,m_MoveDistance), 0, Random.Range(-m_MoveDistance,m_MoveDistance));
			
			// if Terrain is set then limit x and z position to area of Terrain
			if(m_Terrain!=null)
			{
				if(m_EndPosition.x < m_LimitArea.min.x)
					m_EndPosition.x = m_LimitArea.min.x;
				if(m_EndPosition.x > m_LimitArea.max.x)
					m_EndPosition.x = m_LimitArea.max.x;
				if(m_EndPosition.z < m_LimitArea.min.z)
					m_EndPosition.z = m_LimitArea.min.z;
				if(m_EndPosition.z > m_LimitArea.max.z)
					m_EndPosition.z = m_LimitArea.max.z;
			}
				
			// Random new Distance to go and new moving speed
			float fDistance = Vector2.Distance(new Vector2(m_StartPosition.x, m_StartPosition.z), new Vector2(m_EndPosition.x, m_EndPosition.z));
			m_currentSpeed = m_Speed + Random.Range(-m_SpeedSpread, m_SpeedSpread);
			
			// calculate time long for this move
			m_TimeRound = fDistance/m_currentSpeed;
			m_TimeCount = 0;
		}
	
	#endregion {Component Segments}
}

				