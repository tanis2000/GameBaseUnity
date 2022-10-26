using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameBase.Dev
{
    public class DevReload : MonoBehaviour
    {
        private void Update()
        {
            if (!Application.isEditor) return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneChanger.SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}