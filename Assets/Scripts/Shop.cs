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

    [Space]
    [Header("UI")]
    [SerializeField] private Button _nextButton, _prevButton;
    [SerializeField] private Slider _healthSlider, _damageSlider, _speedSlider;

    [Space]
    [Header("Local Data")]
    [SerializeField] private PlayerSelectionData _selectedPlayer;
    [SerializeField] private int defaultIndex = 0;

    void Start()
    {
        SelectPlayer(_selectedPlayer, defaultIndex);
        AddingListeners();
    }
    private void SelectPlayer(PlayerSelectionData selectionData, int index)
    {
        _playerImage.sprite = selectionData.playersData[index].playerImage;
        _playerName.text = selectionData.playersData[index].PlayerName;
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
        if (defaultIndex != _selectedPlayer.playersData.Count)
        {
            SelectPlayer(_selectedPlayer, defaultIndex);
            defaultIndex++;
        }
        else
        {
            defaultIndex = 0;
            SelectPlayer(_selectedPlayer, defaultIndex);
        }
    }

    public void PrevSelect()
    {
        if (defaultIndex < 0)
        {
            SelectPlayer(_selectedPlayer, defaultIndex);
            defaultIndex--;
        }
        else
        {
            defaultIndex = _selectedPlayer.playersData.Count - 1;
            SelectPlayer(_selectedPlayer, defaultIndex);
        }
    }

    void AddingListeners()
    {
        _nextButton.onClick.RemoveAllListeners();
        _nextButton.onClick.AddListener(() => { NextSelect(); });
        _prevButton.onClick.RemoveAllListeners();
        _prevButton.onClick.AddListener(() => { PrevSelect(); });
    }
}