using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameButton()
    {
        BattleController.main.StartMove();
        gameObject.SetActive(false);
    }
}
