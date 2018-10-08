using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    // speed is the rate at which the object will rotate
    public float speed;

    public bool flipRotation = false;

    void LateUpdate()
    {

        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            //Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Vector3 lookRotation = (transform.position - targetPoint);
            if (flipRotation == true)
            {
                lookRotation = -(transform.position - targetPoint);
            }

            Quaternion targetRotation = Quaternion.LookRotation(lookRotation, Vector3.up);

            //Smoothly rotate towards the target point.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);

        }
    }
}