using System.Collections.Generic;
using Others.Utils;
using UnityEngine;

namespace HomeScene
{
    public class HomeMenuItemCreator : MonoBehaviour
    {
        [SerializeField] private HomeMenuItem menuItemPrefab;

        private readonly List<HomeMenuItem> _menuItemInstances = new();
        

        [SerializeField] private float spreadAngle;
        [SerializeField] private float angleOffset;
        [SerializeField] private float radius;
        [SerializeField] private float yDistance;

        [SerializeField] private float positionOffsetX;
        [SerializeField] private float positionOffsetY;


        private void Start()
        {
            foreach (var homeMenuType in EnumUtil<HomeMenuType>.GetValues())
            {
                var instance = Instantiate(menuItemPrefab, transform);
                instance.gameObject.SetActive(true);
                instance.Text = homeMenuType.ToString();

                _menuItemInstances.Add(instance);
            }
        }

        private void Update()
        {
            for (var i = 0; i < _menuItemInstances.Count; i++)
            {
                var normalAngle = spreadAngle / _menuItemInstances.Count;
                var angle = normalAngle * -i;
                angle += angleOffset;
                var posX = (Vector2Extension.AngleToVector(angle * Mathf.Deg2Rad) * radius).x;
                var posY = yDistance * i;
                var pos = new Vector2(posX, posY);
                pos += new Vector2(positionOffsetX, positionOffsetY);

                _menuItemInstances[i].transform.localPosition = pos;
            }
        }

    }
}