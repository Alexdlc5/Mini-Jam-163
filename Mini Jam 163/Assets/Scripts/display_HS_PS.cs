using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class display_HS_PS : MonoBehaviour
{
    public TextMeshProUGUI high;
    public TextMeshProUGUI prev;
    // Start is called before the first frame update
    void Start()
    {
        high.text = "Fastest Time: " + player_data.best_time + "sec";
        prev.text = "Previous Time: " + player_data.previous_time + "sec";
    }
}
