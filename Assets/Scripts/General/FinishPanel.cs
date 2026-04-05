using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _pollutionText;
    public void Show(int score, int pollution)
    {
        gameObject.SetActive(true);
        _scoreText.text = score.ToString();
        _pollutionText.text = pollution.ToString();
    }
    public void ChangeScene(int scene) => SceneManager.LoadScene(scene);
}
