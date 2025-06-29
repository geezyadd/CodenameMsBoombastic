using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(GameObject))]
// public class CustomInspector : Editor
// {
//     private SerializedObject serializedObj;
//
//     private void OnEnable()
//     {
//         // Инициализируем SerializedObject для текущего объекта
//         serializedObj = new SerializedObject(target);
//     }
//
//     public override void OnInspectorGUI()
//     {
//         // Обновляем SerializedObject, чтобы отобразить актуальные данные
//         serializedObj.Update();
//
//         // Рисуем кастомный заголовок
//         DrawHeader();
//
//         // Рисуем компонент вручную
//         DrawComponent();
//
//         // Применяем изменения
//         serializedObj.ApplyModifiedProperties();
//     }
//
//     private void DrawHeader()
//     {
//         // Рисуем кастомный заголовок
//         GUILayout.BeginHorizontal("Box");
//         GUILayout.Label("Custom Inspector Header", EditorStyles.boldLabel);
//         GUILayout.EndHorizontal();
//     }
//
//     private void DrawComponent()
//     {
//         // Вручную рисуем компоненты
//         EditorGUILayout.LabelField("My Custom Component", EditorStyles.boldLabel);
//         
//         // Отображаем стандартные поля компонента
//         SerializedProperty myProperty = serializedObj.FindProperty("myComponent");
//         if (myProperty != null)
//         {
//             EditorGUILayout.PropertyField(myProperty);
//         }
//
//         // Дополнительные кастомные элементы
//         if (GUILayout.Button("Custom Button"))
//         {
//             Debug.Log("Button clicked!");
//         }
//     }
// }