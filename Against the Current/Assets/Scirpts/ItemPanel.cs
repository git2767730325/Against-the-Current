using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemPanel : MonoBehaviour
{
    public static ItemPanel itemP;
    private BagPanel bp;
    public Button useBtn;
    public Button delBtn;
    public Button sellBtn;
    public Button closeBtn;

    public Grid grid;

    private void Awake()
    {
        itemP = this;
        delBtn.onClick.AddListener(DelItemBtn);
        useBtn.onClick.AddListener(UseItemBtn);
        sellBtn.onClick.AddListener(SellItemBtn);
        closeBtn.onClick.AddListener(CloseBtn);
        bp = transform.parent.GetComponent<BagPanel>();
        this.gameObject.SetActive(false);
    }

    private void DelItemBtn()
    {
        bp.DelItem(grid);
    }

    private void SellItemBtn()
    {
        bp.SellItem(grid);
    }   
    private void UseItemBtn()
    {
        bp.UseItem(grid);
    }
    private void CloseBtn()
    {
        grid.SetColor();
        this.gameObject.SetActive(false);
    }
}
