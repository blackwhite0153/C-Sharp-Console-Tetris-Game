using System;

namespace C__My_Project
{
    internal class MovementManager
    {
        private Random _random = new Random();  // 랜덤 함수 사용을 위한 선언 및 초기화
        private Block _block = new Block();     // 블록 배열을 받아오기 위한 선언 및 초기화

        private const int blockSize = 4;        // 블록 크기 상수

        private int _blockType;         // 블록의 형태
        private int _nextBlockType;     // 다음 블록의 형태

        private int _blockColor;        // 블록의 컬러
        private int _nextBlockColor;    // 다음 블록의 컬러

        private int _blockCreateX, _blockCreateY;   // 블록의 초기 생성 X, Y 좌표 값

        private int _moveRotate;        // 블록 회전 상태 값
        private int _moveLeftRight;     // 블록 좌우측 이동 상태 값
        private int _moveDown;          // 블록 하강 상태 값

        private int _shadowBlockY;      // 그림자 블록 Y 좌표

        // 생성자
        public MovementManager()
        {
            _blockCreateX = (GameScreen.BoardStartX + 10);   // 생성 좌표 X = 50 (+ 10 해줘야 보드의 정중앙이 초기 생성 위치로 설정됨)
            _blockCreateY = (GameScreen.BoardStartY);        // 생성 좌표 Y = 3

            _nextBlockType = _random.Next(0, 7);                // 초기 생성 블록 타입
            _nextBlockColor = Color.GetRandomBlockColor<int>(); // 초기 생성 블록 컬러

            _moveRotate = 0;                     // 초기 회전 상태
            _moveLeftRight = _blockCreateX;      // 좌우측 이동을 시작할 위치
            _moveDown = _blockCreateY;           // 1칸씩 하강을 시작할 위치

            _shadowBlockY = 0;
        }

        // 랜덤 블록 생성 메서드
        public void RandomBlock()
        {
            _blockType = _nextBlockType;            // 다음 블록 출력하기 위해 설정
            _blockColor = _nextBlockColor;          // 다음 블록 컬러 출력하기 위해 설정
            _nextBlockType = _random.Next(0, 7);    // 새로운 블록 미리 뽑아둠
            _nextBlockColor = Color.GetRandomBlockColor<int>(); // 새로운 블록 컬러 미리 뽑아둠
        }

