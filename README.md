# 연금술사 오토 배틀러 (Alchemist Auto-Battler)

유니티(Unity) 기반의 데이터 주도형 로그라이크 오토 배틀러 프로젝트입니다.

## 🚀 바로가기

- **GitHub (Unity)**: [tglkwon/calchemy-unity](https://github.com/tglkwon/calchemy-unity.git)
- **Live Demo**: [Link](https://tglkwon.github.io/Calchemy.io/)

---

## 📖 문서 도서관 (Documentation Index)

### 🎨 게임 디자인 (Game Design)
기획 팀을 위한 핵심 규칙과 데이터 정의입니다.
- [핵심 메커니즘](docs/game_design/core_mechanics.md): 원소 및 전투 루프 정의.
- [그리드 및 빙고](docs/game_design/grid_bingo.md): 4x4 마방진 및 빙고 시너지.
- [콘텐츠 명세](docs/game_design/content_spec.md): 카드, 유물, 아이템 데이터 구조.
- [진행 보상](docs/game_design/progression.md): 맵 시스템 및 보상 구조.

### 🛠 기술 설계 (Technical Docs)
개발 팀을 위한 아키텍처 및 시스템 사양입니다.
- [유니티 아키텍처](docs/technical/architecture.md): C# 프로젝트 구조 및 시스템 설계.
- [빙고 확장성 전략](docs/technical/bingo_extensibility.md): 가변 그리드 및 패턴 매칭 설계.
- [로직 레퍼런스](docs/technical/logic_reference.md): YAML/JSON 데이터 작성을 위한 키워드 사전.
- [LogicForge 연동](docs/technical/logicforge_integration.md): Web-Unity 데이터 파이프라인 및 로직 구현 구조. ⚠️ (검증 대기)

### 📜 프로젝트 관리 (History)
- [히스토리](docs/history/milestones.md): 개발 마일스톤 및 변경 내역.

---

## 🤖 AI 개발 지침
프로젝트 자동화 및 개발 규칙은 `.agent/` 폴더 내의 지침을 따릅니다.
- [.agent/rules/](.agent/rules/): 코딩 컨벤션 및 보안 규칙.
- [.agent/workflows/](.agent/workflows/): 자동화 워크플로우.
