using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts;
using EGL = UnityEditor.EditorGUILayout;

namespace Assets.Editor
{
    public class Slide_Wizard : EditorWindow
    {
        internal SlideManager sMan;
        internal Vector2 SlideListPos;
        internal Vector2 SlideEditorPos;
        internal Slide currSlide;
        [MenuItem("Window/Slide Wizard")]
        static void Init()
        {
            Slide_Wizard window = EditorWindow.GetWindow<Slide_Wizard>();
            window.titleContent = new GUIContent("Slide Wizard");
            window.sMan = SlideManager.Instance;
            window.Show();
        }

        private void OnGUI()
        {
            if (sMan == null)
            {
                Debug.Log("Slide manager not found");
                sMan = SlideManager.Instance;
                return;
            }
            using (new HorizontalGroup())
            {
                using (var sg = new ScrollGroup(SlideListPos, new[] { GUILayout.Width(256f) }))
                {
                    SlideListPos = sg.pos;
                    using (new VerticalGroup())
                    {
                        for (int i = 0; i < (sMan.Slides??(sMan.Slides = new List<Slide>())).Count; i++)
                        {
                            using (new HorizontalGroup())
                            {
                                EGL.LabelField((i + 1) + ")", GUILayout.Width(15f));
                                if (GUILayout.Button(sMan.Slides[i].Title.text))
                                {
                                    currSlide = sMan.Slides[i];
                                }
                                if (GUILayout.Button("↑"))
                                {
                                    MoveStep(i, true);
                                    break;
                                }
                                if (GUILayout.Button("↓"))
                                {
                                    MoveStep(i, false);
                                    break;
                                }
                                if (GUILayout.Button("X"))
                                {
                                    sMan.Slides[i] = null;
                                    DestroyImmediate(sMan.SlideObjects[i]);
                                }
                            }
                        }
                        if (GUILayout.Button("+"))
                        {
                            currSlide = sMan.AddSlide();
                        }
                    } 
                }
                sMan.SlideObjects.RemoveAll(o => o == null);
                sMan.Slides.RemoveAll(s => s == null);
                if (currSlide)
                {
                    using (new VerticalGroup())
                    {
                        EGL.LabelField("Title");
                        currSlide.Title.text = EGL.TextArea(currSlide.Title.text);
                        EGL.Space();
                        EGL.LabelField("Body");
                        using (var sg = new ScrollGroup(SlideEditorPos, new[] {GUILayout.MaxHeight(256f)}))
                        {
                            SlideEditorPos = sg.pos;
                            currSlide.Body.text = EGL.TextArea(currSlide.Body.text);
                        }
                        currSlide.Bg.sprite = EGL.ObjectField(currSlide.Bg.sprite, typeof(Sprite)) as Sprite;
                        currSlide.Bg.color = EGL.ColorField("Background image color", currSlide.Bg.color);
                        currSlide.Title.color = EGL.ColorField("Title Color", currSlide.Title.color);
                        currSlide.Body.color = EGL.ColorField("Body Color", currSlide.Body.color);

                    } 
                }
            }
            if (GUI.changed)
            {
                Undo.RegisterCompleteObjectUndo(sMan, "Slide change");
                EditorUtility.SetDirty(sMan);
            }
        }

        private void MoveStep(int step, bool isUp)
        {
            var toMoveObj = sMan.SlideObjects[step];
            var toMoveSlide = sMan.Slides[step];
            sMan.SlideObjects.RemoveAt(step);
            sMan.Slides.RemoveAt(step);
            if (isUp)
            {
                sMan.SlideObjects.Insert(step - 1 >= 0 ? step - 1 : 0, toMoveObj);
                sMan.Slides.Insert(step - 1 >= 0 ? step - 1 : 0, toMoveSlide);
                toMoveObj.transform.SetSiblingIndex(step - 1 >= 0 ? step - 1 : 0);
            }
            else
            {
                sMan.SlideObjects.Insert(step + 1 < sMan.SlideObjects.Count ? step + 1 : sMan.SlideObjects.Count, toMoveObj);
                sMan.Slides.Insert(step + 1 < sMan.Slides.Count ? step + 1 : sMan.Slides.Count, toMoveSlide);
                toMoveObj.transform.SetSiblingIndex(step + 1 < sMan.Slides.Count ? step + 1 : sMan.Slides.Count);
            }
        }
    }
}
