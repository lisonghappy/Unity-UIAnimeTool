 using UnityEngine;
using DG.Tweening;
using System;

namespace isong.UIAnime
{



    #region normal data

    [System.Serializable]
    public class UIStateAnimeAttribute
    {
        /// <summary>
        /// anime enable 
        /// </summary>
        public bool isEnableAnime = false;
        /// <summary>
        /// start value
        /// </summary>
        public Vector3 fromValue = Vector3.zero;
        /// <summary>
        /// stay  show value
        /// </summary>
        public Vector3 stayValue = Vector3.zero;

        /// <summary>
        /// exit value
        /// </summary>
        public Vector3 toValue = Vector3.zero;
    }



    [System.Serializable]
    public class UIAnimeAttribute
    {
        /// <summary>
        /// whether to use animation
        /// </summary> 
        public bool isUseAnime = false;
        /// <summary>
        /// sue Curve or use ease
        /// </summary>
        public bool isCurve = false;
        /// <summary>
        /// use lcoal duration
        /// </summary>
        public bool isUseLocalDuration = false;

        /// <summary>
        /// use enter anime prop 
        /// </summary>
        public bool isUseBackwards = true;

        /// <summary>
        /// anime's curve
        /// </summary>
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1f), new Keyframe(1, 1f));
        /// <summary>
        /// anime' s ease
        /// </summary>
        public Ease ease = Ease.Linear;
        /// <summary>
        /// local anime duration
        /// </summary>
        [Range(0f, 10f)] public float duration = 0.1f;

    }

    #endregion



    [RequireComponent(typeof(Transform))]
    public class UIAnimeTool : MonoBehaviour
    {

        #region property

        [SerializeField] private bool isEnableAnime = false;
        [Range(0f, 10f)] public float duration = 0.1f;

        public bool isEnableMask = false;



        //normal
        public UIStateAnimeAttribute posStateVal = new UIStateAnimeAttribute();
        public UIStateAnimeAttribute rotateStateVal = new UIStateAnimeAttribute();
        public UIStateAnimeAttribute scaleStateVal = new UIStateAnimeAttribute() { stayValue = Vector3.one };
        public UIStateAnimeAttribute alphaStateVal = new UIStateAnimeAttribute() { stayValue = Vector3.one };
        public UIStateAnimeAttribute maskStateVal = new UIStateAnimeAttribute() { stayValue = Vector3.one };

        //frome 
        public UIAnimeAttribute posFromAnimeAttribute;
        public UIAnimeAttribute rotateFromAnimeAttribute;
        public UIAnimeAttribute scaleFromAnimeAttribute;
        public UIAnimeAttribute alphaFromAnimeAttribute;
        public UIAnimeAttribute maskFromAnimeAttribute;

        //to
        public UIAnimeAttribute posToAnimeAttribute;
        public UIAnimeAttribute rotateToAnimeAttribute;
        public UIAnimeAttribute scaleToAnimeAttribute;
        public UIAnimeAttribute alphaToAnimeAttribute;
        public UIAnimeAttribute maskToAnimeAttribute;

        /// <summary>
        /// target of recttrans
        /// </summary>
        [SerializeField] private RectTransform rectTrans;
        public RectTransform targetRectTrans
        {
            get
            {
                if (rectTrans == null)
                    rectTrans = transform as RectTransform;
                return rectTrans;
            }
            set
            {
                rectTrans = value;
            }

        }

        private CanvasGroup _canvasGroup;
        protected CanvasGroup canvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    if (targetRectTrans != null)
                        _canvasGroup = targetRectTrans.GetComponent<CanvasGroup>();
                    else
                        return null;

                    if (_canvasGroup == null)
                        _canvasGroup = targetRectTrans.gameObject.AddComponent<CanvasGroup>();
                }

                return _canvasGroup;
            }
        }


        [SerializeField] private CanvasGroup maskCanvasGroup;
        [SerializeField] private bool isMaskblocksRaycasts = true;

        #endregion



        //=====================      Show UI Anime        =====================
        #region 
        private Sequence showSqueue;

        /// <summary>
        /// events processed before show
        /// </summary>
        public Action OnShowBefore;
        /// <summary>
        /// events processed after show
        /// </summary>
        public Action OnShowAfter;
        /// <summary>
        /// play show  anime
        /// </summary>
        public void Show()
        {

            if (targetRectTrans == null) return;
            if (showSqueue != null)
                showSqueue.Kill();
            if (hideSqueue != null)
                hideSqueue.Kill();


            // play anime
            showSqueue = DOTween.Sequence();

            if (OnShowBefore != null)
                OnShowBefore();

            #region // --------------      mask         --------------------
            var tweenMask = DoMaskAnime(true);
            if (tweenMask != null)
                showSqueue.Join(tweenMask);
            #endregion


            #region // --------------      pos         --------------------
            var tweenPos = DoTransformAnime(EAnimeType.pos, true);
            if (tweenPos != null)
                showSqueue.Join(tweenPos);
            #endregion


            #region // --------------      rotate         --------------------
            var tweenRotate = DoTransformAnime(EAnimeType.rotate, true);
            if (tweenRotate != null)
                showSqueue.Join(tweenRotate);
            #endregion


            #region  // --------------      scale         --------------------
            var tweenScale = DoTransformAnime(EAnimeType.scale, true);
            if (tweenScale != null)
                showSqueue.Join(tweenScale);
            #endregion

             
            #region // --------------      alpha         --------------------
            var tweenAlpha = DoAlphaAnime(true);
            if (tweenAlpha != null)
                showSqueue.Join(tweenAlpha);
            #endregion

            //show finished
            showSqueue.OnComplete(() => {
                if (canvasGroup != null)
                    canvasGroup.blocksRaycasts = true;
                if (OnShowAfter != null)
                    OnShowAfter();
            });

            showSqueue.Play();

        }
        #endregion




        //  =====================        Hide UI Anime        =====================
        #region

        private Sequence hideSqueue;
        /// <summary>
        /// events processed before hide
        /// </summary>
        public Action OnHideBefore;
        /// <summary>
        /// events processed after hide
        /// </summary>
        public Action OnHideAfter;
        /// <summary>
        /// play hide  anime
        /// </summary>
        public void Hide()
        {

            if (targetRectTrans == null) return;
            if (showSqueue != null)
                showSqueue.Kill();
            if (hideSqueue != null)
                hideSqueue.Kill();


            if (OnHideBefore != null)
                OnHideBefore();

            //set state
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = false;



            //play anime
            hideSqueue = DOTween.Sequence();


            #region // --------------      mask         --------------------
 
            var tweenMask = DoMaskAnime(false);
            if (tweenMask != null)
                showSqueue.Join(tweenMask);
            #endregion


            #region  //--------------      pos         --------------------
            var tweenPos = DoTransformAnime(EAnimeType.pos, false);
            if (tweenPos != null)
                hideSqueue.Join(tweenPos);
            #endregion


            #region //--------------      rotate         --------------------
            var tweenRotate = DoTransformAnime(EAnimeType.rotate, false);
            if (tweenRotate != null)
                hideSqueue.Join(tweenRotate);
            #endregion

             
            #region  //--------------      scale         --------------------
            var tweenScale = DoTransformAnime(EAnimeType.scale, false);
            if (tweenScale != null)
                hideSqueue.Join(tweenScale);
            #endregion

             
            #region    //--------------      alpha         --------------------
            var tweenAlpha = DoAlphaAnime(false);
            if (tweenAlpha != null)
                hideSqueue.Join(tweenAlpha);
            #endregion

            //hide finished
            hideSqueue.OnComplete(() => {
                if (tweenMask != null )
                    maskCanvasGroup.blocksRaycasts= false; 
                if (tweenPos == null)
                    targetRectTrans.localPosition = posStateVal.toValue;
                if (tweenRotate == null)
                    targetRectTrans.localRotation = Quaternion.Euler(rotateStateVal.toValue);
                if (tweenScale == null)
                    targetRectTrans.localScale = scaleStateVal.toValue;
                if (tweenAlpha == null)
                {
                    if (canvasGroup != null)
                        canvasGroup.alpha = alphaStateVal.toValue.x;
                }

                if (OnHideAfter != null)
                    OnHideAfter();
            });
        }

        #endregion



        //  =====================        Do Anime        =====================
        #region Do Anime

        private enum EAnimeType
        {
            pos,
            rotate,
            scale,
        }

        #region Do Mask Anime

        private Tween DoMaskAnime(bool isEnter = true)
        {
            Tweener tweener = null;
            
            if (!isEnableMask || maskCanvasGroup == null) return tweener;
             
            maskCanvasGroup.blocksRaycasts = isMaskblocksRaycasts;

            //get value
            float val = 0f;
            if (isEnter)
            {
                maskCanvasGroup.alpha = maskStateVal.fromValue.x;
                val = maskStateVal.stayValue.x;
            }
            else
                val = maskStateVal.toValue.x;
            //get attribute
            UIAnimeAttribute animeAttribute = null;

            if (isEnter)
                animeAttribute = maskFromAnimeAttribute;
            else
                animeAttribute = maskToAnimeAttribute;

            if (isEnableAnime && maskStateVal.isEnableAnime && animeAttribute.isUseAnime)
            {

                if (!isEnter && maskFromAnimeAttribute.isUseAnime &&animeAttribute.isUseBackwards)
                    animeAttribute = maskFromAnimeAttribute;

                //get duration
                var tempDurtion = 0f;
                if (animeAttribute.isUseLocalDuration)
                    tempDurtion = animeAttribute.duration;
                else
                    tempDurtion = duration;

                if (animeAttribute.isCurve)
                    tweener = maskCanvasGroup.DOFade(val, tempDurtion).SetEase(animeAttribute.curve);
                else
                    tweener = maskCanvasGroup.DOFade(val, tempDurtion).SetEase(animeAttribute.ease);
            }
            else
            {
                maskCanvasGroup.alpha = val;
            }
            return tweener;
        }

        #endregion

        #region Do Transform Anime
        /// <summary>
        /// do transform's pos/rotate/scale anime
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isEnter"></param>
        /// <returns></returns>
        private Tween DoTransformAnime(EAnimeType type, bool isEnter = true)
        {
            Tweener tweener = null;


            //get value  & attribute
            Vector3 val = Vector3.zero;
            UIAnimeAttribute animeAttribute = null;
            UIAnimeAttribute animeAttributeTemp = null;
            var isAnime = isEnableAnime;

            switch (type)
            {
                case EAnimeType.pos:
                    {
                        animeAttributeTemp = posFromAnimeAttribute;
                        if (isEnter)
                        {
                            targetRectTrans.localPosition = posStateVal.fromValue;
                            val = posStateVal.stayValue;
                            animeAttribute = posFromAnimeAttribute;
                        }
                        else
                        {
                            val = posStateVal.toValue;
                            animeAttribute = posToAnimeAttribute;
                        }
                        isAnime = isAnime && posStateVal.isEnableAnime;
                        break;
                    }
                case EAnimeType.rotate:
                    {
                        animeAttributeTemp = rotateFromAnimeAttribute;
                        if (isEnter)
                        {
                            targetRectTrans.localRotation = Quaternion.Euler(rotateStateVal.fromValue);
                            val = rotateStateVal.stayValue;
                            animeAttribute = rotateFromAnimeAttribute;
                        }
                        else
                        {
                            val = rotateStateVal.toValue;
                            animeAttribute = rotateToAnimeAttribute;
                        }
                        isAnime = isAnime && rotateStateVal.isEnableAnime;
                        break;
                    }
                case EAnimeType.scale:
                    {
                        animeAttributeTemp = scaleFromAnimeAttribute;
                        if (isEnter)
                        {
                            targetRectTrans.localScale = scaleStateVal.fromValue;
                            val = scaleStateVal.stayValue;
                            animeAttribute = scaleFromAnimeAttribute;
                        }
                        else
                        {
                            val = scaleStateVal.toValue;
                            animeAttribute = scaleToAnimeAttribute;
                        }
                        isAnime = isAnime && scaleStateVal.isEnableAnime;
                        break;
                    }
            }

            if (isAnime && animeAttribute.isUseAnime)
            {
                //if is exit and ,and  use fromVal
                if (!isEnter && animeAttributeTemp.isUseAnime && animeAttribute.isUseBackwards)
                    animeAttribute = animeAttributeTemp;

                //get duration
                float tempDurtion;
                if (animeAttribute.isUseLocalDuration)
                    tempDurtion = animeAttribute.duration;
                else
                    tempDurtion = duration;

                switch (type)
                {
                    case EAnimeType.pos:
                        if (animeAttribute.isCurve)
                            tweener = targetRectTrans.DOLocalMove(val, tempDurtion).SetEase(animeAttribute.curve);
                        else
                            tweener = targetRectTrans.DOLocalMove(val, tempDurtion).SetEase(animeAttribute.ease);
                        break;
                    case EAnimeType.rotate:
                        if (animeAttribute.isCurve)
                            tweener = targetRectTrans.DOLocalRotate(val, tempDurtion).SetEase(animeAttribute.curve);
                        else
                            tweener = targetRectTrans.DOLocalRotate(val, tempDurtion).SetEase(animeAttribute.ease);
                        break;
                    case EAnimeType.scale:
                        if (animeAttribute.isCurve)
                            tweener = targetRectTrans.DOScale(val, tempDurtion).SetEase(animeAttribute.curve);
                        else
                            tweener = targetRectTrans.DOScale(val, tempDurtion).SetEase(animeAttribute.ease);
                        break;
                }

            }
            else
            {
                if (isEnter)
                {
                    switch (type)
                    {
                        case EAnimeType.pos: targetRectTrans.localPosition = val; break;
                        case EAnimeType.rotate: targetRectTrans.localRotation = Quaternion.Euler(val); break;
                        case EAnimeType.scale: targetRectTrans.localScale = val; break;
                    }
                }
            }

            return tweener;
        }
        #endregion

        #region Do Alpha Anime
        private Tween DoAlphaAnime(bool isEnter = true)
        {
            Tweener tweener = null;

            if (canvasGroup == null)
                return tweener;

            //get value
            float val = 0f;
            if (isEnter)
            {
                canvasGroup.alpha = alphaStateVal.fromValue.x;
                val = alphaStateVal.stayValue.x;
            }
            else
                val = alphaStateVal.toValue.x;
            //get attribute
            UIAnimeAttribute animeAttribute = null;

            if (isEnter)
                animeAttribute = alphaFromAnimeAttribute;
            else
                animeAttribute = alphaToAnimeAttribute;

            if (isEnableAnime && alphaStateVal.isEnableAnime && animeAttribute.isUseAnime)
            {

                if (!isEnter && alphaFromAnimeAttribute.isUseAnime && animeAttribute.isUseBackwards)
                    animeAttribute = alphaFromAnimeAttribute;

                //get duration
                var tempDurtion = 0f;
                if (animeAttribute.isUseLocalDuration)
                    tempDurtion = animeAttribute.duration;
                else
                    tempDurtion = duration;

                if (animeAttribute.isCurve)
                    tweener = canvasGroup.DOFade(val, tempDurtion).SetEase(animeAttribute.curve);
                else
                    tweener = canvasGroup.DOFade(val, tempDurtion).SetEase(animeAttribute.ease);
            }
            else
            {
                if (isEnter) canvasGroup.alpha = val;
            }
            return tweener;

        }
        #endregion


        #endregion

    }
}