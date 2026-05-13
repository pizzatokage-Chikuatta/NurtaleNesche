# Drop Table 필드 가이드

이 문서는 `drop_table.example.training_box.json`을 읽기 위한 설명입니다.

## 이 예시가 드롭하는 것

예시는 항상 `item.example.training_arrow`를 1개 드롭합니다. 그 다음 랜덤 roll을 한 번 합니다:

- `itemId: null`, `weight: 3`: 추가 아이템 없음.
- `item.lockpick`, `weight: 2`: lockpick 1개.
- `item.potion.example.training_tonic`, `weight: 1`, `hasBrokenChance: false`: 완성품 Training Tonic 1개.

weight는 상대값입니다. 총 weight가 3 + 2 + 1이므로, 이 한 번의 roll은 추가 아이템 없음 50%, lockpick 약 33.3%, tonic 약 16.7%입니다.

## 필드

- `id`: 다른 record가 이 drop table을 요청할 때 쓰는 id입니다.
- `guaranteedItem`: 이 table이 사용될 때마다 항상 추가되는 item 목록입니다.
- `guaranteedEquipment`: 이 table이 사용될 때마다 항상 추가되는 equipment 목록입니다.
- `groups`: 랜덤 roll 구간입니다.
- `rolls`: 해당 group의 `entries`에서 몇 번 선택할지입니다.
- `entries`: 한 group 안에서 선택 가능한 항목입니다.
- `itemId`: 이 entry가 선택되었을 때 생성할 item입니다. `null`은 그 roll에서 아무것도 생성하지 않습니다.
- `weight`: 같은 group 안의 다른 entry들과 비교하는 상대 확률입니다.
- `hasBrokenChance`: false면 해당 drop에서 item broken chance를 건너뛰고, 생략하거나 true면 일반 broken chance 동작을 사용합니다.