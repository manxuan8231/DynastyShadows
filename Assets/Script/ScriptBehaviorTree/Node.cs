using System;
using System.Collections.Generic;
using UnityEngine;

// Danh sách các trạng thái của Node
public enum NodeState
{
    RUNNING, // đang chạy
    SUCCESS, // thành công
    FAILURE // thất bại
}

// Lớp Node đại diện cho một nút trong cây hành vi
public abstract class Node
{
    protected NodeState state;
    public NodeState CurrentState => state;
    public abstract NodeState Evaluate(); // Tick
}
// Lớp SelectorNode đại diện cho một nút lựa chọn trong cây hành vi
public class SelectorNode : Node
{
    // Danh sách các con nút
    protected List<Node> children = new List<Node>();

    public SelectorNode(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        // Duyệt qua tất cả các con nút
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    // Nếu con nút thất bại, tiếp tục với nút tiếp theo
                    continue;
                case NodeState.SUCCESS:
                    state = NodeState.SUCCESS; // Nếu con nút thành công, trả về thành công
                    return state;
                case NodeState.RUNNING:
                    state = NodeState.RUNNING; // Nếu con nút đang chạy, trả về trạng thái đang chạy
                    return state;
                default: continue;
            }
        }

        state = NodeState.FAILURE; // Mặc định là thất bại
        return state;
    }
}
// Lớp SequenceNode đại diện cho một nút chuỗi trong cây hành vi
public class SequenceNode : Node
{
    // Danh sách các con nút
    protected List<Node> children = new List<Node>();

    public SequenceNode(List<Node> children)
    {
        this.children = children;
    }

    // Nút này sẽ thực hiện các con nút theo thứ tự và trả về trạng thái phù hợp
    public override NodeState Evaluate()
    {
        var anyChildRunning = false; // Biến để kiểm tra xem có con nút nào đang chạy hay không
        // Duyệt qua tất cả các con nút
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.FAILURE; // Nếu con nút thất bại, trả về thất bại
                    return state;
                case NodeState.SUCCESS:
                    continue; // Nếu con nút thành công, tiếp tục với nút tiếp theo
                case NodeState.RUNNING:
                    anyChildRunning = true; // Nếu con nút đang chạy, đánh dấu là đang chạy
                    continue;
                default:
                    state = NodeState.FAILURE; // Mặc định là thất bại
                    return state;
            }
        }

        state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        // Nếu có con nút đang chạy, trả về RUNNING, nếu không thì SUCCESS
        return state;
    }
}

// Lớp ConditionNode đại diện cho một nút điều kiện trong cây hành vi
public class ConditionNode : Node
{
    private Func<bool> condition; // Điều kiện để kiểm tra

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        state = condition() ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}

//hành động
public class ActionNode : Node
{
    private Func<NodeState> action; // Hành động để thực hiện

    public ActionNode(Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        state = action();
        return state;
    }
}