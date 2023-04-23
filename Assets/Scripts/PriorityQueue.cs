using System;
using System.Collections.Generic;

// https://jiwanm.github.io/algorithm%20lesson%202/chapter5-2/
class PriorityQueue<T> where T : IComparable<T>
{
    // 힙 트리는 배열로 관리할 수 있다.
    List<T> _heap = new List<T>();

    public void Push(T data)
    {
        // 힙의 맨 끝에 새로운 데이터를 삽입한다.
        _heap.Add(data);

        int now = _heap.Count - 1; // 추가한 노드의 위치. 힙의 맨 끝에서 시작.

        // 위로 도장 깨기 시작
        while (now > 0)
        {
            int next = (now - 1) / 2; // 부모 노드
            if (_heap[now].CompareTo(_heap[next]) < 0) // 부모 노드와 비교
                break;

            // 두 값을 서로 자리 바꿈
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            // 검사 위치로 이동한다.
            now = next;
        }
    }

    public T Pop() // 최대값(루트)을 뽑아낸다.
    {
        // 반환할 데이터를 따로 저장
        T ret = _heap[0];

        // 마지막 데이터를 루트로 이동시킨다.
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);
        lastIndex--;

        // 아래로 도장 깨기 시작
        int now = 0;
        while (true)
        {
            int left = 2 * now + 1;
            int right = 2 * now + 2;

            int next = now;
            // 왼쪽 값이 현재값보다 크면, 왼쪽으로 이동
            if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                next = left;
            // 오른쪽 값이 현재값(왼쪽 이동 포함)보다 크면, 오른쪽으로 이동
            if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                next = right;

            // 왼쪽/오른쪽 모두 현재값보다 작으면 종료
            if (next == now)
                break;

            // 두 값 서로 자리 바꿈
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            // 검사 위치로 이동한다.
            now = next;
        }

        return ret;
    }

    public int Count()
    {
        return _heap.Count;
    }

    public void Clear()
    {
        _heap.Clear();
    }

    public T Top()
    {
        return _heap[0];
    }

    public bool Empty()
    {
        return (_heap.Count > 0) ? false : true;
    }
}