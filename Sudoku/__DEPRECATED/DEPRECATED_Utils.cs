// using System;

// namespace Sudoku
// {
//     public static class Utils
//     {
//         public static void Loop(Action<int> action) => Loop(Constants.UnitSize, action);
//         public static void Loop<T>(T t, Action<int, T> action) => Loop<T>(Constants.UnitSize, t, action);
//         public static bool LoopAnd(Func<int, bool> func) => LoopAnd(Constants.UnitSize, func);
//         public static bool LoopOr(Func<int, bool> func) => LoopOr(Constants.UnitSize, func);

//         public static void Loop(int count, Action<int> action)
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 action?.Invoke(i);
//             }
//         }

//         public static void Loop<T>(int count, T t, Action<int, T> action)
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 action?.Invoke(i, t);
//             }
//         }

//         public static bool LoopAnd(int count, Func<int, bool> func)
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 if (!func?.Invoke(i) ?? false) return false;
//             }
//             return true;
//         }

//         public static bool LoopOr(int count, Func<int, bool> func)
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 if (func?.Invoke(i) ?? false) return true;
//             }
//             return false;
//         }
//     }
// }
