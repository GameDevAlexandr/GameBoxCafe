using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataManager : MonoBehaviour
{
    public Action onChangeCoins;
    public Action onChangePollution;
    public int Pollution { get; private set; }
    public int Coins { get; private set; }
    public int Score { get; private set; }
    [SerializeField] private float _removePolutionAtSecond;
    [SerializeField] private int _playTimeSeconds;
    [SerializeField] private Text _pollutionText;
    [SerializeField] private Text _coinsText;
    [SerializeField] private Text _gameTimerText;
    [SerializeField] private Text _scroreText;
    [SerializeField] private FinishPanel _finishPanel;
    private int _gameTime;
    private float _curPollution;
    private void Start()
    {
        StartCoroutine(Tic());
    }
    public void ChangePollution(int pollution)
    {
        Pollution = Mathf.Clamp(Pollution + pollution, 0, 100);
        if (Pollution == 100)
        {
            _finishPanel.Show(Score-Pollution, Pollution);
        }
        _pollutionText.text = Pollution+"%";
        _pollutionText.color = Color.Lerp(Color.green, Color.red, (float)Pollution / 100);
        ChangeScore(0);
        onChangePollution?.Invoke();
    }
    public void ChangeCoins(int coins)
    {        
        Coins = Mathf.Max(0,Coins+ coins);        
        _coinsText.text = Coins.ToString();
        onChangeCoins?.Invoke();

        if (coins > 0)
            ChangeScore(coins*2);
    }
    public void ChangeScore(int score)
    {
        Score += score;
        int Res = Mathf.Max(0,Score- Pollution);
        _scroreText.text = Res.ToString();
    }
    private void ChangePlayTime()
    {
        _gameTime++;
        if(_playTimeSeconds == _gameTime)
        {
            _finishPanel.Show(Score-Pollution, Pollution);
        }
        int sec = _playTimeSeconds - _gameTime;
        TimeSpan ts = TimeSpan.FromSeconds(sec);
        _gameTimerText.text = ts.ToString(@"m\:ss");
    }
    private IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ChangePlayTime();
            _curPollution += _removePolutionAtSecond;
            if (_curPollution >= 1)
            {
                ChangePollution(-1);
                _curPollution -= 1;
            }
        }
    }

}
