using System.Collections.Generic;

namespace BehaviorTree
{
    public class RandomSelector : Selector
    {
        public RandomSelector() : base() { }
        public RandomSelector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            Shuffle(children);

            return base.Evaluate();
        }
        public List<Node> Shuffle(List<Node> _list)     // 리스트 셔플 -> 랜덤한 순서로 자식노드 실행
        {
            for (int i = _list.Count - 1; i > 0; i--)
            {
                int rnd = UnityEngine.Random.Range(0, i);

                Node temp = _list[i];
                _list[i] = _list[rnd];
                _list[rnd] = temp;
            }

            return _list;
        }
    }
}

