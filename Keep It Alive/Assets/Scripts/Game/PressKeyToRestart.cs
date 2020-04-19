using UnityEngine;
using UnityEngine.SceneManagement;

public class PressKeyToRestart : MonoBehaviour
{
    public KeyCode key = KeyCode.R;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
