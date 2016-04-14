using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image containerImage;
	public Image receivingImage;
	private Color normalColor;
	public Color highlightColor = Color.yellow;
	
	public void OnEnable ()
	{
		if (containerImage != null)
			normalColor = containerImage.color;
	}
	
	public void OnDrop(PointerEventData data)
	{
		string item;
		item = data.selectedObject.name;
		var player = GameObject.FindGameObjectWithTag ("Player");
		var psc = player.GetComponent<BaseCharacter>(); 
		var inv = psc.inventory;
		int idx=0; 
		int.TryParse(item.Substring (7),out idx);
		var slot = gameObject.GetComponent<EquipSlotType> ();
		if (slot.ItemType != inv [idx].type) 
			return;
		containerImage.color = normalColor;
		
		if (receivingImage == null)
			return;
		
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			receivingImage.overrideSprite = dropSprite;
		//equip item
		GameObject wprefab = Resources.Load (inv[idx].prefab) as GameObject;
		GameObject weapon = Instantiate (wprefab) as GameObject;
		foreach (Transform t in player.GetComponentsInChildren<Transform>()) {
			if (t.name == inv [idx].attach) { //"weapon_target_side.R_end"
				weapon.transform.SetParent (t);
				weapon.transform.localPosition = inv[idx].offset;//new Vector3(9.2f, -9.9f,-12.4f);
				weapon.transform.localRotation = Quaternion.Euler (inv[idx].rot);
				weapon.transform.localScale = inv[idx].scale;//new Vector3(1,7f,7);
			}
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			containerImage.color = highlightColor;
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		containerImage.color = normalColor;
	}
	
	private Sprite GetDropSprite(PointerEventData data)
	{
		var originalObj = data.pointerDrag;
		if (originalObj == null)
			return null;

		var srcImage = originalObj.GetComponent<Image>();
		if (srcImage == null)
			return null;
		
		return srcImage.sprite;
	}
}
