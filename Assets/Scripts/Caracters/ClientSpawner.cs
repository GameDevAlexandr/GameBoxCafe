using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private int _spawnDelay;
    [SerializeField] private Client _clientPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    private Queue<Client> _clienQueue = new Queue<Client>();
    private int[] _spawnPosition;
    [Inject] DiContainer _container;
    [Inject] PathFinder _pFinder;

    [Inject]
    private void Init()
    {
        _pFinder.onRoadComplete += () =>
         {
             _spawnPosition = new int[_spawnPoints.Length];
             for (int i = 0; i < _spawnPoints.Length; i++)
             {
                 _spawnPosition[i] = _pFinder.GetRoadPosition(_spawnPoints[i].position);
             }
         };
    }
    private void Start()
    {
        StartCoroutine(Tic());
    }
    private IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            Client c;
            if (!_clienQueue.TryDequeue(out c))
            {
                c = _container.InstantiatePrefabForComponent<Client>(_clientPrefab);
            }
            int rnd = Random.Range(0, _spawnPoints.Length);
            c.SetPosition(_spawnPosition[rnd]);
            c.MoveToCafe();
        }
    }
    public void Comeback(Client client) => _clienQueue.Enqueue(client);
}
