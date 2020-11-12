using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //Menus
    public GameObject buttonIssueOrders, OrdersPanel, InfoBoxPanel;
        
    // Start is called before the first frame update
    void Start()
    {
        OrdersPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIssueOrderMenu()
    {
        OrdersPanel.SetActive(!OrdersPanel.activeSelf);
    }
}
