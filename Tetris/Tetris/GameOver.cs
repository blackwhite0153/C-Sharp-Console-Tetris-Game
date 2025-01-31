using System;

namespace C__My_Project
{
    internal class GameOver
    {
        private int _gameOverX, _gameOverY;             // 게임 오버 좌표
        private int _selectGameOverMenuX, _selectGameOverMenuY; // 게임 오버 시 선택지 좌표

        private string[] _gameOver = new string[5];     // 게임 오버 배열

        // Property (Getter, Setter), InputManager에서의 처리를 위한 프로퍼티
        public int SelectGameOverMenuX { get { return _selectGameOverMenuX; } set { _selectGameOverMenuX = value; } }
        public int SelectGameOverMenuY { get { return _selectGameOverMenuY; } set { _selectGameOverMenuY = value; } }

        public GameOver()
        {
            // 게임 오버 좌표 설정
            _gameOverX = 10;
            _gameOverY = 5;

            // 게임 오버 선택지 좌표 설정
            _selectGameOverMenuX = 50;
            _selectGameOverMenuY = 20;

            // 게임 오버 설정
            _gameOver[0] = "   ■■■        ■■     ■■      ■■ ■■■■       ■■■  ■        ■ ■■■■ ■■■   ";
            _gameOver[1] = " ■             ■  ■    ■ ■    ■ ■ ■            ■    ■  ■      ■  ■       ■   ■  ";
            _gameOver[2] = " ■  ■■■    ■■■■   ■  ■  ■  ■ ■■■■     ■      ■  ■    ■   ■■■■ ■■■   ";
            _gameOver[3] = " ■    ■■   ■      ■  ■   ■■   ■ ■            ■    ■    ■  ■    ■       ■   ■  ";
            _gameOver[4] = "  ■■■ ■  ■        ■ ■    ■    ■ ■■■■       ■■■      ■■     ■■■■ ■    ■ ";
        }

        // 메인 메뉴 출력 메서드
        public void Display()
        {
            // 타이틀 컬러 설정
            Console.ForegroundColor = ConsoleColor.Red;

            // 타이틀 출력
            for (int i = 0; i < _gameOver.Length; i++)
            {
                Console.SetCursorPosition(_gameOverX, _gameOverY + i);
                Console.WriteLine(_gameOver[i]);
            }
            Console.ResetColor();   // 컬러 설정 리셋

            // 메뉴 선택지 출력
            Console.SetCursorPosition(_selectGameOverMenuX, _selectGameOverMenuY);
            Console.Write("> 게임 재시작");
            _selectGameOverMenuY += 2;              // 1칸 공백
            Console.SetCursorPosition(_selectGameOverMenuX, _selectGameOverMenuY);
            Console.Write("  메인으로 돌아가기");
            _selectGameOverMenuY += 2;              // 1칸 공백
            Console.SetCursorPosition(_selectGameOverMenuX, _selectGameOverMenuY++);
            Console.Write("  게임 종료");

            // 선택지 입력 처리를 위한 설정
            _selectGameOverMenuY = 20;
            Console.SetCursorPosition(_selectGameOverMenuX, _selectGameOverMenuY);
        }
    }
}