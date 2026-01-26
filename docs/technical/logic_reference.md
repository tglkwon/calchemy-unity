# 로직 레퍼런스 (Logic Reference)

기획 데이터(YAML/JSON) 작성 시 사용하는 핵심 키워드 레퍼런스입니다.

## 1. 일반 효과 (Standard Effects)
| 키워드 (Key) | 설명 | 파라미터 예시 |
| :--- | :--- | :--- |
| `ATTACK` | 대상에게 피해를 줌 | `{ "dmg": 10 }` |
| `BLOCK` | 방어도를 획득함 | `{ "amount": 5 }` |
| `HEAL` | 체력을 회복함 | `{ "amount": 8 }` |

## 2. 그리드 조작 (Grid Manipulation)
`GRID_MANIPULATION` 객체 하위에서 사용되는 상세 동작입니다.

### Action
- `TRANSFORM`: 원소 속성 변환.
- `SWAP`: 위치 교환.
- `REPLACE`: 카드 교체.

### Target
- `UP`, `DOWN`, `LEFT`, `RIGHT`: 인접 방향.
- `NEAR_4`, `NEAR_8`: 범위형 선택.
- `RANDOM`, `ALL`: 전역 선택.

## 3. 작성 예시
```yaml
# 십자 범위 카드 속성 변환
GRID_MANIPULATION:
  action: TRANSFORM
  target: NEAR_4
  toType: ORIGIN
```
