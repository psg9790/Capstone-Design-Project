using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
/*
public class InventoryUI : MonoBehaviour
{
    private GraphicRaycaster _gr;
private PointerEventData _ped;
private List<RaycastResult> _rrList;

private ItemSlotUI _beginDragSlot; // 현재 드래그를 시작한 슬롯
private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
private int _beginDragSlotSiblingIndex;

private void Update()
{
    _ped.position = Input.mousePosition;

    OnPointerDown();
    OnPointerDrag();
    OnPointerUp();
}

private T RaycastAndGetFirstComponent<T>() where T : Component
{
    _rrList.Clear();

    _gr.Raycast(_ped, _rrList);

    if(_rrList.Count == 0)
        return null;

    return _rrList[0].gameObject.GetComponent<T>();
}

private void OnPointerDown()
{
    // Left Click : Begin Drag
    if (Input.GetMouseButtonDown(0))
    {
        _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // 아이템을 갖고 있는 슬롯만 해당
        if (_beginDragSlot != null && _beginDragSlot.HasItem)
        {
            // 위치 기억, 참조 등록
            _beginDragIconTransform = _beginDragSlot.IconRect.transform;
            _beginDragIconPoint = _beginDragIconTransform.position;
            _beginDragCursorPoint = Input.mousePosition;

            // 맨 위에 보이기
            _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
            _beginDragSlot.transform.SetAsLastSibling();

            // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
            _beginDragSlot.SetHighlightOnTop(false);
        }
        else
        {
            _beginDragSlot = null;
        }
    }
}
/// <summary> 드래그하는 도중 </summary>
private void OnPointerDrag()
{
    if(_beginDragSlot == null) return;

    if (Input.GetMouseButton(0))
    {
        // 위치 이동
        _beginDragIconTransform.position =
            _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
    }
}
/// <summary> 클릭을 뗄 경우 </summary>
private void OnPointerUp()
{
    if (Input.GetMouseButtonUp(0))
    {
        // End Drag
        if (_beginDragSlot != null)
        {
            // 위치 복원
            _beginDragIconTransform.position = _beginDragIconPoint;

            // UI 순서 복원
            _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

            // 드래그 완료 처리
            EndDrag();

            // 참조 제거
            _beginDragSlot = null;
            _beginDragIconTransform = null;
        }
    }
}
}
*/

