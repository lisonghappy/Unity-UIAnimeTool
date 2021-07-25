# Unity-UIAnimeTool
A simple unity3d ui animation tool

## Screenshot
![Screenshot](https://github.com/lisonghappy/Unity-UIAnimeTool/blob/main/img.png) 

## How to use

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
