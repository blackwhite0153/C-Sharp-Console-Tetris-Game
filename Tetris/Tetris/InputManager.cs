using System;

namespace C__My_Project
{
    internal class InputManager
    {
        private readonly Menu _menu;            // 메인 메뉴 컨트롤 객체
        private readonly GameOver _gameOver;    // 게임 오버 메뉴 컨트롤 객체
        private readonly MovementManager _movementManager = new MovementManager();  // 블록 움직임 관리 객체

        private const int BlockFallInterval = 200;  // 블록 자동 하강 간격 (ms, 밀리초 단위)

        // 생성자
        public InputManager(Menu menu, GameOver gameOver)
        {
            this._menu = menu;
            this._gameOver = gameOver;
        }

        // 메인 메뉴 컨트롤 메서드
        public void MenuControl()
        {
            ConsoleKeyInfo input;

            while (true)
            {
                // 키 입력 감지
                if (Console.KeyAvailable)
                {
                    // intercept: true : 키 출력 숨김
                    input = Console.ReadKey(intercept: true);

                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:    // ↑: 메뉴 위로 이동
                            MoveMenuUp();
                            continue;
                        case ConsoleKey.DownArrow:  // ↓: 메뉴 아래로 이동
                            MoveMenuDown();
                            continue;
                        case ConsoleKey.Enter:      // Enter : 선택
                            IsStart();
                            break;
                        default:
                            continue;
                    }
                    break;
                }
            }
        }

        // 메뉴 위로 이동 메서드
        private void MoveMenuUp()
        {
            if (_menu.SelectMenuY == 22) _menu.SelectMenuY -= 2;

            if (_menu.SelectMenuY <= 20)
            {
                _menu.SelectMenuY = 20;
                UpdateMenuDisplay("> 게임 시작", "  게임 종료");
            }
        }

        // 메뉴 아래로 이동 메서드
        private void MoveMenuDown()
        {
            if (_menu.SelectMenuY == 20) _menu.SelectMenuY += 2;

            if (_menu.SelectMenuY >= 22)
            {
                _menu.SelectMenuY = 22;
                UpdateMenuDisplay("  게임 시작", "> 게임 종료");
            }
        }

        // 메뉴 화면 업데이트 메서드
        private void UpdateMenuDisplay(string option1, string option2)
        {
            Console.SetCursorPosition(_menu.SelectMenuX, 20);
            Console.Write(option1);
            Console.SetCursorPosition(_menu.SelectMenuX, 22);
            Console.Write(option2);
        }

        // 게임 시작 여부 확인 및 설정 메서드
        private void IsStart()
        {
            if (_menu.SelectMenuY == 20) main.isStart = true;
            else if (_menu.SelectMenuY == 22) main.isExit = true;
        }

