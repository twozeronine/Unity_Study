# 2. C# 자료구조와 알고리즘

복습하거나 새로운 학습 내용만 적었습니다.

### 개요

자료구조와 알고리즘 학습을 통해 길찾기 알고리즘을 구현하고 실제 게임에서도 길찾기 능력을 갖는 알고리즘을 구현하는 것이 목표인 강의입니다.

![image](https://cdn.inflearn.com/public/files/courses/324727/2d8d5911-b35c-485e-a245-ac242149c9ac/rookiss-pt2-2.gif)

---

## BinaryTreeAlgorithm

![이진트리알고리즘](https://user-images.githubusercontent.com/67315288/119761091-2a844880-bee6-11eb-8936-2e0df2e624fe.png)

이진트리를 활용한 미로 만들기이다.  
점 하나를 트리의 노드로 가정하였을때 랜덤한 확률로 x좌표나 y좌표로 1씩 증가한 노드로 접근하여 벽 혹은 뚫린 공간으로 만든다.

![이진트리](https://user-images.githubusercontent.com/67315288/119761651-286eb980-bee7-11eb-990f-b91f7e3e0b56.png)

하지만 마지막 size-1 노드에서는 더이상 진행 할 구간이 없어 한쪽 방향으로만 진행하게 되어 약간은 단조로운 패턴이 될 수 있다는 단점이 있다.
