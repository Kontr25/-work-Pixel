using UnityEngine;

namespace _Scripts.EnvironmentElements
{
    public class BackGroundWalls : MonoBehaviour
    {
        [SerializeField] private Material _blueSide;
        [SerializeField] private Material _blueBG;
        [SerializeField] private Material _pinkSide;
        [SerializeField] private Material _pinkBG;
        [SerializeField] private Material _greenSide;
        [SerializeField] private Material _greenBG;
        [SerializeField] private Material _violetSide;
        [SerializeField] private Material _violetBG;
        [SerializeField] private Material _orangeSide;
        [SerializeField] private Material _orangeBG;
        [SerializeField] private MeshRenderer[] _sideWalls;
        [SerializeField] private MeshRenderer _bgWall;

        public static BackGroundWalls Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                transform.parent = null;
                Instance = this;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetWallsColor(WallsColor wallsColor)
        {
            switch (wallsColor)
            {
                case WallsColor.Blue:
                    ChangeColor(_blueSide, _blueBG);
                    break;
                case WallsColor.Pink:
                    ChangeColor(_pinkSide, _pinkBG);
                    break;
                case WallsColor.Green:
                    ChangeColor(_greenSide, _greenBG);
                    break;
                case WallsColor.Violet:
                    ChangeColor(_violetSide, _violetBG);
                    break;
                case WallsColor.Orange:
                    ChangeColor(_orangeSide, _orangeBG);
                    break;
            }
        }

        private void ChangeColor(Material sideWalls, Material bgWall)
        {
            for (int i = 0; i < _sideWalls.Length; i++)
            {
                _sideWalls[i].material = sideWalls;
            }

            _bgWall.material = bgWall;
        }
    }

    public enum WallsColor
    {
        Blue,
        Pink,
        Green,
        Violet,
        Orange,
        
    }
}