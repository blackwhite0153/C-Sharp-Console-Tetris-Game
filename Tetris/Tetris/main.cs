using System;

namespace C__My_Project
{
    // 게임 상태를 나타내는 Enum
    enum GameState
    {
        MainMenu,   // 메인 메뉴
        Playing,    // 게임 중
        GameOver,   // 게임 오버
        Exit        // 게임 종료
    };

    internal class main
    {
        public static bool isStart = false;      // 게임 시작 여부
        public static bool isExit = false;       // 게임 종료 여부
        public static bool isReStart = false;    // 게임 재시작 여부
        public static bool isBackMenu = false;   // 메인 메뉴 돌아가기 여부

        public static bool isGameOver = false;   // 게임 오버 여부

        private static GameState _currentState = GameState.MainMenu;    // 현재 게임 상태

        private static Menu _mainMenu;              // 메인 메뉴 객체
        private static GameScreen _gameScreen;      // 게임 화면 객체
        private static GameOver _gameOver;          // 게임 오버 객체
        private static InputManager _inputManager;  // 입력 관리 객체

        static void Main(string[] args)
        {
            // 게임 관련 객체 초기화
            InitializeObjects();

            Console.SetWindowSize(120, 40);     // 콘솔 창 크기 설정
            Console.CursorVisible = false;      // 콘솔 창 커서 숨김

            while (_currentState != GameState.Exit)
            {
                // 게임 오버 상태
                if (isGameOver) _currentState = GameState.GameOver;

                switch (_currentState)
                {
                    case GameState.MainMenu:
                        HandleMainMenu();       // 메인 메뉴 호출
                        break;
                    case GameState.Playing:
                        HandleGameScreen();     // 게임 화면 호출
                        break;
                    case GameState.GameOver:
                        HandleGameOver();       // 게임 오버 화면 호출
                        break;
                }
            }

            // 종료 처리
            Console.Clear();
        }

        // 메인 메뉴 관리 메서드
        private static void HandleMainMenu()
        {
            // 화면 리셋
            Console.Clear();

            // 객체 초기화 (재생성)
            InitializeObjects();

            // 메인 메뉴 출력 및 입력 처리
            _mainMenu.Display();
            _inputManager.MenuControl();

            // 게임 상태 전환
            if (isStart)
            {
                _currentState = GameState.Playing;
            }
            else if (isExit)
            {
                _currentState = GameState.Exit;
            }
        }

        // 게임 화면 관리 메서드
        private static void HandleGameScreen()
        {
            // 화면 리셋
            Console.Clear();

            // 객체 초기화 (재생성)
            InitializeObjects();

            // 게임 화면 출력 및 입력 처리
            _gameScreen.Display();
            _inputManager.GameControl();

            // 게임 상태 전환
            if (isBackMenu)
            {
                _currentState = GameState.MainMenu;
                isExit = false; // 상태 초기화
            }
        }

        // 게임 오버 관리 메서드
        private static void HandleGameOver()
        {
            // 화면 리셋
            Console.Clear();

            // 객체 초기화 (재생성)
            InitializeObjects();

            // 게임 오버 화면 출력 및 입력 처리
            _gameOver.Display();
            _inputManager.GameOverControl();

            if (isReStart && isGameOver)
            {
                _currentState = GameState.Playing;  // 게임 재시작
                isReStart = false;  // 상태 초기화
                isGameOver = false;
                return;
            }
            else if (isBackMenu && isGameOver)
            {
                _currentState = GameState.MainMenu; // 메인 메뉴로 돌아가기
                isBackMenu = false; // 상태 초기화
                isGameOver = false;
                return;
            }
            else if (isExit && isGameOver)
            {
                _currentState = GameState.Exit;     // 게임 종료
                isExit = false;     // 상태 초기화
                isGameOver = false;
            }
        }

        // 객체 초기화 메서드
        private static void InitializeObjects()
        {
            _mainMenu = new Menu();
            _gameScreen = new GameScreen();
            _gameOver = new GameOver();
            _inputManager = new InputManager(_mainMenu, _gameOver);
        }
    }
}
