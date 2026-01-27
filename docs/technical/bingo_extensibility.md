# 빙고 시스템 확장성 전략 (Bingo Extensibility Strategy)

이 문서는 고정된 4x4 그리드를 넘어, 가변적인 그리드 크기와 복합적인 빙고 패턴(T자형, L자형 등)을 수용하기 위한 기술적 설계 전략을 제안합니다.

## 1. 아키텍처 핵심 가시성 (Key Abstractions)

### ① 가변 그리드 데이터 (Dynamic Grid Data)
- **문제**: 현재 `grid[4,4]` 배열이 고정되어 있어 크기 변경이 어려움.
- **해결**: `GridData` ScriptableObject를 도입하여 행(Rows), 열(Columns) 정보를 데이터화함.
- **데이터 구조**:
  ```csharp
  public class GridConfiguration : ScriptableObject {
      public int width;
      public int height;
  }
  ```

### ② 패턴 기반 빙고 조건 (Pattern-Based Bingo)
- **문제**: 가로/세로/대각선이 코드로 하드코딩되어 있음.
- **해결**: **패턴 오프셋(Pattern Offset)** 방식을 도입. 빙고를 '특정 좌표들의 집합'으로 정의함.
- **예시 (T자형)**: `(0,0), (1,0), (2,0), (1,1)` 오프셋 리스트로 정의.

## 2. 인터페이스 설계

### `IBingoCondition` 인터페이스
다양한 빙고 승리 조건을 전략 패턴으로 관리합니다.

```csharp
public interface IBingoCondition {
    // 특정 라인/패턴이 빙고 조건(모두 동일 원소, 4원소 조화 등)을 만족하는지 검사
    bool Evaluate(List<UnitEntity> units);
}
```

### `BingoPatternAsset` (ScriptableObject)
빙고의 '모양'을 정의하는 데이터 에셋입니다.

```csharp
[CreateAssetMenu]
public class BingoPatternAsset : ScriptableObject {
    public string patternName;
    public Vector2Int[] offsets; // 패턴을 구성하는 상대 좌표들
    public IBingoCondition condition; // 이 패턴에 적용될 조건
}
```

## 3. 매칭 엔진 작동 방식

1. **그리드 스캔**: 엔진이 그리드의 모든 셀을 순회합니다.
2. **패턴 오버레이**: 각 셀을 기점으로 등록된 `BingoPatternAsset`들을 겹쳐봅니다.
3. **조건 검사**: 패턴 내의 유닛들이 `IBingoCondition`을 만족하는지 확인합니다.
4. **결과 반환**: 만족된 패턴과 해당 유닛 리스트를 반환하여 효과를 처리합니다.

## 4. 기대 효과 및 확장 케이스

- **맵별 특성**: 특정 스테이지에서는 '3x3 그리드'만 사용하거나, 'T자 빙고만 인정'하는 등의 제약 조건을 데이터 수정만으로 구현 가능.
- **유물 시너지**: 특정 유물을 획득하면 'ㄴ자 빙고'가 추가로 활성화되는 로직을 코드 수정 없이 에셋 추가로 대응.
- **테트리스 방식**: 테트리스 블록 모양의 다양한 패턴을 기획자가 `ScriptableObject` 에셋으로 직접 생성 및 밸런싱 가능.

## 5. 단계별 구현 제안
1. **1단계**: `GridManager`에서 크기 변수를 상수가 아닌 변수로 전환.
2. **2단계**: 라인 판정 로직을 `Vector2Int[]` 오프셋 스캔 방식으로 리팩토링.
3. **3단계**: 기획 데이터를 통해 외부에서 패턴을 주입받는 시스템 구축.
