using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resident;

public class MenuSettings: MonoBehaviour {
	[SerializeField]
	private GameObject _menuPanel;
	[SerializeField]
	private Resident.CharacterManager _characterManager;
	[SerializeField]
	private Slider _maxResident;
	[SerializeField]
	private Text _textMax;
	[SerializeField]
	private Slider _hunterResident;
	[SerializeField]
	private Text _textHunter;
	[SerializeField]
	private Slider _clashResident;
	[SerializeField]
	private Text _textClash;
	[SerializeField]
	private Slider _timeResident;
	[SerializeField]
	private Text _textTime;

	public void StartGame() {
		Dictionary<Type, int> chance = new Dictionary<Type, int>();
		chance.Add(Type.Passive, 75);
		chance.Add(Type.RandomClash, (int)_clashResident.value);
		chance.Add(Type.RandomTime, (int)_timeResident.value);
		chance.Add(Type.Hunter, (int)_hunterResident.value);
		_characterManager.SpawnCharacter((int)_maxResident.value, chance);
		_menuPanel.SetActive(false);
	}

	public void ChangeMax() {
		var t = _textMax.text.Split(':');
		_textMax.text = t[0] + ": " + _maxResident.value;
	}

	public void ChangeHunter() {
		var t = _textHunter.text.Split(':');
		_textHunter.text = t[0] + ": " + _hunterResident.value;
	}

	public void ChangeClash() {
		var t = _textClash.text.Split(':');
		_textClash.text = t[0] + ": " + _clashResident.value;
	}

	public void ChangeTime() {
		var t = _textTime.text.Split(':');
		_textTime.text = t[0] + ": " + _timeResident.value;
	}

	public void ExitGame() {
		Application.Quit();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			_menuPanel.SetActive(!_menuPanel.activeSelf);
		}
	}
}