# 개발 히스토리 (History)

## 🏁 마일스톤
- **2026.01.26**: 유니티(Unity) 아키텍처 수립 및 문서 구조 리팩토링 (Hybrid Documentation 도입).
- **2026.01.24**: React 기반 레거시 기능(상점, 카드 강화 등) 개발 완료.

## ✅ 변경 내역
### [2026.02.01] 데이터 연동 시스템 구축 및 리팩토링
- **리소스 시스템**: `ResourceManager` 싱글톤 구현 및 `DataLoader` 연동 (ID 기반 이미지 자동 로드 및 Fallback 처리).
- **구조 리팩토링**: 중복된 `Assets/Scripts/Core` 폴더를 제거하고 `Assets/Scripts/Calchemy`로 통합. 네임스페이스 일관성 확보.
- **데이터 검증**: LogicForge `SharedLogic` 및 `Component` 스키마 복구 및 데이터 파싱 로직 검증 완료.

### [2026.01.26] 유니티 이식 시작 및 환경 최적화
- 유니티 C# 기반 `Core`, `Systems`, `Entities` 폴더 구조 생성.
- `GridManager`, `BattleManager`, `EffectProcessor` 뼈대 구현.
- `README.md` 인덱스화 및 `docs/` 주제별 문서 분리.
- 유니티 표준 `.gitignore` 설정을 통한 Git 저장소 용량 최적화.
- 레거시 `CardSystem.js` 기반 빙고 판정(대각선, 조화 빙고) 및 그리드 조작(반사 타겟팅) 로직 이식.
