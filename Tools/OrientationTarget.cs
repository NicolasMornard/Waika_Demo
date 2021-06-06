using UnityEngine;

public class OrientationTarget : MonoBehaviour
{
	[SerializeField] private bool displayOrientationTarget = false;

	private MeshRenderer localMeshRenderer;

	private void Awake()
	{
		localMeshRenderer = GetComponent<MeshRenderer>();

		if (localMeshRenderer == null)
		{
			Debug.LogWarningFormat("{0} does not have a Mesh Renderer!");
		}

		if (!displayOrientationTarget)
		{
			DisableMeshRenderer();
		}
	}

	private void DisableMeshRenderer()
	{
		localMeshRenderer.enabled = false;
	}
}