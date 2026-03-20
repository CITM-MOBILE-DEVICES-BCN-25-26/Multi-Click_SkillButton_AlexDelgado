# Multi-Click Skill Button

## Thresholds

- Long press threshold: 500ms
- Double click threshold: 220ms

## Button Flow

- First press for at least 500ms triggers `Long Press`.
- First release before 500ms starts a 220ms double-click time window.
- If no second press arrives before the window expires, the system triggers `Short Click`.
- If a second press arrives in time and is released before 500ms, the system triggers `Double Click`.
- If the second press is held for at least 500ms, pending short/double detection is canceled and the system triggers `Long Press`.


Postdata para Eric: No te voy a mentir se me olvidó por completo la entrega estos dias y lo he hecho esta mañana :^|