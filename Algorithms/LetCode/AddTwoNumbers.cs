public class Solution 
{
    public static ListNode AddTwoNumbers(ListNode l1, ListNode l2) 
    {	
        var newNode = new ListNode();
        var current = newNode;
        int carry = 0, sum = 0;

        while (l1 != null || l2 != null) 
        {
            sum = carry + (l1?.val ?? 0) + (l2?.val ?? 0);
            carry = sum / 10;

            current.next = new ListNode(sum % 10);
            current = current.next;

            l1 = l1?.next;
            l2 = l2?.next;
        }

        if (carry > 0) 
            current.next = new ListNode(carry);

        return newNode.next;
    }

    public static void Main()
    {
        var l1 = new ListNode(2);
        l1 = new ListNode(4, l1);
        l1 = new ListNode(3, l1);
        
        var l2 = new ListNode(4);
        l2 = new ListNode(6, l2);
        l2 = new ListNode(5, l2);

        var result = AddTwoNumbers(l1, l2);

        Console.WriteLine(result);
    }
}

public class ListNode 
{
    public int val;
    public ListNode next;
	
    public ListNode(int val = 0, ListNode next = null) 
	{
        this.val = val;
        this.next = next;
    }
}

// You are given two non-empty linked lists representing two non-negative integers. 
// The digits are stored in reverse order and each of their nodes contain a single digit. 
// Add the two numbers and return it as a linked list.

//You may assume the two numbers do not contain any leading zero, except the number 0 itself.
//Example:
//Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
//Output: 7 -> 0 -> 8
//Explanation: 342 + 465 = 807.

// Time complexity : O(max(m, n)), Assume that m and n represents the length of l1 and l2 respectively