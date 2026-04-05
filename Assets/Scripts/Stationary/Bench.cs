using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bench : StationaryBase
{
    [Range(1,10)][SerializeField] private float _sitTime;
    [Range(0,100)][SerializeField] private int _comebackChance;
    [SerializeField] private Image[] _clientImages;
    private List<ClientData> _clients = new List<ClientData>();
    private class ClientData
    {
        public Client client;
        public float sitTime;
    }
    public void ClientHere(Client client)
    {
        if (_clients.Count == _clientImages.Length)
        {
            client.GoHome();
        }
        else
        {
            _clients.Add(new ClientData() { client = client });
            ShowClients(true);
        }
    }
    private void ShowClients(bool isShow)
    {
        _clientImages[_clients.Count - 1].gameObject.SetActive(isShow);
        _clientImages[0].transform.parent.gameObject.SetActive(_clientImages[0].gameObject.activeSelf);
    }
    private void Update()
    {
        if (_clients.Count > 0)
        {
            int idx = 0;
            while (idx<_clients.Count)
            {
                _clients[idx].sitTime+=Time.deltaTime;
                if (_clients[idx].sitTime >= _sitTime)
                {
                    SitFinish(_clients[idx].client);
                    ShowClients(false);
                    _clients.RemoveAt(idx);
                    continue;
                }
                idx++;
            }
        }
    }
    private void SitFinish(Client client)
    {
        int rnd = Random.Range(0, 100);
        if (_comebackChance > rnd)
        {
            client.MoveToCafe();
        }
        else
        {
            client.GoHome();
        }
    }

    protected override void PlayerHere() {}
}
