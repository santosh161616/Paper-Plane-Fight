using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Image _playerImage;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _adText;

    [Space]
    [Header("UI")]
    [SerializeField] private Button _nextButton, _prevButton, _selectButton;
    [SerializeField] private Slider _healthSlider, _damageSlider, _speedSlider;

    [Space]
    [Header("Local Data")]
    [SerializeField] private PlayerSelectionData _selectedPlayer;
    [SerializeField] private int defaultIndex = 0;

    [Space]
    [Header("Players Data")]
    [SerializeField] private List<Player> _players;
    private int _currentPlayerIndex;
    [SerializeField] public int CurrentPLayerIndex { get { return _currentPlayerIndex; } set {  _currentPlayerIndex = value; } }

    void Start()
    {
        SetPlayerData(_selectedPlayer, defaultIndex);
        AddingListeners();
        SetAdData(0);
    }
    private void SetPlayerData(PlayerSelectionData selectionData, int index)
    {
        CurrentPLayerIndex = index;
        _playerImage.sprite = selectionData.playersData[index].playerImage;
        _playerName.text = selectionData.playersData[index].PlayerName;
        _adText.text = selectionData.playersData[index].adAlreadyShown.ToString() + "/" + selectionData.playersData[index].totalAds.ToString();
        _health.text = selectionData.playersData[index].Health.ToString();
        _healthSlider.value = selectionData.playersData[index].Health;
        _healthSlider.interactable = false;

        _damage.text = selectionData.playersData[index].Damage.ToString();
        _damageSlider.value = selectionData.playersData[index].Damage;
        _damageSlider.interactable = false;

        _speed.text = selectionData.playersData[index].Speed.ToString();
        _speedSlider.value = selectionData.playersData[index].Speed;
        _speedSlider.interactable = false;
    }

    public void NextSelect()
    {
        if (defaultIndex < _selectedPlayer.playersData.Count)
        {
            SetPlayerData(_selectedPlayer, defaultIndex);
            defaultIndex++;
        }
        else
        {
            defaultIndex = 0;
            SetPlayerData(_selectedPlayer, defaultIndex);
        }
    }

    public void SetAdData(int value)
    {
        for (int i = 0; i < _selectedPlayer.playersData.Count; i++)
        {
            if (!PlayerPrefs.HasKey("RemainingAd_" + i))
            {
                PlayerPrefs.SetInt("RemainingAd_" + i, value);                
            }
            _adText.text = PlayerPrefs.GetInt("RemainingAd_" + i) + "/" + _selectedPlayer.playersData[i].totalAds.ToString();
            Debug.Log(value + "Ad Value");
        }
    }

    public void PrevSelect()
    {
        if (defaultIndex < 0)
        {
            SetPlayerData(_selectedPlayer, defaultIndex);
            defaultIndex--;
        }
        else
        {
            defaultIndex = _selectedPlayer.playersData.Count - 1;
            SetPlayerData(_selectedPlayer, defaultIndex);
        }
    }
    
    public void SelectPlayer()
    {

    }

    void AddingListeners()
    {
        _nextButton.onClick.RemoveAllListeners();
        _nextButton.onClick.AddListener(() => { NextSelect(); });
        _prevButton.onClick.RemoveAllListeners();
        _prevButton.onClick.AddListener(() => { PrevSelect(); });
        _selectButton.onClick.RemoveAllListeners();
        _selectButton.onClick.AddListener(() => { PrevSelect(); });

    }
}