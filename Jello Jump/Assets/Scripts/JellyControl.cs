using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class JellyControl : MonoBehaviour {

	public bool canJump = false;
	public float jumpForce = 500;
	public LayerMask mask;
	public bool grounded = false;
	[HideInInspector]
	public JellyMesh m_JellyMesh;
	public GameObject refPoint;
	public GameManager manager;

	[Header("ColorSetup")]
	public Color mainColor;

	public bool spawnedParticle = false;
	public GameObject splatFX;
	public GameObject jumpFX;

	void Start()
	{
		m_JellyMesh = GetComponent<JellyMesh>();
		if(m_JellyMesh.m_ReferencePoints[0].Body3D != null)
		{
			m_JellyMesh.m_ReferencePoints[0].Body3D.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			refPoint = m_JellyMesh.m_ReferencePointParent.gameObject;
		}
	}

	void Update()
	{
		grounded = m_JellyMesh.IsGrounded(mask,1);	
		if(grounded)
		{
			if(!spawnedParticle)
			{
				RaycastHit hit;
				if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y + 1,transform.position.z), Vector3.down, out hit, mask)) 
				{
					manager.clips[2].Play();
					GameObject _splatFX = Instantiate(splatFX,new Vector3(hit.point.x,hit.point.y + 0.05f,hit.point.z),Quaternion.identity) as GameObject;
					_splatFX.transform.parent = manager.generator.currentPlatform.transform;
					spawnedParticle = true;
				}

			}
			
		}
		else
		{
			spawnedParticle = false;
		}

	}
	public void Jump()
	{
		if(manager.generator.currentPlatform.locked)
		{
			print(transform.position.y - manager.generator.currentPlatform.transform.position.y);
			if(transform.position.y - manager.generator.currentPlatform.transform.position.y < 0.3f)
			{
				GameObject _jumpFX = Instantiate(jumpFX,transform.position,Quaternion.identity) as GameObject;
				_jumpFX.transform.parent = transform;
				m_JellyMesh.m_ReferencePointParent.transform.position += Vector3.up * 1;
			}
		}

		grounded = m_JellyMesh.IsGrounded(mask,1);

		if(canJump && grounded)
		{
			GameObject _jumpFX = Instantiate(jumpFX,transform.position,Quaternion.identity) as GameObject;
			_jumpFX.transform.parent = transform;

			manager.clips[1].Play();
			m_JellyMesh.CentralPoint.Body3D.velocity = Vector3.zero;
			m_JellyMesh.AddForce(Vector3.up * jumpForce, true);
		}
		else
		{
			canJump = true;
		}
	}

	void Harakiri()
	{
		Destroy(m_JellyMesh.m_ReferencePointParent.gameObject);
		Destroy(gameObject);
	}
}
