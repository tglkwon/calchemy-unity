---
description: 실행 중인 유니티 MCP 서버를 안전하게 종료하는 워크플로우
---

유니티 MCP 서버가 실행 중인지 확인하고 종료합니다.

### 1단계: 서버 작동 여부 확인
// turbo
1. 다음 명령어를 실행하여 8080 포트를 점유 중인 프로세스 ID(PID)를 찾습니다.
   `powershell.exe -Command "(Get-NetTCPConnection -LocalPort 8080 -ErrorAction SilentlyContinue).OwningProcess"`

### 2단계: 조건부 종료
- 만약 PID가 반환된다면:
  - 해당 프로세스를 강제 종료합니다.
  // turbo
  - `powershell.exe -Command "Stop-Process -Id (Get-NetTCPConnection -LocalPort 8080).OwningProcess -Force"`
  - "유니티 MCP 서버를 종료했습니다."라고 보고합니다.
- 만약 PID가 없다면:
  - "현재 실행 중인 유니티 MCP 서버가 없습니다."라고 사용자에게 알립니다.
