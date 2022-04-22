using UnityEngine;

public class CameraWithNoShadow : MonoBehaviour
{

    float storedShadowDistance;

    void OnPreRender()
    {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }

    void OnPostRender()
    {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
}