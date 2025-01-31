using System;

namespace C__My_Project
{
    internal class Color
    {
        // 컬러 목록을 나타내는 Enum
        public enum BlockColor
        {
            Blue = 9,
            Green = 10,
            Red = 12,
            Magenta = 13,
            Yellow = 14
        }

        // 색상을 설정하는 메서드
        public static void SetConsoleColor(BlockColor blockColor)
        {
            // 열거형 값에 맞는 ForegroundColor 설정
            Console.ForegroundColor = (ConsoleColor)blockColor;
        }

        // 랜덤 BlockColor 메서드
        public static T GetRandomBlockColor<T>()
        {
            // BlockColor 열거형 값 배열로 가져오기
            Array colors = Enum.GetValues(typeof(BlockColor));

            return (T)colors.GetValue(new Random().Next(0, colors.Length));
        }
    }
}