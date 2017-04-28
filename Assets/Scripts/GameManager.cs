using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour {
	public Row rowPrefab;
	public Tile tilePrefab;
	public ExtraBall extraBallPrefab;
	public GameObject blankPrefab;
	public Transform rowRoot;
	public Vector2 rowTop;
	public Vector2 rowShift;
	public TextMeshProUGUI levelLabel;
	public TextMeshProUGUI pointsLabel;

	private int level;

	private int points;
	private List<Row> rows;

	private void Awake() {
		rows = new List<Row>();

		Messenger.AddListener("SpawnRow",SpawnRow);
	}

	private void Start() {
		level = 1;

		SpawnRow();
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

			Debug.Log(r.transform.localPosition.y);
			if(r.transform.localPosition.y <= 70) {
				if(r.occupants.Count( o => o != null) > 0)
					Messenger.Broadcast("GameOver");
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

	private void PopulateRow(Row row) {
		row.occupants = new List<GameObject>();
		List<Row.SlotType> slots = new List<Row.SlotType>();
		int tiles = Random.Range(1,7);
		slots.Add(Row.SlotType.BALL);

		for(int i = 0; i < 6; i++) {
			if(i < tiles)
				slots.Add(Row.SlotType.TILE);
			else
				slots.Add(Row.SlotType.BLANK);
		}

		if(slots.Count < 7) {
			if(Random.value > 0.5f)
				slots.Add(Row.SlotType.POINT);
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
				break;
			case Row.SlotType.POINT:

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
}