        // 블록 출력 메서드
        public void DrawBlock()
        {
            Console.ForegroundColor = (ConsoleColor)_blockColor;    // 블록 컬러 설정

            for (int row = 0; row < blockSize; row++)
            {
                Console.SetCursorPosition(_moveLeftRight, _moveDown + row); // 해당 위치부터 출력

                for (int col = 0; col < blockSize; col++)   // 해당 위치부터 출력 시작
                {
                    if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 2)
                    {
                        Console.Write("■"); // 블록의 값이 2일 경우만 출력
                    }
                    else if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 0)
                    {
                        // 공백(0)은 출력하지 않고 보드 값을 유지
                        Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
                    }
                }
            }
            Console.ResetColor();   // 컬러 설정 리셋
        }

        // 블록 제거 메서드
        public void DeleteBlock()
        {
            for (int row = 0; row < blockSize; row++)
            {
                Console.SetCursorPosition(_moveLeftRight, _moveDown + row);  // 해당 위치부터 제거

                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 2)
                    {
                        Console.Write("  "); // 블록의 값이 2인 경우만 제거
                    }
                    else if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 0)
                    {
                        // 공백(0)은 제거하지 않고 커서만 이동
                        Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
                    }
                }
            }
        }

        // 그림자 블록 출력 메서드
        public void DrawShadowBlock()
        {
            int shadowBlockY = _moveDown;

            // 그림자 블록 위치 계산
            while (true)
            {
                // 그림자 블록이 밑 경계나 다른 블록과 충돌하기 전까지 Y 좌표를 내린다.
                if (!CheckCollision(GameScreen.BoardArray, _moveLeftRight, shadowBlockY + 1, _moveRotate))
                {
                    shadowBlockY++;
                }
                else
                {
                    break;
                }
            }

            _shadowBlockY = shadowBlockY;

            // 그림자 블록 값을 보드 배열에 설정하고 출력
            for (int row = 0; row < blockSize; row++)
            {
                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 2)
                    {
                        int shadowX = (_moveLeftRight - GameScreen.BoardStartX) / 2 + col;   // 그림자 블록 X 좌표
                        int shadowY = (_shadowBlockY - GameScreen.BoardStartY) + row;        // 그림자 블록 Y 좌표
                        int blockY = (_moveDown - GameScreen.BoardStartY) + row;             // 현재 블록 Y 좌표

                        if (shadowY > blockY && shadowY >= 0 && shadowY < GameScreen.BoardHeight &&
                            shadowX >= 0 && shadowX < GameScreen.BoardWidth)
                        {
                            // 그림자 블록의 위치를 20으로 설정
                            GameScreen.BoardArray[shadowY, shadowX] = 20;

                            // 그림자 블록 출력
                            Console.SetCursorPosition(shadowX * 2 + GameScreen.BoardStartX, shadowY + GameScreen.BoardStartY);
                            Console.Write("□");
                        }
                    }
                }
            }
        }

        // 그림자 블록 제거 메서드
        public void DeleteShadowBlock()
        {
            for (int row = 0; row < blockSize; row++)
            {
                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 2)
                    {
                        int boardX = (_moveLeftRight - GameScreen.BoardStartX) / 2 + col;   // X 좌표 변환
                        int boardY = (_shadowBlockY - GameScreen.BoardStartY) + row;        // Y 좌표 변환

                        if (boardY >= 0 && boardY < GameScreen.BoardHeight &&
                            boardX >= 0 && boardX < GameScreen.BoardWidth)
                        {
                            // 그림자 블록 값 초기화
                            if (GameScreen.BoardArray[boardY, boardX] == 20)
                            {
                                GameScreen.BoardArray[boardY, boardX] = 0;

                                // 화면에서 그림자 블록 제거
                                Console.SetCursorPosition(boardX * 2 + GameScreen.BoardStartX, boardY + GameScreen.BoardStartY);
                                Console.Write("  ");
                            }
                        }
                    }
                }
            }
        }

        // 다음 블록 표시 메서드
        public void DrawNextBlock()
        {
            Console.ForegroundColor = (ConsoleColor)_nextBlockColor;    // 다음 블록 컬러 설정

            // 다음 블록 재생성을 위한 좌표 설정 (+1은 6x6 박스에 4x4 블록이 들어가는 형태이기 때문)
            int nextBlockX = GameScreen.NextBlockStartX + 1;
            int nextBlockY = GameScreen.NextBlockStartY + 1;

            for (int row = 0; row < 4; row++)
            {
                Console.SetCursorPosition(nextBlockX, nextBlockY + row);    // 해당 위치부터 출력

                for (int col = 0; col < 4; col++)
                {
                    if (Block.tetrisBlock[_nextBlockType, 0, row, col] == 2) Console.Write("■");     // 배열의 2는 "■"으로 출력
                    else Console.Write("  ");   // 배열의 0은 "  "으로 출력
                }
            }
            Console.ResetColor();   // 컬러 설정 리셋
        }

        // 다음 블록 표시 제거 메서드
        private void DeleteNextBlock()
        {
            // 다음 블록 재생성을 위한 좌표 설정 (+1은 6x6 박스에 4x4 블록이 들어가는 형태이기 때문)
            int nextBlockX = GameScreen.NextBlockStartX + 1;
            int nextBlockY = GameScreen.NextBlockStartY + 1;

            for (int row = 0; row < blockSize; row++)
            {
                Console.SetCursorPosition(nextBlockX, nextBlockY + row);    // 해당 위치부터 출력

                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_nextBlockType, 0, row, col] == 2) Console.Write("  ");    // "■"으로 출력된 블록 제거
                }
            }
        }

        // 블록 하강 메서드
        public void MoveBlockDown(int[,] boardArray, int[,] colorArray)
        {
            // 충돌 검사
            if (!CheckCollision(boardArray, _moveLeftRight, _moveDown + 1, _moveRotate))
            {
                DeleteBlock();  // 이전 블록 지우기
                _moveDown++;    // 충돌이 없으면 블록을 한 칸 아래로 이동
            }
            else
            {
                // 블록, 하단 보드 라인과 충돌한 위치에서 고정 처리
                FixBlockToBoard(boardArray, colorArray);

                // 블록 고정 후 줄 삭제 처리
                if (IsMaxLine(GameScreen.BoardArray))
                {
                    DeleteLine(GameScreen.BoardArray, GameScreen.ColorArray);
                }

                // 다음 블록 생성
                RandomBlock();          // 새로운 블록 생성
                DeleteNextBlock();      // 기존에 표시된 다음 블록 제거
                DrawNextBlock();        // 새로운 다음 블록 표시
                CreateNewBlock();       // 새 블록 위치 초기화
            }
            DrawBlock(); // 새 위치에 블록 출력
        }

        // 블록 빠른 하강 메서드
        public void FastBlockDown(int[,] boardArray, int[,] colorArray)
        {
            // 블록을 그림자 블록 위치로 즉시 이동
            DeleteBlock();              // 기존 블록 제거
            _moveDown = _shadowBlockY;  // Y 좌표를 그림자 위치로 설정
            DrawBlock();                // 새로운 위치에 블록 출력

            // 그림자 블록과 충돌한 위치에서 고정 처리
            FixBlockToBoard(boardArray, colorArray);

            // 블록 고정 후 줄 삭제 처리
            if (IsMaxLine(GameScreen.BoardArray))
            {
                DeleteLine(GameScreen.BoardArray, GameScreen.ColorArray);
            }

            // 다음 블록 생성
            RandomBlock();          // 새로운 블록 생성
            DeleteNextBlock();      // 기존에 표시된 다음 블록 제거
            DrawNextBlock();        // 새로운 다음 블록 표시
            CreateNewBlock();       // 새 블록 위치 초기화
        }

        // 블록 좌측 이동 메서드
        public void MoveBlockLeft(int[,] boardArray)
        {
            if (!CheckCollision(boardArray, _moveLeftRight - 2, _moveDown, _moveRotate))     // -2은 충돌 전에 미리 알아야하기 때문
            {
                DeleteBlock();          // 기존 위치 블록 제거
                _moveLeftRight -= 2;    // 좌측으로 1칸 이동 (2가 1칸)
                DrawBlock();            // 이동한 위치에 블록 출력
            }
        }

        // 블록 우측 이동 메서드
        public void MoveBlockRight(int[,] boardArray)
        {
            if (!CheckCollision(boardArray, _moveLeftRight + 2, _moveDown, _moveRotate))     // +2은 충돌 전에 미리 알아야하기 때문
            {
                DeleteBlock();          // 기존 위치 블록 제거
                _moveLeftRight += 2;    // 우측으로 1칸 이동 (2가 1칸)
                DrawBlock();            // 이동한 위치에 블록 출력
            }
        }

        // 블록 회전 메서드
        public void RotateBlock(int[,] boardArray)
        {
            int newRotate = (_moveRotate + 1) % 4; // 회전 상태 변경

            if (!CheckCollision(boardArray, _moveLeftRight, _moveDown, newRotate))
            {
                DeleteBlock();              // 기존 형태 블록 제거
                _moveRotate = newRotate;    // 회전한 형태 값 설정
                DrawBlock();                // 회전한 형태의 블록 출력
            }
        }

        // 새 블록 생성 메서드
        private void CreateNewBlock()
        {
            // 게임 오버 상태가 아닐 경우에만 작동
            if (!main.isGameOver)
            {
                _moveRotate = 0;                 // 초기 생성 회전 상태
                _moveLeftRight = _blockCreateX;  // 초기 X 위치
                _moveDown = _blockCreateY;       // 초기 Y 위치

                // 블록 생성 직후 충돌 검사 (생성 좌표까지 블록이 쌓였는지 확인용)
                if (CheckCollision(GameScreen.BoardArray, _moveLeftRight, _moveDown, _moveRotate))
                {
                    // Game Over 활성화
                    main.isGameOver = true;
                }
            }
        }

        // 충돌 감지 메서드
        public bool CheckCollision(int[,] boardArray, int posX, int posY, int rotate)
        {
            for (int row = 0; row < blockSize; row++)
            {
                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_blockType, rotate, row, col] != 0)
                    {
                        int boardX = (posX - GameScreen.BoardStartX) / 2 + col;     // x 좌표에서 40을 빼고, 2로 나눠 변환
                        int boardY = (posY - GameScreen.BoardStartY) + row;         // y 좌표에서 보드 시작 위치 3을 빼서 변환

                        // 경계 충돌 확인
                        if (boardX < 0 || boardX >= GameScreen.BoardWidth || boardY >= GameScreen.BoardHeight)
                            return true;

                        // 다른 블록과의 충돌 확인 (값이 20인 경우는 제외)
                        if (boardY >= 0 && boardArray[boardY, boardX] != 0 && boardArray[boardY, boardX] != 20)
                            return true;
                    }
                }
            }
            return false; // 충돌 없음
        }

        // 블록 고정 메서드
        private void FixBlockToBoard(int[,] boardArray, int[,] colorArray)
        {
            for (int row = 0; row < blockSize; row++)
            {
                for (int col = 0; col < blockSize; col++)
                {
                    if (Block.tetrisBlock[_blockType, _moveRotate, row, col] == 2)
                    {
                        int boardX = (_moveLeftRight - GameScreen.BoardStartX) / 2 + col;   // x 좌표에서 40을 빼고, 2로 나눠 변환
                        int boardY = (_moveDown - GameScreen.BoardStartY) + row;            // y 좌표에서 보드 시작 위치 3을 빼서 변환

                        if (boardY >= 0 && boardY < GameScreen.BoardHeight && boardX >= 0 && boardX < GameScreen.BoardWidth)    // 보드 범위 내에 있는 경우
                        {
                            boardArray[boardY, boardX] = _blockType + 3;    // 고정된 블록 저장
                            colorArray[boardY, boardX] = _blockColor;       // 색상 정보 저장
                        }
                    }
                }
            }
        }

        // 채워진 줄 검사 메서드
        private bool IsMaxLine(int[,] boardArray)
        {
            for (int y = GameScreen.BoardHeight - 2; y > 1; y--)
            {
                int count = 0;

                for (int x = 1; x < GameScreen.BoardWidth; x++)
                {
                    if (boardArray[y, x] >= 3) // 블록이 있는 칸만 카운트
                        count++;
                    // 보드의 너비 전체가 채워졌는지 확인
                    if (count > 12)
                        return true;
                }
            }
            return false;
        }

        // 채워진 줄 제거 메서드
        private void DeleteLine(int[,] boardArray, int[,] colorArray)
        {
            // 현재 커서 위치 저장
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            // 가득 찬 줄을 위에서부터 처리
            for (int y = GameScreen.BoardHeight - 2; y > 0; y--)
            {
                bool isFullLine = true;

                // 해당 줄이 꽉 찼는지 확인
                for (int x = 1; x < GameScreen.BoardWidth - 1; x++)
                {
                    if (boardArray[y, x] == 0)
                    {
                        isFullLine = false;
                        break;
                    }
                }

                if (isFullLine)
                {
                    // 꽉 찬 줄을 삭제
                    for (int x = 1; x < GameScreen.BoardWidth - 1; x++)
                    {
                        boardArray[y, x] = 0;
                        colorArray[y, x] = 0;

                        // 화면에서도 해당 줄 제거
                        Console.SetCursorPosition(x * 2 + GameScreen.BoardStartX, y + GameScreen.BoardStartY);
                        Console.Write("  ");
                    }

                    // 점수 증가
                    GameScreen.Score += 1000;

                    // 윗줄을 아래로 모두 내림
                    for (int moveY = y; moveY > 0; moveY--)
                    {
                        for (int x = 1; x < GameScreen.BoardWidth - 1; x++)
                        {
                            // 1줄 내리기 위해 현위치에 윗줄 값 할당
                            boardArray[moveY, x] = boardArray[moveY - 1, x];
                            colorArray[moveY, x] = colorArray[moveY - 1, x];

                            // 화면 갱신
                            Console.SetCursorPosition(x * 2 + GameScreen.BoardStartX, moveY + GameScreen.BoardStartY);

                            // 블록 고정 시 블록 타입 + 3 값을 할당했기에 검사도 3 이상인 값을 탐색해야한다.
                            if (boardArray[moveY, x] >= 3)
                            {
                                Console.ForegroundColor = (ConsoleColor)colorArray[moveY, x];   // 색상 배열에서 색상 정보 가져와 설정
                                Console.Write("■");
                                Console.ResetColor();   // 컬러 설정 리셋
                            }
                            else
                            {
                                Console.Write("  ");
                            }
                        }
                    }
                    // 현재 삭제된 줄 이후 다시 확인
                    y++;
                }
            }
            // 커서 위치 복구
            Console.SetCursorPosition(cursorX, cursorY);

            // 점수 출력
            GameScreen.DrawText();
        }
    }
}