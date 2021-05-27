# 2. C# 자료구조와 알고리즘

복습하거나 새로운 학습 내용만 적었습니다.

### 개요

자료구조와 알고리즘 학습을 통해 길찾기 알고리즘을 구현하고 실제 게임에서도 길찾기 능력을 갖는 알고리즘을 구현하는 것이 목표인 강의입니다.

![image](https://cdn.inflearn.com/public/files/courses/324727/2d8d5911-b35c-485e-a245-ac242149c9ac/rookiss-pt2-2.gif)

---

## 미로 초기 맵 생성

![초기 맵생성](https://user-images.githubusercontent.com/67315288/119766376-dda56f80-beef-11eb-9da6-42c1bdbb27d2.png)

테두리를 모두 벽으로 막고 홀수 좌표만 빈공간을 만든다.  
이후에 미로 생성 알고리즘에서 짝수 좌표의 처리는 하지않고 홀수번째의 좌표만 연산하여 미로를 생산한다.

## BinaryTreeAlgorithm

![이진트리알고리즘](https://user-images.githubusercontent.com/67315288/119761091-2a844880-bee6-11eb-8936-2e0df2e624fe.png)

이진트리를 활용한 미로 만들기이다.  
점 하나를 트리의 노드로 가정하였을때 랜덤한 확률로 x좌표나 y좌표로 1씩 증가한 노드로 접근하여 벽 혹은 뚫린 공간으로 만든다.

![이진트리](https://user-images.githubusercontent.com/67315288/119761651-286eb980-bee7-11eb-990f-b91f7e3e0b56.png)

하지만 약간은 단조로운 패턴이 될 수 있다는 단점이 있다.

## SideWinderAlgorithm

![사이드와인더 알고리즘](https://user-images.githubusercontent.com/67315288/119765675-a5516180-beee-11eb-91bf-ca08c1a38225.png)

SideWinderAlgorithm을 통한 미로 만들기이다.
각 칸마다 x좌표 혹은 y좌표로 랜덤으로 선택하여 진행하는데 x좌표로 진행하게 되면 count 갯수를 세며 진행하고 만약 다음 진행시 y좌표 진행이 나온다면 여태까지 x좌표로 진행해왔던 노드중 하나를 선택하여 y방향으로 진행시킨다.

이진트리를 활용한 알고리즘보다 다채로운 패턴을 보여준다.

## 오른손 법칙

![Animation1](https://user-images.githubusercontent.com/67315288/119799582-291f4400-bf17-11eb-8467-1703676e16e9.gif)

미로찾기 알고리즘 중 하나인 우선법을 사용해 미로를 찾는다.

## 그래프

![그래프](https://user-images.githubusercontent.com/67315288/119821139-2fb8b600-bf2d-11eb-8925-484119ebe0ce.png)

길 찾기 알고리즘을 구현하기 위한 자료구조인 그래프
