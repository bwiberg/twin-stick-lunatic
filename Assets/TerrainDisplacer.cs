using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDisplacer : MonoBehaviour {
	[SerializeField] private bool DisplaceEachFrame = false;
	[SerializeField, Range(0, 100)] private float DisplaceHeight = 10.0f;
	[SerializeField, Range(0, 10)] private float DisplaceScale = 1.0f;
	
	private MeshFilter meshFilter;
	private MeshCollider collider;

	private Mesh mesh {
		get { return meshFilter.sharedMesh; }
	}
	
	private void Start () {
		meshFilter = GetComponent<MeshFilter>();
		collider = GetComponent<MeshCollider>();

		if (!DisplaceEachFrame) {
			DisplaceMesh();
		}
	}

	private void Update() {
		if (DisplaceEachFrame) {
			DisplaceMesh();
		}
	}

	private void DisplaceMesh() {
		Vector3[] positions = mesh.vertices;
		for (int i = 0; i < positions.Length; ++i) {
			positions[i].y = DisplaceHeight * Mathf.PerlinNoise(DisplaceScale * positions[i].x, DisplaceScale * positions[i].z);
		}
		mesh.vertices = positions;

		collider.sharedMesh = null;
		collider.sharedMesh = mesh;
	}
}
