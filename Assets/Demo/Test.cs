using isong.UIAnime;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Transform panelTrans;
    private UIAnimeTool uiAnime;

    private Button openBtn;
    private Button closeBtn;



    void Start()
    {
        panelTrans = transform.Find("Panel");
        uiAnime = panelTrans.GetComponent<UIAnimeTool>();
        openBtn = transform.Find("ButtonOpen").GetComponent<Button>();
        closeBtn = panelTrans.Find("ButtonClose").GetComponent<Button>();


        //set anime target
        uiAnime.targetRectTrans = panelTrans as RectTransform;
        //anime show before action
        uiAnime.OnShowBefore += () => { if (!panelTrans.gameObject.activeSelf) panelTrans.gameObject.SetActive(true); };
        //anime show after action
        uiAnime.OnShowAfter += () => { openBtn.gameObject.SetActive(false); };
        //anime hide after action
        uiAnime.OnHideAfter+= () => { openBtn.gameObject.SetActive(true); };

        
        //show anime
        openBtn.onClick.AddListener(uiAnime.Show);
        //hide anime
        closeBtn.onClick.AddListener(uiAnime.Hide);
        
    }

 
}
