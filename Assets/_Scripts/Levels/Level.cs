using System;
using _Scripts.BulletsForCannon;
using _Scripts.EnvironmentElements;
using _Scripts.PixelObject;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelColor currentLevelColor;
    [SerializeField] private PixelObjectType[] _objects;
    [SerializeField] private PixelObjectPrefabs _pixelObjectPrefabs;
    [SerializeField] private ContainerColor[] _containeColors;
    [SerializeField] private int[] _bulletNumber;
    [SerializeField] private BulletsCounter _bulletsCounter;
    [SerializeField] private WallsColor _wallsColor;

    public LevelColor CurrentLevelColor => currentLevelColor;

    public PixelObjectType[] Objects
    {
        get => _objects;
        set => _objects = value;
    }

    private int _currentPrefabNumber = 0;
    private int _currentContainerColorNumber = 0;
    private PixelObject _currentPrefab;
    
    public PixelObject PixelObjectPrefab()
    {
        _currentPrefab = null;
        
        switch (_objects[_currentPrefabNumber])
        {
            case PixelObjectType.Turtle:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[0];
                break;
            case PixelObjectType.Amogus:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[1];
                break;
            case PixelObjectType.Bunny:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[2];
                break;
            case PixelObjectType.Burger:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[3];
                break;
            case PixelObjectType.Cat:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[4];
                break;
            case PixelObjectType.Coale:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[5];
                break;
            case PixelObjectType.Dog:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[6];
                break;
            case PixelObjectType.Elephant:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[7];
                break;
            case PixelObjectType.Fox:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[8];
                break;
            case PixelObjectType.Hamster:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[9];
                break;
            case PixelObjectType.IronMan:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[10];
                break;
            case PixelObjectType.Lion:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[11];
                break;
            case PixelObjectType.Melon:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[12];
                break;
            case PixelObjectType.Pear:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[13];
                break;
            case PixelObjectType.PocketBall:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[14];
                break;
            case PixelObjectType.SadCat:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[15];
                break;
            case PixelObjectType.Santa:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[16];
                break;
            case PixelObjectType.Smile:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[17];
                break;
            case PixelObjectType.SpiderMan:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[18];
                break;
            case PixelObjectType.Stormtrooper:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[19];
                break;
            case PixelObjectType.Vinni:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[20];
                break;
            case PixelObjectType.BulletContainer:
                _currentPrefab = _pixelObjectPrefabs.Prefabs[21];
                _currentPrefab.ColorContainer = _containeColors[_currentContainerColorNumber];
                _currentPrefab.BulletCount = _bulletNumber[_currentContainerColorNumber];
                _currentPrefab.Counter = _bulletsCounter;
                _currentContainerColorNumber++;
                break;
        }

        _currentPrefabNumber++;

        return _currentPrefab;
    }

    private void Start()
    {
        BackGroundWalls.Instance.SetWallsColor(_wallsColor);
    }
}

public enum LevelColor
{
    WhiteBlue,
    WhiteGreen,
    WhitePink,
    None
}

public enum PixelObjectType
{
    None,
    Turtle,
    Amogus,
    Bunny,
    Burger,
    Cat,
    Coale,
    Dog,
    Elephant,
    Fox,
    Hamster,
    IronMan,
    Lion,
    Melon,
    Pear,
    PocketBall,
    SadCat,
    Santa,
    Smile,
    SpiderMan,
    Stormtrooper,
    Vinni,
    BulletContainer
}
