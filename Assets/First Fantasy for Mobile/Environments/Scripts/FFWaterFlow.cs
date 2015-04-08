// First Fantasy for Mobile version: 1.3.2
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/***************
* FFWaterFlow class.
* This class moves UV texture on material.
**************/

public class FFWaterFlow : MonoBehaviour {

	#region Variables
	
	    public float m_SpeedU = 0.1f;
	    public float m_SpeedV = -0.1f;
	
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments

	// Update is called once per frame
	void Update () {
        float newOffsetU = Time.time * m_SpeedU;
        float newOffsetV = Time.time * m_SpeedV;

        if (this.renderer)
        {
            renderer.material.mainTextureOffset = new Vector2(newOffsetU, newOffsetV);
        }
	}
	
	#endregion {Component Segments}
}