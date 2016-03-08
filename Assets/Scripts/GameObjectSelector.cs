using UnityEngine;
using System.Collections;

public class GameObjectSelector : MonoBehaviour {

    public string[] tagsToSelect;
    public float distanceToCheck;

    GameObject cachedObj;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool selectingObject = false;

        if (Physics.Raycast(ray, out hit, distanceToCheck))
        {
            foreach (string tagName in tagsToSelect)
            {
                if (hit.collider.tag == tagName)
                {
                    selectingObject = true;

                    GameObject curObj = hit.collider.gameObject;
                    if (curObj != cachedObj) //new object selected
                    {
                        ParticleSystem particleSys;
                        ParticleSystem.EmissionModule emmisionModule;

                        particleSys = curObj.GetComponent<ParticleSystem>();

                        if (particleSys == null)
                        {
                            // Add particle system to newly selected object
                            particleSys = curObj.AddComponent<ParticleSystem>();
                            particleSys.startLifetime = 1.0f;
                            particleSys.startSpeed = 0.05f;
                            particleSys.startSize = 0.5f;

                            MeshCollider collider = curObj.GetComponent<MeshCollider>();

                            ParticleSystem.ShapeModule shapeModule = particleSys.shape;
                            shapeModule.shapeType = ParticleSystemShapeType.Mesh;
                            shapeModule.mesh = collider.sharedMesh;

                            emmisionModule = particleSys.emission;
                            emmisionModule.type = ParticleSystemEmissionType.Time;
                            emmisionModule.rate = new ParticleSystem.MinMaxCurve(10.0f, 10.0f);
                        }

                        emmisionModule = particleSys.emission;
                        emmisionModule.enabled = true;

                        deselectObject();

                        cachedObj = curObj; //cache the new selected object
                    }
                }
            }

            if (!selectingObject)
            {
                deselectObject();
                cachedObj = null;
            }
        }
        else
        {
            deselectObject();
            cachedObj = null;
        }
    }

    void deselectObject()
    {
        // Disable and remove particle system from previously selected object
        if (cachedObj != null)
        {
            ParticleSystem cachedParticleSys = cachedObj.GetComponent<ParticleSystem>();
            ParticleSystem.EmissionModule cachedEmisionModule = cachedParticleSys.emission;
            cachedEmisionModule.enabled = false;
        }
    }

}