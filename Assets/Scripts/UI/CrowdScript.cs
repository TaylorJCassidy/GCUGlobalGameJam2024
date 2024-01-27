using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdScript : MonoBehaviour
{
    public Slider crowd;

    public void SetCrowdMax(int CrowdScore)
    {
        crowd.maxValue = CrowdScore;
        crowd.value = CrowdScore;
    }
    public void SetCrowd(int CrowdScore)
    {
        crowd.value = CrowdScore;
    }
}
