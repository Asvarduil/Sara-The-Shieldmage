// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFFloatObject class
* This class floats object up/down on water.
**************/

public class FFFloatObject : MonoBehaviour {
	
	#region Variables
		public float m_Time = 2.0f;
		public float m_TimeSpread = 0.25f;
		public float m_HorizontalSpread = 0.25f;
		public float m_VerticalSpread = 0.25f;
		
		float m_TimeRound = 1;
		float m_TimeCount = 0;
		Vector3	m_StartPosition;
		Vector3 m_EndPosition;

		enum statMove
		{
			statMoveBegin,
			statMoveAway,
			statMoveBack
		};
		statMove m_statMove = statMove.statMoveBegin;
	
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
	
		// Use this for initialization
		void Start () {
			
			// Keep original position for floating forward/backward
			m_StartPosition = this.transform.localPosition;
			
			// Setup for first move
			SetupNewMove();
		}
	
		// Update is called once per frame
		void Update () {
			if(m_TimeCount>=m_TimeRound)
			{
				// Setup next move
				SetupNewMove();
			}
			else
			{
				float CalTime = m_TimeCount/m_TimeRound;		
				// Floating forward
				if(m_statMove==statMove.statMoveAway)
				{
					transform.localPosition = Vector3.Lerp(m_StartPosition, m_EndPosition, CalTime);
				}
				// Floating backward
				else
				{
					transform.localPosition = Vector3.Lerp(m_EndPosition, m_StartPosition, CalTime);				
				}
				m_TimeCount += Time.deltaTime;
			}
		}
	
	#endregion {Component Segments}	
	
	// ######################################################################
	// Functions Functions
	// ######################################################################

	#region Functions
		
		void SetupNewMove()
		{
			// Random round time
			m_TimeRound = m_Time + Random.Range(-m_TimeSpread,m_TimeSpread);
			m_TimeCount = 0;		
			
			// Check for update float state and random next position
			if(m_statMove==statMove.statMoveAway)
			{
				m_statMove = statMove.statMoveBack;
			}
			else if(m_statMove==statMove.statMoveBack || m_statMove==statMove.statMoveBegin)
			{
				// Random next position
				m_EndPosition = m_StartPosition + new Vector3(Random.Range(-m_HorizontalSpread,m_HorizontalSpread), Random.Range(-m_VerticalSpread,m_VerticalSpread), Random.Range(-m_HorizontalSpread,m_HorizontalSpread));
				m_statMove = statMove.statMoveAway;
			}
		}
	
	#endregion {Functions}
}

				