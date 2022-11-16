namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
[CustomEditor(typeof(ShapeData),false)]
[CanEditMultipleObjects]
[System.Serializable]
    public class ShapeDataDrawer : Editor
    {
       private ShapeData ShapeDataInstance => target as ShapeData;

       public override void OnInspectorGUI()
       {
           serializedObject.Update();
           ClearBoardButton();
           EditorGUILayout.Space();
           
           DrawColumnsInputFields();
           EditorGUILayout.Space();

           if (ShapeDataInstance.board != null && ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
           {
               DrawBoardTable();
           }

           serializedObject.ApplyModifiedProperties();

           if (GUI.changed)
           {
               EditorUtility.SetDirty(ShapeDataInstance);
           }
       }

       private void ClearBoardButton()
       {
           if (GUILayout.Button("Clear Board"))
           {
               ShapeDataInstance.Clear();
           }
       }

       private void DrawColumnsInputFields()
       {
           var _columnsTemp = ShapeDataInstance.columns;
           var _rowsTemp = ShapeDataInstance.rows;

           ShapeDataInstance.columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.columns);
           ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);

           if ((ShapeDataInstance.columns != _columnsTemp || ShapeDataInstance.rows != _rowsTemp) &&
               ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
           {
               ShapeDataInstance.CreateNewBoard();
           }
       }

       private void DrawBoardTable()
       {
           var _tableStyle = new GUIStyle("box");
           _tableStyle.padding = new RectOffset(10, 10, 10, 10);
           _tableStyle.margin.left = 32;

           var _headerColumnSytle = new GUIStyle();
           _headerColumnSytle.fixedWidth = 65;
           _headerColumnSytle.alignment = TextAnchor.MiddleCenter;

           var _rowStyle = new GUIStyle();
           _rowStyle.fixedHeight = 25;
           _rowStyle.alignment = TextAnchor.MiddleCenter;

           var _dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
           _dataFieldStyle.normal.background = Texture2D.grayTexture;
           _dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

           for (var row = 0; row < ShapeDataInstance.rows; row++)
           {
               EditorGUILayout.BeginHorizontal(_headerColumnSytle);
               for (var column = 0; column < ShapeDataInstance.columns; column++)
               {
                   EditorGUILayout.BeginHorizontal(_rowStyle);
                   var _data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].column[column], _dataFieldStyle);
                   ShapeDataInstance.board[row].column[column] = _data;
                   EditorGUILayout.EndHorizontal();
               }
               EditorGUILayout.EndHorizontal();
           }
       }
    }

}