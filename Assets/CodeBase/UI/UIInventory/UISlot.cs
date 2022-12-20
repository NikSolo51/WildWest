using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Logic;
using CodeBase.Services.Hud;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.UIInventory
{
    public class UISlot : MonoBehaviour, IPointerDownHandler
    {
        [FormerlySerializedAs("uiItemType")] public ItemType itemType;
        public Image _image;
        public Image _imageBG;
        public DoubleClick _doubleClick;
        [HideInInspector] public int index;
        public bool filled;
        [HideInInspector] public string description;
        private GameObject itemObject;
        private IUIItemInventory _iUiItemInventory;
        private IGameFactory _gameFactory;
        public UnityEvent OnShowItem;
        private bool select;
        private Sprite _startSprite;
        private UIItem _uiItem;
        private IHudService _hudService;

        private void Start()
        {
            _iUiItemInventory = AllServices.Container.Single<IUIItemInventory>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _hudService = AllServices.Container.Single<IHudService>();
            _startSprite = _image.sprite;
            _iUiItemInventory.RegisterNewSlot(this);
            _doubleClick.OnDoubleClick += ShowItem;
        }

        private void OnDestroy()
        {
            if (_doubleClick)
            {
                _doubleClick.OnDoubleClick -= ShowItem;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (itemType == ItemType.Nothing)
                return;

            if (!select)
            {
                _imageBG.color = Color.green;
                _iUiItemInventory.Select(itemType);
                _doubleClick.Check();
                select = true;
            }
            else
            {
                Deselect();
            }
        }

        private void Deselect()
        {
            _imageBG.color = Color.white;
            _iUiItemInventory.Deselect(itemType);
            _doubleClick.Check();
            select = false;
        }

        private async void ShowItem()
        {
            if (itemType == ItemType.Nothing)
                return;

            if (!itemObject)
            {
                itemObject = await _gameFactory?.CreateUIItem(itemType, this.transform);
            }

            if (itemObject)
            {
                if(_hudService.GetState() != HudState.Empty)
                    return;
                _uiItem = itemObject.GetComponent<UIItem>();
                _uiItem.OnCloseObject += Deselect;
                itemObject.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                itemObject.SetActive(true);
                _hudService.ChangeState(HudState.PuzzleHudOpen);
                OnShowItem?.Invoke();
            }
        }


        public void Clear()
        {
            if (_uiItem)
                _uiItem.OnCloseObject -= Deselect;
            itemType = ItemType.Nothing;
            _image.sprite = _startSprite;
            _imageBG.color = Color.white;
            description = "";
            filled = false;
            itemObject = null;
        }
    }
}