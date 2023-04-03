using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Basic temp script to manage the one button used in the UI.
public class UIManager : MonoBehaviour
{
    public GameObject welcomePanel;

    public void HideWelcomePanel() {
        welcomePanel.SetActive(false);
    }
}
