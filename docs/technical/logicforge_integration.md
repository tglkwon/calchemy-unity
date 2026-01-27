# LogicForge 연동 시스템 (LogicForge Integration)

LogicForge(Web)에서 정의된 카드 데이터와 로직 스키마를 Unity 프로젝트로 동기화하고 실행하는 시스템입니다.

> [!WARNING]
> **검증 대기 중 (Pending Verification)**
> 본 문서는 구현된 시스템의 아키텍처를 설명하며, 실제 API 서버와의 연동 테스트 및 대량 데이터 로드 검증은 아직 수행되지 않았습니다. (Step 2 완료, Step 3/4 대기 중)

## 1. 시스템 구조 (Architecture)

### 데이터 파이프라인 (Data Pipeline)
1.  **Schema Sync (`LogicForgeGenerator`)**: API로부터 C# 스키마 코드를 받아 `SharedSchema.cs`를 생성/갱신합니다.
2.  **Data Sync (`LogicForgeSync`)**: API로부터 카드 데이터(JSON)를 받아 `Resources/Data/Raw/cards.json`에 저장합니다.
3.  **Runtime Loader (`DataLoader`)**: 게임 시작 시 JSON을 로드하여 `SharedLogic` 객체로 역직렬화합니다.

### 로직 실행 (Behavior)
*   **TS-to-CS 전략**: 별도의 팩토리(Factory) 클래스를 거치지 않고, 자동 생성된 DTO(`Component`)에 `partial class`로 동작을 직접 주입합니다.
*   **파일 위치**: `Assets/Scripts/Core/Logic/ComponentBehavior.cs`
*   **실행 방식**: `Component.Execute(ILogicContext)` 호출 시, 내장된 `Effect` 키워드("ATTACK" 등)에 따라 동작을 수행합니다.

## 2. 사용 방법 (Usage)

### 에디터 툴 (Editor Tools)
상단 메뉴 `Tools > LogicForge`에서 접근 가능합니다.
*   **Sync Schema**: `SharedSchema.cs` (C# 클래스) 동기화.
*   **Sync Cards**: `cards.json` (실제 데이터) 동기화.

*설정 파일은 `Assets/Resources/LogicForgeSettings` (또는 Editor 폴더)에 위치하며 API Key 입력이 필요합니다.*

### 검증 시뮬레이션 (Simulation)
`Assets/Scripts/Core/Simulation/LogicSimulator.cs`를 사용하여 로드된 데이터의 로직 수행을 테스트할 수 있습니다.
