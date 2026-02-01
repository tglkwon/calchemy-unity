# LogicForge 연동 시스템 (LogicForge Integration)

LogicForge(Web)에서 정의된 카드 데이터와 로직 스키마를 Unity 프로젝트로 동기화하고 실행하는 시스템입니다.

> [!NOTE]
> **검증 완료 (Verified)**
> 데이터 스키마 동기화, JSON 파싱, 리소스 링킹(ResourceManager) 기능이 검증되었습니다. (2026.02.01)

## 1. 시스템 구조 (Architecture)

### 데이터 파이프라인 (Data Pipeline)
1.  **Schema Sync (`LogicForgeGenerator`)**: API로부터 C# 스키마 코드를 받아 `SharedSchema.cs`를 생성/갱신합니다.
2.  **Data Sync (`LogicForgeSync`)**: API로부터 카드 데이터(JSON)를 받아 `Resources/Data/Raw/cards.json`에 저장합니다.
3.  **Runtime Loader (`DataLoader`)**: 게임 시작 시 JSON을 로드하고 `ResourceManager`를 통해 이미지를 캐싱합니다.

### 로직 실행 (Behavior)
*   **TS-to-CS 전략**: 자동 생성된 DTO(`Component`)에 `partial class`로 동작을 확장(`ComponentBehavior.cs`)하거나 확장 메서드(`ComponentExtensions.cs`)를 사용합니다.
*   **파일 위치**: `Assets/Scripts/Calchemy/Logic/ComponentBehavior.cs`
*   **실행 방식**: `Component.Execute(ILogicContext)` 호출 시, 내장된 `Effect` 키워드("ATTACK" 등)에 따라 동작을 수행합니다.

## 2. 사용 방법 (Usage)

### 에디터 툴 (Editor Tools)
상단 메뉴 `Tools > LogicForge`에서 접근 가능합니다.
*   **Sync Schema**: `SharedSchema.cs` (C# 클래스) 동기화.
*   **Sync Cards**: `cards.json` (실제 데이터) 동기화 (이미지 자동 연동 포함).

*설정 파일은 `Assets/Resources/LogicForgeSettings` (또는 Editor 폴더)에 위치하며 API Key 입력이 필요합니다.*

### 검증 시뮬레이션 (Simulation)
`Assets/Scripts/Calchemy/Systems/LogicSimulator.cs`를 사용하여 로드된 데이터의 로직 수행을 테스트할 수 있습니다.