        // 게임 화면 컨트롤 메서드
        public void GameControl()
        {
            ConsoleKeyInfo input;

            // 초기 블록 및 다음 블록 화면 출력
            _movementManager.RandomBlock();
            _movementManager.DrawNextBlock();

            // 블록 하강 기준 시간
            DateTime lastFallTime = DateTime.Now;

            while (true)
            {
                // 게임 오버 시 반복 종료
                if (main.isGameOver) return;

                // 그림자 블록 출력 및 제거
                _movementManager.DrawShadowBlock();

                // 현재 시간 기준으로 0.2초 경과 && 게임 오버 상태가 아닐 경우 작동
                // 현재 시각과 마지막 하강 시각 간의 경과 시간을 밀리초 단위로 계산
                if ((DateTime.Now - lastFallTime).TotalMilliseconds >= BlockFallInterval)
                {
                    // 블록을 한 칸 아래로 이동
                    _movementManager.MoveBlockDown(GameScreen.BoardArray, GameScreen.ColorArray);
                    // 마지막 블록이 하강 시간 갱신
                    lastFallTime = DateTime.Now;
                }

                // 키 입력 감지
                if (Console.KeyAvailable)
                {
                    // intercept: true : 키 출력 숨김
                    input = Console.ReadKey(intercept: true);

                    // 게임 중 키 입력 처리
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:    // ↑ : 블록 회전
                            // 블록의 회전 상태, X 좌표값이 변화 할때만 삭제 처리
                            _movementManager.DeleteShadowBlock();
                            _movementManager.RotateBlock(GameScreen.BoardArray);
                            continue;
                        case ConsoleKey.DownArrow:  // ↓ : 블록 한 칸 하강
                            _movementManager.MoveBlockDown(GameScreen.BoardArray, GameScreen.ColorArray);
                            continue;
                        case ConsoleKey.LeftArrow:  // ← : 블록 왼쪽 이동
                            _movementManager.DeleteShadowBlock();
                            _movementManager.MoveBlockLeft(GameScreen.BoardArray);
                            continue;
                        case ConsoleKey.RightArrow: // → : 블록 오른쪽 이동
                            _movementManager.DeleteShadowBlock();
                            _movementManager.MoveBlockRight(GameScreen.BoardArray);
                            continue;
                        case ConsoleKey.Spacebar:   // Spacebar : 블록 빠른 하강
                            _movementManager.FastBlockDown(GameScreen.BoardArray, GameScreen.ColorArray);
                            continue;
                        case ConsoleKey.Escape:     // Esc : 메인 메뉴 돌아가기
                            main.isBackMenu = true;
                            break;
                        default:
                            continue;
                    }
                    break;
                }
            }
        }

        // 게임 오버 화면 컨트롤 메서드
        public void GameOverControl()
        {
            ConsoleKeyInfo input;

            while (true)
            {
                // 키 입력 감지
                if (Console.KeyAvailable)
                {
                    // intercept: true : 키 출력 숨김
                    input = Console.ReadKey(intercept: true);

                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:    // ↑: 메뉴 위로 이동
                            MoveGameOverMenuUp();
                            continue;
                        case ConsoleKey.DownArrow:  // ↓: 메뉴 아래로 이동
                            MoveGameOverMenuDown();
                            continue;
                        case ConsoleKey.Enter:      // Enter : 선택
                            IsReStart();
                            break;
                        default:
                            continue;
                    }
                    break;
                }
            }
        }

        // 게임 오버 메뉴 위로 이동 메서드
        private void MoveGameOverMenuUp()
        {
            _gameOver.SelectGameOverMenuY -= 2;

            if (_gameOver.SelectGameOverMenuY <= 20)
            {
                _gameOver.SelectGameOverMenuY = 20;
                UpdateGameOverMenuDisplay("> 게임 재시작", "  메인으로 돌아가기", "  게임 종료");
            }
            else if (_gameOver.SelectGameOverMenuY > 20 && _gameOver.SelectGameOverMenuY < 24)
            {
                _gameOver.SelectGameOverMenuY = 22;
                UpdateGameOverMenuDisplay("  게임 재시작", "> 메인으로 돌아가기", "  게임 종료");
            }
            else if (_gameOver.SelectGameOverMenuY >= 24)
            {
                _gameOver.SelectGameOverMenuY = 24;
                UpdateGameOverMenuDisplay("  게임 재시작", "  메인으로 돌아가기", "> 게임 종료");
            }
        }

        // 게임 오버 메뉴 아래로 이동 메서드
        private void MoveGameOverMenuDown()
        {
            _gameOver.SelectGameOverMenuY += 2;

            if (_gameOver.SelectGameOverMenuY <= 20)
            {
                _gameOver.SelectGameOverMenuY = 20;
                UpdateGameOverMenuDisplay("> 게임 재시작", "  메인으로 돌아가기", "  게임 종료");
            }
            else if (_gameOver.SelectGameOverMenuY > 20 && _gameOver.SelectGameOverMenuY < 24)
            {
                _gameOver.SelectGameOverMenuY = 22;
                UpdateGameOverMenuDisplay("  게임 재시작", "> 메인으로 돌아가기", "  게임 종료");
            }
            else if (_gameOver.SelectGameOverMenuY >= 24)
            {
                _gameOver.SelectGameOverMenuY = 24;
                UpdateGameOverMenuDisplay("  게임 재시작", "  메인으로 돌아가기", "> 게임 종료");
            }
        }

        // 게임 오버 메뉴 화면 업데이트 메서드
        private void UpdateGameOverMenuDisplay(string option1, string option2, string option3)
        {
            Console.SetCursorPosition(_gameOver.SelectGameOverMenuX, 20);
            Console.Write(option1);
            Console.SetCursorPosition(_gameOver.SelectGameOverMenuX, 22);
            Console.Write(option2);
            Console.SetCursorPosition(_gameOver.SelectGameOverMenuX, 24);
            Console.Write(option3);
        }

        // 게임 재시작 여부 확인 및 설정 메서드
        private void IsReStart()
        {
            if (_gameOver.SelectGameOverMenuY == 20) main.isReStart = true;         // 게임 재시작
            else if (_gameOver.SelectGameOverMenuY == 22) main.isBackMenu = true;   // 메인 메뉴로 돌아가기
            else if (_gameOver.SelectGameOverMenuY == 24) main.isExit = true;       // 게임 종료
        }
    }
}
