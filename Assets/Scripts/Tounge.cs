using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tounge : MonoBehaviour
{

    [SerializeField] private Transform toungeOrigin;

    [SerializeField] private Transform toungeEnd;

    [SerializeField] private LineRenderer toungeLine;

    // This is the distance the clickable plane is from the camera. Set it in the Inspector before running.
    public float m_DistanceZ;

    Plane m_Plane;
    Vector3 m_DistanceFromCamera;

    bool canShoot = true;


    void Start()
    {
        toungeLine.SetPosition(0, toungeOrigin.position);
        toungeLine.SetPosition(1, toungeOrigin.position);
        // This is how far away from the Camera the plane is placed
        m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - m_DistanceZ);

        // Create a new plane with normal (0,0,1) at the position away from the camera you define in the Inspector. This is the plane that you can click so make sure it is reachable.
        m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
    }


    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            ShootTounge();
        }
    }

    private void ShootTounge()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we want to use a plane for general clicking
        float enter = 0.0f;
        // Snap to thrown object
        Debug.Log(LayerMask.LayerToName(20));
        if (Physics.Raycast(r, out hit))
        {
            if (hit.transform.gameObject.layer == 20)
            {
                canShoot = false;
                Debug.Log("Hit Object");
                toungeLine.SetPosition(0, toungeOrigin.position);
                toungeLine.SetPosition(1, hit.transform.position);

                MovingObject throwObjects = hit.transform.GetComponent<MovingObject>();
                if (throwObjects)
                {
                    Debug.Log("Hit Moving Object");
                    throwObjects.Deactivate();
                    throwObjects.transform.SetParent(toungeEnd.transform);
                    throwObjects.transform.localPosition = Vector3.zero;
                }
                StartCoroutine(ToungeDraw());

                StartCoroutine(ToungeReset());
                return;
            }

        }

        if (m_Plane.Raycast(r, out enter))
        {
            canShoot = false;
            Debug.Log("Hit Plane");
            //Get the point that is clicked
            Vector3 hitPoint = r.GetPoint(enter);

            toungeLine.SetPosition(0, toungeOrigin.position);
            toungeLine.SetPosition(1, hitPoint);

            StartCoroutine(ToungeDraw());
            StartCoroutine(ToungeReset());

        }

    }

    void DrawMeshBetween2Points()
    {
       
    }

    IEnumerator ToungeDraw()
    {
        float t = 0;
        float time = 0.25f;

        Vector3 orig = toungeLine.GetPosition(0);
        Vector3 orig2 = toungeLine.GetPosition(1);

        toungeLine.SetPosition(1, orig);

        Vector3 newpos;
        for (; t < time; t += Time.deltaTime)
        {
            newpos = Vector3.Lerp(orig, orig2, t / time);
            toungeLine.SetPosition(1, newpos);
            toungeEnd.position = newpos;
            yield return null;
        }
        toungeLine.SetPosition(1, orig2);
        yield break;
    }

   
    IEnumerator ToungeDrawBack()
    {
        float t = 0;
        float time = 0.25f;

        Vector3 orig = toungeLine.GetPosition(0);
        Vector3 orig2 = toungeLine.GetPosition(1);

        toungeLine.SetPosition(1, orig2);

        Vector3 newpos;
        for (; t < time; t += Time.deltaTime)
        {
            newpos = Vector3.Lerp(orig2, orig, t / time);
            toungeLine.SetPosition(1, newpos);
            toungeEnd.position = newpos;
            yield return null;
        }
        toungeLine.SetPosition(1, orig);
        if (toungeEnd.childCount > 0)
        {
            Destroy(toungeEnd.GetChild(0).gameObject);
        }
        canShoot = true;
        yield break;
    }

    IEnumerator ToungeReset()
    {
        yield return new WaitForSeconds(0.25f);
        toungeLine.SetPosition(0, toungeOrigin.position);
        StartCoroutine(ToungeDrawBack());
        yield break;
    }

    private void OnDrawGizmos()
    {
        //draw plane
        Plane p = new Plane(Vector3.forward, m_DistanceFromCamera);
        Gizmos.DrawWireCube(p.ClosestPointOnPlane(transform.position), new Vector3(10, 10, 0.1f));
    }
}