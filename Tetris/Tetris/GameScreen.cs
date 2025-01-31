using System;

namespace C__My_Project
{
    internal class GameScreen
    {
        // 보드 및 화면 좌표와 크기 상수 정의
        private const int _boardX = 40, _boardY = 3;                // 게임 보드 좌표
        private const int _boardRow = 30, _boardCol = 15;           // 게임 보드 크기

        private const int _nextBlockX = 85, _nextBlockY = 10;       // 다음 블록 출력 좌표
        private const int _nextBlockRow = 6, _nextBlockCol = 6;     // 다음 블록 크기

        private const int _textX = 75, _textY = 20;                 // 텍스트 출력 좌표

        // 게임 데이터
        private static readonly int[,] _gameBoard = new int[_boardRow, _boardCol];      // 게임 보드 배열
        private static readonly int[,] _colorArray = new int[_boardRow, _boardCol];     // 보드의 컬러 배열
        private readonly int[,] _nextBlock = new int[_nextBlockRow, _nextBlockCol];     // 다음 블록 배열
        private static readonly string[] _gameText = new string[6];

        public static int Score { get; set; }   // 점수 값 변동을 위한 Property

        // 외부 클래스에서 읽기 위한 속성
        public static int BoardHeight => _boardRow;
        public static int BoardWidth => _boardCol;
        public static int[,] BoardArray => _gameBoard;
        public static int[,] ColorArray => _colorArray;
        public static int BoardStartX => _boardX;
        public static int BoardStartY => _boardY;
        public static int NextBlockStartX => _nextBlockX;
        public static int NextBlockStartY => _nextBlockY;

        // 생성자
        public GameScreen()
        {
            // 점수
            Score = 0;

            // 텍스트 배열 초기화
            InitializeText(_gameText);

            // 보드 배열 초기화
            InitializeBoard(_gameBoard, _boardRow, _boardCol);

            // 다음 블록 배열 초기화
            InitializeNextBoard(_nextBlock, _nextBlockRow, _nextBlockCol);
        }

        // 보드 배열 초기화 메서드
        private static void InitializeBoard(int[,] board, int rows, int cols)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (row == rows - 1 || col == 0 || col == cols - 1)
                        board[row, col] = 1; // 테두리 부분 1로 설정
                    else
                        board[row, col] = 0; // 내부는 0으로 설정
                }
            }
        }

        // 다음 블록 표시 칸 배열 초기화 메서드
        private static void InitializeNextBoard(int[,] nextBoard, int rows, int cols)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (row == 0 || row == rows - 1 || col == 0 || col == cols - 1)
                        nextBoard[row, col] = 1; // 테두리 부분 1로 설정
                    else
                        nextBoard[row, col] = 0; // 내부는 0으로 설정
                }
            }
        }

        // 텍스트 배열 초기화 메서드
        private static void InitializeText(string[] text)
        {
            text[0] = $"  Score      : {Score}";
            text[1] = " Exit Key    : ESC";
            text[2] = "    ▲       : Block Rotate";
            text[3] = "  ◀  ▶     : Move LEFT / RIGHT";
            text[4] = "    ▼       : Move DOWN";
            text[5] = " ■■■■    : SPACE (Direct DOWN)";
        }

        // 보드 출력 메서드
        private void DrawBoard()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;    // 보드 컬러 설정

            for (int row = 0; row < _boardRow; row++)
            {
                Console.SetCursorPosition(_boardX, _boardY + row);           // 해당 위치부터 출력

                for (int col = 0; col < _boardCol; col++)
                {
                    Console.Write(_gameBoard[row, col] == 1 ? "▦" : "  ");  // 배열 값 1은 "▦", 그 외 "  " 출력
                }
                Console.WriteLine();
            }
            Console.ResetColor();   // 컬러 설정 리셋
        }

        // 다음 블록 출력 메서드
        private void DrawNextBlock()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;    // 다음 블록 표시 칸 컬러 설정

            for (int row = 0; row < _nextBlockRow; row++)
            {
                Console.SetCursorPosition(_nextBlockX, _nextBlockY + row);   // 해당 위치부터 출력

                for (int col = 0; col < _nextBlockCol; col++)
                {
                    Console.Write(_nextBlock[row, col] == 1 ? "▦" : "  ");  // 배열 값 1은 "▦", 그 외 "  " 출력
                }
                Console.WriteLine();
            }
            Console.ResetColor();   // 컬러 설정 리셋
        }

        // 텍스트 출력 메서드
        public static void DrawText()
        {
            InitializeText(_gameText);

            for (int i = 0; i < _gameText.Length; i++)
            {
                Console.SetCursorPosition(_textX, _textY + i);  // 해당 위치부터 출력
                Console.WriteLine(_gameText[i]);
            }
        }

        // 게임 화면 출력 메서드
        public void Display()
        {
            DrawBoard();
            DrawNextBlock();
            DrawText();
        }
    }
}