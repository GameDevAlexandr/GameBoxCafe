using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] private Controll _controll;
    [SerializeField] private PathFinder _pathFinder;
    [SerializeField] private Player _player;
    [SerializeField] private DataManager _data;
    [SerializeField] private Cafe _cafe;
    [SerializeField] private ClientSpawner _spawner;
    [SerializeField] private Bench _bench;
    [SerializeField] private TrashCan _trashCan;
    public override void InstallBindings()
    {
        Container.Bind<Controll>().FromInstance(_controll).AsSingle();
        Container.Bind<PathFinder>().FromInstance(_pathFinder).AsSingle();
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<Cafe>().FromInstance(_cafe).AsSingle();
        Container.Bind<ClientSpawner>().FromInstance(_spawner).AsSingle();
        Container.Bind<DataManager>().FromInstance(_data).AsSingle();
        Container.Bind<Bench>().FromInstance(_bench).AsSingle();
        Container.Bind<TrashCan>().FromInstance(_trashCan).AsSingle();
    }
}