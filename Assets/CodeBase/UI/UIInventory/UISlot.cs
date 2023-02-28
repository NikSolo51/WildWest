using CodeBase.Infrastructure.Factory;
using CodeBase.Inventory;
using CodeBase.Logic;
using CodeBase.Services.Hud;
using CodeBase.UI.UIInventory.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.UIInventory
{
    public class UISlot : MonoBehaviour, IPointerDownHandler
    {
        public ItemType _itemType;
        public Image _image;
        public Image _imageBG;
        public DoubleClick _doubleClick;
        [HideInInspector] public int index;
        public bool filled;
        [HideInInspector] public string description;
        private GameObject itemObject;
        private UIItem _uiItem;
        private bool select;
        private Sprite _startSprite;
        private IHudService _hudService;
        private IGameFactory _gameFactory;
        private IUIItemInventory _iUiItemInventory;
        public UnityEvent OnShowItem;

        [Inject]
        public void Construct(IHudService hudService, IGameFactory gameFactory, IUIItemInventory iUiItemInventory)
        {
            _hudService = hudService;
            _gameFactory = gameFactory;
            _iUiItemInventory = iUiItemInventory;
            _iUiItemInventory.RegisterNewSlot(this);
        }

        private void Start()
        {
            _startSprite = _image.sprite;
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
            if (_itemType == ItemType.Nothing)
                return;

            if (!select)
            {
                if (_uiItem)
                    if (eventData.pointerEnter.name == _uiItem.name)
                        return;
                _imageBG.color = Color.green;
                _iUiItemInventory.Select(_itemType);
                _doubleClick.StartDetect();
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
            _iUiItemInventory.Deselect(_itemType);
            _doubleClick.StartDetect();
            select = false;
        }

        private async void ShowItem()
        {
            if (_itemType == ItemType.Nothing)
                return;

            if (!itemObject)
            {
                itemObject = await _gameFactory?.CreateUIItem(_itemType, this.transform);
            }

            if (itemObject)
            {
                if (_hudService.GetState() != HudState.Empty)
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
            {
                _uiItem.OnCloseObject -= Deselect;
            }

            _itemType = ItemType.Nothing;
            _image.sprite = _startSprite;
            _imageBG.color = Color.white;
            description = "";
            filled = false;
            itemObject = null;
        }
    }
}