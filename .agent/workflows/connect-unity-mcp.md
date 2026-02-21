---
description: 유니티 MCP 서버(mcp-for-unity)를 실행하고 연결하는 워크플로우
---

유니티 MCP 서버를 연결하기 전 상태를 확인하고 실행합니다.

### 1단계: 서버 상태 및 프로세스 확인
// turbo
1. 다음 명령어를 실행하여 8080 포트가 이미 사용 중인지 확인합니다.
   `netstat -ano | findstr :8080`

2. (선택 사항) 서버가 응답하는지 헬스 체크를 수행합니다.
   `curl -I http://127.0.0.1:8080/mcp`

### 2단계: 조건부 실행
- 만약 포트가 사용 중이고 헬스 체크에 성공한다면:
  - "유니티 MCP 서버가 이미 정상적으로 실행 중입니다 (http://127.0.0.1:8080/mcp)."라고 사용자에게 알리고 종료합니다.
- 만약 포트는 사용 중이지만 응답이 없거나, 포트가 비어 있다면:
  - 기존 프로세스가 있다면 종료한 후(필요 시), 다음 단계로 진행합니다.

### 3단계: 서버 실행
// turbo
1. 유니티 MCP 서버를 백그라운드에서 실행합니다.
   `/c/Users/AquaCo/.local/bin/uvx --prerelease explicit --from "mcpforunityserver>=0.0.0a0" mcp-for-unity --transport http --http-url http://127.0.0.1:8080 --project-scoped-tools`

2. 실행 후 잠시 기다린 후 `http://127.0.0.1:8080/mcp` 주소를 안내합니다.
