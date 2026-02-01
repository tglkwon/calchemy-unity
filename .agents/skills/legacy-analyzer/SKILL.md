# Skill: Legacy Logic Analyzer

이 스킬은 React/JavaScript 기반의 레거시 코드를 Unity/C# 아키텍처로 변환하는 데 필요한 분석 기법과 변환 패턴을 제공합니다.

## 1. 패턴 변환 팁

### 데이터 매핑 (Data Mapping)
- **JS Object**: `const card = { id: 1, name: 'Fire' }`
- **Unity C#**: `[CreateAssetMenu] public class CardData : ScriptableObject`

### 컬렉션 처리 (Collections)
- **JS Array Methods**: `map`, `filter`, `find`
- **Unity C# (LINQ)**: `Select`, `Where`, `FirstOrDefault`
  - *주의: 퍼포먼스 이슈가 있을 수 있는 루프에서는 일반 for/foreach 권장*

### 비동기 로직 (Asynchronous)
- **JS Promise / async-await**
- **Unity**: `IEnumerator (Coroutine)` 또는 `UniTask`

## 2. 로직 분석 체크리스트
- [ ] 하드코딩된 수치가 있는가? -> `Constants.cs` 또는 `ScriptableObject`로 분리
- [ ] 사이드 이펙트(State 변경)가 어디서 발생하는가? -> `BattleManager`의 순차 처리에 반영
- [ ] UI 이벤트와 결합되어 있는가? -> `GameEvents`를 통한 비결합화 처리

## 3. 권장 검색 툴 사용법
- `grep_search(Query: "function_name", SearchPath: "transfer/")`
- `view_file`을 통해 전체 맥락 파악 전, `view_file_outline`으로 구조 파악
