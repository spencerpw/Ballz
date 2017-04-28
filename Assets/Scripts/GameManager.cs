using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	public Row rowPrefab;
	public Tile tilePrefab;
	public ExtraBall extraBallPrefab;
	public GameObject blankPrefab;
	public Transform rowRoot;
	public Vector2 rowTop;
	public Vector2 rowShift;
	public TextMeshProUGUI levelLabel, finalLevel;
	public TextMeshProUGUI pointsLabel;
	public GameObject menuRoot;
	public GameObject gameRoot;
	public GameObject gameOverRoot;
	public GameObject pointPrefab;
	public GameObject fastforwardButton;
	public float fastForwardScale = 4f;
	public float fastForwardDelay = 5f;

	private int level;

	private int points;
	private List<Row> rows;

	private void Awake() {
		rows = new List<Row>();

		Messenger.AddListener("SpawnRow",SpawnRow);
		Messenger.AddListener("Point",AddPoint);
		//Messenger.AddListener("Shoot",DelayActivateFastForward);
		Messenger.AddListener("HideFastForward",HideFastForward);

	}

	private void Start() {
		ShowMainMenu();
	}

	public void FastForward() {
		Time.timeScale = fastForwardScale;
	}

	//private IEnumerator Delay

	private void HideFastForward() {
		fastforwardButton.SetActive(false);
	}

	public void NewGame(PointerEventData e) {
		Messenger.Broadcast("NewGame");

		menuRoot.SetActive(false);
		gameOverRoot.SetActive(false);
		gameRoot.SetActive(true);

		foreach(Row r in rows) {
			Destroy(r.gameObject);
		}

		rows.Clear();

		level = 1;
		points = 0;

		levelLabel.text = level.ToString();
		pointsLabel.text = points.ToString();

		SpawnRow();
	}

	public void GameOver() {
		gameOverRoot.SetActive(true);
		gameRoot.SetActive(false);
		menuRoot.SetActive(false);

		finalLevel.text = level.ToString();
	}

	public void SpawnRow() {
		Row row = Instantiate<Row>(rowPrefab);
		row.transform.SetParent(rowRoot,false);
		row.transform.localScale = Vector3.one;
		row.transform.localPosition = rowTop;
		rows.Add(row);

		PopulateRow(row);
		ShiftRows();
	}

	[ContextMenu("Spawn Rows")]
	public void SpawnRows() {
		for(int i = 0; i < 9; i++) {
			SpawnRow();
		}
	}

	private void ShiftRows() {
		Row deadRow = null;

		foreach(Row r in rows) {
			r.transform.localPosition = r.transform.localPosition + (Vector3)rowShift;

			if(r.transform.localPosition.y <= 70) {
				if(r.occupants.Count( o => o != null && o.name != "Blank") > 0)
					GameOver();
				else
					deadRow = r;
			}
		}

		if(deadRow != null) {
			rows.Remove(deadRow);
			Destroy(deadRow.gameObject);
		}

		level++;
		levelLabel.text = (level-1).ToString();
	}

	public void ShowMainMenu(PointerEventData e = null) {
		menuRoot.SetActive(true);
		gameOverRoot.SetActive(false);
		gameRoot.SetActive(false);
	}

	private void PopulateRow(Row row) {
		row.occupants = new List<GameObject>();
		List<Row.SlotType> slots = new List<Row.SlotType>();
		int tiles = Random.Range(1,7);
		slots.Add(Row.SlotType.BALL);

		for(int i = 0; i < tiles; i++) {
			if(i < tiles)
				slots.Add(Row.SlotType.TILE);
			else
				slots.Add(Row.SlotType.BLANK);
		}

		if(slots.Count < 7) {
			if(Random.value > 0.8f)
				slots.Add(Row.SlotType.POINT);
		}

		for(int i = slots.Count; i < 7; i++) {
				slots.Add(Row.SlotType.BLANK);
		}

		foreach(Row.SlotType t in slots.OrderBy( s => Random.value)) {
			GameObject go = null;
			switch(t) {
			case Row.SlotType.BALL:
				ExtraBall ball = Instantiate<ExtraBall>(extraBallPrefab);
				go = ball.gameObject;
				break;
			case Row.SlotType.BLANK:
				go = Instantiate<GameObject>(blankPrefab);
				go.name = "Blank";
				break;
			case Row.SlotType.POINT:
				go = Instantiate<GameObject>(pointPrefab);
				break;
			case Row.SlotType.TILE:
				Tile tile = Instantiate<Tile>(tilePrefab);
				tile.value = level;
				go = tile.gameObject;
				break;
			}

			if(go != null) {
				go.transform.SetParent(row.transform,false);
				go.transform.localScale = Vector3.one;
				row.occupants.Add(go);
			}
		}
	}

	private void AddPoint() {
		points++;
		pointsLabel.text = points.ToString();
	}
}
