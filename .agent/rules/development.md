# 개발 규칙: Legacy-First Development

이 규칙은 `calchemy-unity` 프로젝트의 일관성을 유지하고 중복 개발을 방지하기 위해 모든 기능 구현 전에 반드시 준수해야 합니다.

## 1. 구현 전 레거시 검토 (Pre-Implementation Review)
- 새로운 C# 클래스나 메서드를 설계하기 전, 반드시 `transfer/` 폴더 내에서 관련 로직이 있는지 검색합니다.
- `grep_search` 또는 `find_by_name` 툴을 활용하여 유사한 키워드(예: `Bingo`, `Effect`, `Grid`)를 먼저 조사합니다.

## 2. 분석 및 보고
- 레거시 로직을 발견했다면, 이를 어떻게 유니티(C#, MONO, SO)에 맞게 변환할지 계획을 세웁니다.
- 만약 레거시 로직과 다르게 구현해야 한다면, 그 타당한 이유를 사용자에게 설명해야 합니다.

## 3. 파일 매핑 가이드
- **JS Entities/State** -> **Unity ScriptableObject / UnitEntity**
- **JS Systems/Logic** -> **Unity Systems / MonoBehavior Managers**
- **JS UI/React Components** -> **Unity UI / Canvas Systems**

## 4. 유니티 MCP 서버 관리 (MCP Management)
- 유니티와의 세션이 필요한 작업을 수행하기 전, 반드시 `/check-unity-mcp`를 통해 서버 상태를 확인하십시오.
- 서버가 종료되어 있다면 `/connect-unity-mcp`를 사용하여 연결을 재개하십시오.
- 작업 종료 후 시스템 자원 확보를 위해 `/disconnect-unity-mcp`를 사용할 수 있습니다.
- 세션이 끊긴 경우(예: `mcp_not_found` 에러), 지체 없이 상태 확인 및 재연결 프로세스를 실행하십시오.

> [!IMPORTANT]
> "이미 구현된 것이 있는가?"에 대한 답을 하기 전까지는 코드를 작성하지 마십시오.
