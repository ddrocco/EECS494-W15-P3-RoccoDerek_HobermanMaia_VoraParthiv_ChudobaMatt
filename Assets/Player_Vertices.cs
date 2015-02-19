using UnityEngine;
using System.Collections;

public class Player_Vertices : MonoBehaviour {
	public Vector3[] GetVertices() {
		Vector3[] vertices = new Vector3[14];
		float xExtents = collider.bounds.extents.x;
		float yExtents = collider.bounds.extents.y;
		float zExtents = collider.bounds.extents.z;
		
		//Corners:
		vertices[0] = transform.position + new Vector3(xExtents, yExtents, -zExtents);
		vertices[1] = transform.position + new Vector3(xExtents, yExtents, zExtents);
		vertices[2] = transform.position + new Vector3(xExtents, -yExtents, -zExtents);
		vertices[3] = transform.position + new Vector3(xExtents, -yExtents, zExtents);
		vertices[4] = transform.position + new Vector3(-xExtents, yExtents, -zExtents);
		vertices[5] = transform.position + new Vector3(-xExtents, yExtents, zExtents);
		vertices[6] = transform.position + new Vector3(-xExtents, -yExtents, -zExtents);
		vertices[7] = transform.position + new Vector3(-xExtents, -yExtents, zExtents);
		
		//Faces:
		vertices[8] = transform.position + new Vector3(xExtents, 0, 0);
		vertices[9] = transform.position + new Vector3(-xExtents, 0, 0);
		vertices[10] = transform.position + new Vector3(0, yExtents, 0);
		vertices[11] = transform.position + new Vector3(0, -yExtents, 0);
		vertices[12] = transform.position + new Vector3(0, 0, zExtents);
		vertices[13] = transform.position + new Vector3(0, 0, -zExtents);
		
		return vertices;
	}
}
