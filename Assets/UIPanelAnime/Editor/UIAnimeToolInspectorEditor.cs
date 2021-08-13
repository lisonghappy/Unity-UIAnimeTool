#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace isong.UIAnime
{

    [CustomEditor(typeof(UIAnimeTool), true)]
    public class UIAnimeToolInspectorEditor : Editor
    {

        private string Version = "0.0.2";

        private Texture2D icon;
        private GUIStyle toolbarStyle;



        private static int isSelectedAnimeType = 0;
        private static string[] animeTypeToolbarStr = new string[] {
            "<color=#BF1200>Position</color>",
            "<color=#21BE00>Rotation</color>",
            "<color=#0038BE>Scale</color>",
            "<color=#E5CA00>Alpha</color>",
            "Mask"
        };
        private List<string> animeTypeToolbarStrTemp = new List<string>(animeTypeToolbarStr);



        private UIAnimeTool uiAnimeTool;

        private SerializedProperty targetRectTrans;

        private SerializedProperty isEnableAnime;
        private SerializedProperty duration;

        private SerializedProperty isEnableMask;
        private SerializedProperty maskCanvasGroup;
        private SerializedProperty isMaskblocksRaycasts;
        



        //normal prop 
        private SerializedProperty alphaStateVal;
        private SerializedProperty posStateVal;
        private SerializedProperty scaleStateVal;
        private SerializedProperty rotateStateVal;

        //mask
        private SerializedProperty maskStateVal;

        //enter prop
        private SerializedProperty posFromAnimeAttribute;
        private SerializedProperty scaleFromAnimeAttribute;
        private SerializedProperty rotateFromAnimeAttribute;
        private SerializedProperty alphaFromAnimeAttribute;
        private SerializedProperty maskFromAnimeAttribute;

        //exit prop
        private SerializedProperty posToAnimeAttribute;
        private SerializedProperty scaleToAnimeAttribute;
        private SerializedProperty rotateToAnimeAttribute;
        private SerializedProperty alphaToAnimeAttribute;
        private SerializedProperty maskToAnimeAttribute;


        private void OnEnable()
        {
            icon = Resources.Load<Texture2D>("uianimetool_icon");

            uiAnimeTool = target as UIAnimeTool;

            targetRectTrans = serializedObject.FindProperty("rectTrans");

            isEnableAnime = serializedObject.FindProperty("isEnableAnime");
            duration = serializedObject.FindProperty("duration");
            isEnableMask = serializedObject.FindProperty("isEnableMask");
            maskCanvasGroup = serializedObject.FindProperty("maskCanvasGroup");
            isMaskblocksRaycasts = serializedObject.FindProperty("isMaskblocksRaycasts");


            posStateVal = serializedObject.FindProperty("posStateVal");
            rotateStateVal = serializedObject.FindProperty("rotateStateVal");
            scaleStateVal = serializedObject.FindProperty("scaleStateVal");
            alphaStateVal = serializedObject.FindProperty("alphaStateVal");
            maskStateVal = serializedObject.FindProperty("maskStateVal");


            posFromAnimeAttribute = serializedObject.FindProperty("posFromAnimeAttribute");
            rotateFromAnimeAttribute = serializedObject.FindProperty("rotateFromAnimeAttribute");
            scaleFromAnimeAttribute = serializedObject.FindProperty("scaleFromAnimeAttribute");
            alphaFromAnimeAttribute = serializedObject.FindProperty("alphaFromAnimeAttribute");
            maskFromAnimeAttribute = serializedObject.FindProperty("maskFromAnimeAttribute");

            posToAnimeAttribute = serializedObject.FindProperty("posToAnimeAttribute");
            rotateToAnimeAttribute = serializedObject.FindProperty("rotateToAnimeAttribute");
            scaleToAnimeAttribute = serializedObject.FindProperty("scaleToAnimeAttribute");
            alphaToAnimeAttribute = serializedObject.FindProperty("alphaToAnimeAttribute");
            maskToAnimeAttribute = serializedObject.FindProperty("maskToAnimeAttribute");

            isEnableMaskVal = isEnableMask.boolValue;
            ResetToolbarStrValue();

        }

        private bool isEnableMaskVal = false;
        private GUIStyle labelStyle;
        private GUIStyle linkBtnStyle;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            #region title info
            this.labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            this.linkBtnStyle = new GUIStyle("IconButton") { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fixedWidth = 50f};
            GUILayout.BeginHorizontal();
            GUILayout.Label(icon, labelStyle);
            if (GUILayout.Button(Version, linkBtnStyle)) { OpenLink(); }
            GUILayout.EndHorizontal();
            Space(30);
            #endregion



            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            Space(10);
            EditorGUILayout.PropertyField(targetRectTrans, new GUIContent("Target RectTrans", "When targetRectTrans is null, use the current object as the value of targetRectTrans"));
            Space(10);
            EditorGUILayout.PropertyField(isEnableAnime, new GUIContent("Enable Animation", "Whether to enable animation"));
            EditorGUILayout.EndVertical();
            Space(10);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(isEnableMask); 
            Space(5);
            if (isEnableMask.boolValue) { 
                EditorGUILayout.PropertyField(maskCanvasGroup);
                EditorGUILayout.PropertyField(isMaskblocksRaycasts);
            }
            EditorGUILayout.EndVertical();
                

            Space(10);

            if (isEnableMaskVal != isEnableMask.boolValue) { 
                ResetToolbarStrValue();
                isEnableMaskVal = isEnableMask.boolValue;
                if (isSelectedAnimeType == 4)
                    isSelectedAnimeType = 0;
            }



            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            toolbarStyle = new GUIStyle("AppToolbarButtonMid") { richText = true, fixedHeight = 30f, stretchWidth = true, fontSize = 15 };
            isSelectedAnimeType = GUILayout.Toolbar(isSelectedAnimeType, animeTypeToolbarStrTemp.ToArray(), toolbarStyle);

            switch (isSelectedAnimeType)
            {
                case 0: DrawUIAnime(GetEnableAnimeSP(posStateVal), posStateVal, posFromAnimeAttribute, posToAnimeAttribute, Color.red, isSelectedAnimeType); break;
                case 1: DrawUIAnime(GetEnableAnimeSP(rotateStateVal), rotateStateVal, rotateFromAnimeAttribute, rotateToAnimeAttribute, Color.green, isSelectedAnimeType); break;
                case 2: DrawUIAnime(GetEnableAnimeSP(scaleStateVal), scaleStateVal, scaleFromAnimeAttribute, scaleToAnimeAttribute, Color.blue, isSelectedAnimeType); break;
                case 3: DrawUIAnime(GetEnableAnimeSP(alphaStateVal), alphaStateVal, alphaFromAnimeAttribute, alphaToAnimeAttribute, Color.yellow, isSelectedAnimeType); break;
                case 4: DrawUIAnime(GetEnableAnimeSP(maskStateVal), maskStateVal, maskFromAnimeAttribute, maskToAnimeAttribute, Color.white, isSelectedAnimeType); break;
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

        }


        private void ResetToolbarStrValue() {

            animeTypeToolbarStrTemp = new List<string>();
            for (int i = 0; i < animeTypeToolbarStr.Length; i++)
            {
                var isState = false;
                switch (i)
                {
                    case 0: isState = ISEnableAnime(posStateVal); break;
                    case 1: isState = ISEnableAnime(rotateStateVal); break;
                    case 2: isState = ISEnableAnime(scaleStateVal); break;
                    case 3: isState = ISEnableAnime(alphaStateVal); break;
                    case 4: isState = ISEnableAnime(maskStateVal); break;
                }

                if (i != 4 || isEnableMask.boolValue) 
                    animeTypeToolbarStrTemp.Add(SetToolBarText(isState, i));
                
            }
        }


        /// <summary>
        /// draw ui anime
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="enterAnimeAttribute"></param>
        /// <param name="exitAnimeAttribute"></param>
        /// <param name="bgColor"></param>
        /// <param name="index"></param>
        private void DrawUIAnime(SerializedProperty isEnable, SerializedProperty normalAttribute, SerializedProperty enterAnimeAttribute, SerializedProperty exitAnimeAttribute, Color bgColor, int index)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = bgColor;

            var isEnableVal = isEnable.boolValue;

            //normal ui
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawNormalAttribute(normalAttribute);


            EditorGUILayout.EndVertical();
            Space(10);
            if (!isEnableAnime.boolValue)
            {
                EditorGUILayout.EndVertical();
                return;
            }

            GUI.backgroundColor = Color.white;
            Space(10);
            var isDurationState = ISEnableAnime(posStateVal) || ISEnableAnime(rotateStateVal) || ISEnableAnime(scaleStateVal) 
                                                || ISEnableAnime(alphaStateVal) || ISEnableAnime(maskStateVal);
            EditorGUI.BeginDisabledGroup(!isDurationState);
            EditorGUILayout.PropertyField(duration);
            EditorGUI.EndDisabledGroup();
            Space(10);
            GUI.backgroundColor = bgColor;


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //et toolbar show string
            EditorGUILayout.PropertyField(isEnable, new GUIContent("Enable Anime"));
            animeTypeToolbarStrTemp[index] = SetToolBarText(isEnableVal, index);

            EditorGUI.BeginDisabledGroup(!isEnableVal);

            //draw enter property
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawUIAnimeAttribute(enterAnimeAttribute, true);
            EditorGUILayout.EndVertical();

            Space(5f);

            //draw exit property
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawUIAnimeAttribute(exitAnimeAttribute, false);
            EditorGUILayout.EndVertical();

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUI.backgroundColor = Color.white;

        }




        /// <summary>
        ///  draw normal ui  no anime attribute
        /// </summary>
        /// <param name="property"></param>
        private void DrawNormalAttribute(SerializedProperty property)
        {
            EditorGUILayout.LabelField("Normal", GUILayout.ExpandWidth(false));

            var isSpecial = property == alphaStateVal || property == maskStateVal;
            var isScale = property == scaleStateVal;
            var fromValueStr = "fromValue";
            var stayValueStr = "stayValue";
            var toValueStr = "toValue";
            if (isSpecial)
            {
                fromValueStr += ".x";
                stayValueStr += ".x";
                toValueStr += ".x";
            }

            EditorGUILayout.BeginHorizontal();
            Space();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative(fromValueStr), new GUIContent("from"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(stayValueStr), new GUIContent("stay"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(toValueStr), new GUIContent("to"));
            EditorGUILayout.EndVertical();


            if (GUILayout.Button("Reset", GUILayout.Height(64f)))
            {
                if (isSpecial)
                {
                    property.FindPropertyRelative(fromValueStr).floatValue = 0f;
                    property.FindPropertyRelative(stayValueStr).floatValue = 1f;
                    property.FindPropertyRelative(toValueStr).floatValue = 0f;
                }
                else
                {
                    var val = Vector3.zero;
                    if (isScale) val = Vector3.one;

                    property.FindPropertyRelative(fromValueStr).vector3Value = Vector3.zero;
                    property.FindPropertyRelative(stayValueStr).vector3Value = val;
                    property.FindPropertyRelative(toValueStr).vector3Value = Vector3.zero;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw ui  anime attribute
        /// </summary>
        /// <param name="animeProperty"></param>
        /// <param name="isFrom"></param>
        /// <param name="isVectorVal"></param>
        private void DrawUIAnimeAttribute(SerializedProperty animeProperty, bool isFrom)
        {
            if (animeProperty == null) return;


            Space(5);
            var isUseAnime = animeProperty.FindPropertyRelative("isUseAnime");
            var useAnimeDisplayName = "";
            if (isFrom) 
                useAnimeDisplayName  = "From"; 
            else 
                useAnimeDisplayName = "To";
            EditorGUILayout.PropertyField(isUseAnime, new GUIContent(useAnimeDisplayName));
            if (isUseAnime.boolValue)
            {
                var isShow = true;
                Space(5);
                if (!isFrom)
                {
                    var isUseBackwards = animeProperty.FindPropertyRelative("isUseBackwards");
                    EditorGUILayout.PropertyField(isUseBackwards, new GUIContent("Use Backward"));
                    isShow = !isUseBackwards.boolValue;
                }

                if (isShow)
                {
                    var isCurve = animeProperty.FindPropertyRelative("isCurve");
                    var isUseLocalDuration = animeProperty.FindPropertyRelative("isUseLocalDuration");


                    var curve = animeProperty.FindPropertyRelative("curve");
                    var ease = animeProperty.FindPropertyRelative("ease");
                    var duration = animeProperty.FindPropertyRelative("duration");



                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(isCurve, new GUIContent("Use Curve"));
                    if (isCurve.boolValue)
                        EditorGUILayout.PropertyField(curve, new GUIContent(), GUILayout.ExpandWidth(true));
                    else
                        EditorGUILayout.PropertyField(ease, new GUIContent(), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();



                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(isUseLocalDuration, new GUIContent("Use  Local Duration"));
                    if (isUseLocalDuration.boolValue)
                        EditorGUILayout.PropertyField(duration, new GUIContent(), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                }
            }
        }


        private SerializedProperty GetEnableAnimeSP(SerializedProperty stateVal)
        {
            return stateVal.FindPropertyRelative("isEnableAnime");
        }

        /// <summary>
        /// get state enable anime state
        /// </summary>
        /// <param name="stateVal"></param>
        /// <returns></returns>
        private bool ISEnableAnime(SerializedProperty stateVal)
        {
            return GetEnableAnimeSP(stateVal).boolValue;
        }

        /// <summary>
        /// set tool bar show info state
        /// </summary>
        /// <param name="isSelested"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string SetToolBarText(bool isSelested, int index)
        {
            string str = "";
            for (int i = 0; i < animeTypeToolbarStr.Length; i++)
            {
                if (index == i)
                {
                    if (isSelested)
                        str = animeTypeToolbarStr[i] + "☑";
                    else
                        str = animeTypeToolbarStr[i];
                    break;
                }
            }

            return str;
        }


        /// <summary>
        /// draw space
        /// </summary>
        /// <param name="size"></param>
        private void Space(float size = 0f)
        {
            EditorGUILayout.Space(size);
        }


        [MenuItem("CONTEXT/" + nameof(UIAnimeTool) + "/Open Soure Link")]
        private static void OpenLink()
        {

            Application.OpenURL("https://github.com/lisonghappy/Unity-UIAnimeTool");

        }
    }
}


#endif