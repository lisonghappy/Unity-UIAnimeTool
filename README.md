<!--
 * @Descripttion: 
 * @version: 
 * @Author: risu
 * @Date: 2021-07-30 14:06:50
 * @LastEditors: sueRimn
 * @LastEditTime: 2021-08-13 16:37:23
-->
# Unity-UIAnimeTool
A simple unity3d ui animation tool.

[github page](https://github.com/lisonghappy/Unity-UIAnimeTool)


<img src="https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/demo_anime_show.gif" alt="Demo anime show" />


#### Unity Version
        2017.4.40


## Screenshot

tool panel.

![Screenshot](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img.png) 

position prop:

![position anime prop](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img-position.png) 

scele prop:

![scale prop](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img-scale.png) 

rotate prop:

![rotate prop](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img-rotate.png) 

panel alpaha prop:

![panel alpha prop](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img-panel_alpha.png) 

mask prop:

![panel mask prop](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/Img/img-panel_mask.png) 


## All Attributes

```c#
        /// <summary>
        /// play show  anime
        /// </summary>
        public void Show(){}
        
        /// <summary>
        /// play hide  anime
        /// </summary>
        public void Hide(){}




 
        public bool isUseAnime = false;//whether to use animation
 
        public RectTransform targetRectTrans{get;set}// target of recttrans
        

        public Action OnShowBefore;//events processed before show
        public Action OnShowAfter;//events processed after show
 
        public Action OnHideBefore;//events processed before hide
        public Action OnHideAfter;//events processed after hide

```

## How to use
For details, please see the ***DEMO*** scene.


## Dependencies
[DoTween](https://github.com/Demigiant/dotween)