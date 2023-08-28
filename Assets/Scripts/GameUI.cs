using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject lifeContainer;
    public Player player;
    public Image heart;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int current = player.getLife();
        int uiHearts = lifeContainer.transform.childCount;
        if (current < uiHearts)
        {
            Destroy(lifeContainer.transform.GetChild(uiHearts-1).gameObject);
        }
        if (current > uiHearts)
        {
            Instantiate(heart, new Vector3(lifeContainer.transform.position.x+50+uiHearts*100, lifeContainer.transform.position.y, lifeContainer.transform.position.z), lifeContainer.transform.rotation, lifeContainer.transform);
        }
    }
}
