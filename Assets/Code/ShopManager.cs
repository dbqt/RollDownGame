using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {

	public Material active;
	public Material[] originals;

	// Use this for initialization
	void Start () {
		SetMaterial(PlayerPrefs.GetInt("ActiveMaterial"));
		PlayerPrefs.SetInt("mat0", 1);
		PlayerPrefs.SetInt("mat1", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonShopClick(int index)
	{
		//check if unlocked
		if(PlayerPrefs.GetInt("mat"+index) == 1)
		{
			SetMaterial(index);
		}
		else 
		{
			int total = PlayerPrefs.GetInt("TotalPoints");
			switch(index)
			{
				case 2:{
					if(total >= 100)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-100);
					}
					break;
				}
				case 3:{
					if(total >= 100)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-100);
					}
					break;
				}
				case 4:{
					if(total >= 200)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-200);
					}
					break;
				}
				case 5:{
					if(total >= 200)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-200);
					}
					break;
				}
				case 6:{
					if(total >= 1000)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-1000);
					}
					break;
				}
				case 7:{
					if(total >= 1000)
					{
						SetMaterial(index);
						PlayerPrefs.SetInt("mat"+index, 1);
						PlayerPrefs.SetInt("TotalPoints", total-1000);
					}
					break;
				}
				default:
				break;
			}
		}
		//else denied
	}

	public void SetMaterial(int index)
	{
		active.color = originals[index].color;
		PlayerPrefs.SetInt("ActiveMaterial", index);
		this.gameObject.GetComponent<UIManager>().SetShopButtons();
	}
}
