using System;
using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;

namespace Assets.Editor
{
    public class HorizontalGroup : IDisposable
    {
        public HorizontalGroup(GUILayoutOption[] options = null)
        {
            EGL.BeginHorizontal(options);
        }
        public void Dispose()
        {
            EGL.EndHorizontal();
        }
    }

    public class VerticalGroup : IDisposable
    {
        public VerticalGroup(GUILayoutOption[] options = null)
        {
            EGL.BeginVertical(options);
        }
        public void Dispose()
        {
            EGL.EndVertical();
        }
    }

    public class ScrollGroup : IDisposable
    {
        public Vector2 pos;
        public ScrollGroup(Vector2 scrollPosition, GUILayoutOption[] options = null)
        {
            pos = EGL.BeginScrollView(scrollPosition, options);
        }

        public void Dispose()
        {
            EGL.EndScrollView();
        }
    }
}
