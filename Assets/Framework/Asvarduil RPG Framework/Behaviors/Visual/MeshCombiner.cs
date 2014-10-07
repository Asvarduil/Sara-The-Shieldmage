using UnityEngine;
using System;
 
public class MeshCombiner : MonoBehaviour 
{ 
	public bool DebugMode = false;
	public MeshFilter[] meshFilters;
	public Material material;
 
	void Start () 
	{
	    // if not specified, go find meshes
	    if(meshFilters.Length == 0)
	    {
			// find all the mesh filters
			Component[] comps = GetComponentsInChildren(typeof(MeshFilter));
			meshFilters = new MeshFilter[comps.Length];
			
			int filterIndex = 0;
			foreach(Component comp in comps)
				meshFilters[filterIndex++] = (MeshFilter) comp;
	    }
	 
	    // figure out array sizes
	    int vertCount = 0;
	    int normCount = 0;
	    int triCount = 0;
	    int uvCount = 0;
	 
	    foreach(MeshFilter filter in meshFilters)
	    {
	      	vertCount += filter.mesh.vertices.Length; 
	      	normCount += filter.mesh.normals.Length;
	      	triCount += filter.mesh.triangles.Length; 
	      	uvCount += filter.mesh.uv.Length;
		  
			Material mat = null;
			try
			{
				mat = filter.gameObject.renderer.material;
			}
			catch (MissingComponentException)
			{
				continue;
			}	
			catch (Exception ex)
			{
				Debug.Log("Unhandled Exception has occurred! \r\n"
					      + ex.Message + "\r\n"
					      + ex.StackTrace);
			}
			
			if((material == null) && (mat != null))
	        	material = mat;
	    }
	 
	    // allocate arrays
	    Vector3[] verts = new Vector3[vertCount];
	    Vector3[] norms = new Vector3[normCount];
	    Transform[] aBones = new Transform[meshFilters.Length];
	    Matrix4x4[] bindPoses = new Matrix4x4[meshFilters.Length];
	    BoneWeight[] weights = new BoneWeight[vertCount];
	    int[] tris  = new int[triCount];
	    Vector2[] uvs = new Vector2[uvCount];
	 
	    int vertOffset = 0;
	    int normOffset = 0;
	    int triOffset = 0;
	    int uvOffset = 0;
	    int meshOffset = 0;
	 
	    // merge the meshes and set up bones
	    foreach(MeshFilter filter in meshFilters)
	    {     
			foreach(int i in filter.mesh.triangles)
				tris[triOffset++] = i + vertOffset;
			
			aBones[meshOffset] = filter.transform;
			bindPoses[meshOffset] = Matrix4x4.identity;
			
			foreach(Vector3 v in filter.mesh.vertices)
			{
				weights[vertOffset].weight0 = 1.0f;
				weights[vertOffset].boneIndex0 = meshOffset;
				verts[vertOffset++] = v;
			}
			
			foreach(Vector3 n in filter.mesh.normals)
				norms[normOffset++] = n;
			
			foreach(Vector2 uv in filter.mesh.uv)
				uvs[uvOffset++] = uv;
			
			meshOffset++;
			
			MeshRenderer meshRenderer = filter.gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
			if(meshRenderer)
				meshRenderer.enabled = false;
	    }
	 
	    // hook up the mesh
	    Mesh me = new Mesh();       
	    me.name = gameObject.name;
	    me.vertices = verts;
	    me.normals = norms;
	    me.boneWeights = weights;
	    me.uv = uvs;
	    me.triangles = tris;
	    me.bindposes = bindPoses;
	 
	    // hook up the mesh renderer        
	    SkinnedMeshRenderer smr = gameObject.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
	 
	    smr.sharedMesh = me;
	    smr.bones = aBones;
	    renderer.material = material;
	}
	
	public void OnDestroy()
	{
		SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
		if(smr == null)
			return;
		
		smr.material.mainTexture = null;
		smr.material = null;
		smr.bones = null;
		smr.sharedMesh = null;
	}
}
