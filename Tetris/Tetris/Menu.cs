using System;

namespace C__My_Project
{
    internal class Menu
    {
        private int _titleX, _titleY;               // 타이틀 좌표
        private int _selectMenuX, _selectMenuY;     // 메뉴 선택지 좌표

        private string[] _title = new string[5];    // 타이틀 배열

        // Property (Getter, Setter), InputManager에서의 처리를 위한 프로퍼티
        public int SelectMenuX { get { return _selectMenuX; } set { _selectMenuX = value; } }
        public int SelectMenuY { get { return _selectMenuY; } set { _selectMenuY = value; } }

        // 생성자
        public Menu()
        {
            // 타이틀 좌표 설정
            _titleX = 30;
            _titleY = 5;

            // 메뉴 선택지 좌표 설정
            _selectMenuX = 50;
            _selectMenuY = 20;

            // 타이틀 설정
            _title[0] = "■■■■■  ■■■■  ■■■■■  ■■■    ■   ■■■ ";
            _title[1] = "    ■      ■            ■      ■   ■   ■  ■      ";
            _title[2] = "    ■      ■■■■      ■      ■■■    ■   ■■■ ";
            _title[3] = "    ■      ■            ■      ■   ■   ■        ■";
            _title[4] = "    ■      ■■■■      ■      ■    ■  ■   ■■■ ";
        }

        // 메인 메뉴 출력 메서드
        public void Display()
        {
            // 타이틀 컬러 설정
            Console.ForegroundColor = ConsoleColor.Yellow;

            // 타이틀 출력
            for (int i = 0; i < _title.Length; i++)
            {
                Console.SetCursorPosition(_titleX, _titleY + i);
                Console.WriteLine(_title[i]);
            }
            Console.ResetColor();   // 컬러 설정 리셋

            // 메뉴 선택지 출력
            Console.SetCursorPosition(_selectMenuX, _selectMenuY);
            Console.Write("> 게임 시작");
            _selectMenuY += 2;              // 1칸 공백
            Console.SetCursorPosition(_selectMenuX, _selectMenuY++);
            Console.Write("  게임 종료");

            // 선택지 입력 처리를 위한 설정
            _selectMenuY = 20;
            Console.SetCursorPosition(_selectMenuX, _selectMenuY);
        }
    }
}