<!--
 * @Descripttion: 
 * @version: 
 * @Author: sueRimn
 * @Date: 2021-07-30 14:06:50
 * @LastEditors: sueRimn
 * @LastEditTime: 2021-07-30 14:35:34
-->
# Unity-UIAnimeTool
A simple unity3d ui animation tool

## Screenshot
![Screenshot](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/img.png) 

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
