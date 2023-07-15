using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{
    /// <summary>
    /// Get MousePosition by drawing a Ray from <paramref name="camera"/> to Objects inside <paramref name="layerMask"/>
    /// and set MousePosition.y to equal height with the <paramref name="sender"/>.
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="layerMask"></param>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static Vector3 GetMousePosition(Camera camera, LayerMask layerMask, Transform sender)
    {
        Vector3 mousePosition = Vector3.zero;

        // Ray von Camera zu MausPosition berechnen.
        // Kollisionspunkt von Ray und Boden = Mausposition auf dem Boden.
        // Keine Kollision mit Objekten, deren Layer nicht Teil der _layerMask sind
        // => Wände, Gegner blocken Ray nicht.
        Ray cameraToGroundRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraToGroundRay, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            // Wo trifft Ray von Maus zu Boden -> MausPosition
            mousePosition = raycastHit.point;
            // Gleiche Hoehe Sender und Ziel -> Schaut geradeaus
            mousePosition.y = sender.position.y;
        }
        else
            throw new Exception("Can't find mousePosition. No Ray-Collision with Layermask detected.");

        return mousePosition;
    }
}
