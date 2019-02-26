using System;
using System.Collections.Generic;

namespace Problem
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue q = new Queue();

            for (int i = 0; i <= 3; i++)
            {
                q.Enqueue(i);
            }

            q.Dequeue();

            q.Enqueue(0);
           
            q.Print();
        }
    }

    public sealed class Queue
    {
        Stack<int> stack1 = new Stack<int>();
        Stack<int> stack2 = new Stack<int>();

        // Boolean field is flag, indicates if stack is ready for dequeue or not.
        private bool inDequeueOrder;

        /// <summary>
        /// Print this instance. Order insensitive due to alternating stacks.
        /// Used built-in IEnumerable from Stack<T> to print.
        /// </summary>
        public void Print()
        {
            if (stack1.Count == 0 && stack2.Count == 0)
            {
                throw new InvalidOperationException("No items in Queue");
            }

            if (stack1.Count > 0)
            {
                foreach (var i in stack1)
                {
                    Console.Write($"{i} ");
                }
                Console.WriteLine();
            }
            else
            {
                foreach (var i in stack2)
                {
                    Console.Write($"{i} ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Enqueue the specified num into Queue.
        /// Uses boolean flag. If NOT in Dequeue order, stack is ready for Enqueue.
        /// Otherwise, reverses stack. Avoids repeated calls to ReverseStack O(n).
        /// </summary>
        /// <param name="num">Number.</param>
        public void Enqueue(int num)
        {
            if (stack1.Count == 0 && stack2.Count == 0)
            {
                stack1.Push(num);
                inDequeueOrder = true;
            }

            // Stack 1 is full (active).
            else if (stack1.Count > 0)
            {
                if (!inDequeueOrder) // In Enqueue order.
                {
                    stack1.Push(num);
                }
                else // Not in Enqueue order.
                {
                    ReverseStack(stack1, stack2);
                    stack2.Push(num);
                    inDequeueOrder = false;
                }
            }

            // Stack 2 is full (active).
            else 
            {
                if (!inDequeueOrder)
                {
                    stack2.Push(num);
                }
                else
                {
                    ReverseStack(stack2, stack1);
                    stack1.Push(num);
                    inDequeueOrder = false;
                }
            }
        }

        /// <summary>
        /// Dequeue item from Queue. 
        /// Uses boolean flag. Only Dequeues from stack if stack is in Dequeue order.
        /// Otherwise reverses the stack. Avoids repeated calls to ReverseStack O(n).
        /// </summary>
        /// <returns>The dequeue.</returns>
        public int Dequeue()
        {
            int popped;
       
            if (stack1.Count == 0 && stack2.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            // Stack 1 is full (active).
            if (stack1.Count > 0)
            {
                if (inDequeueOrder) 
                {
                    popped = stack1.Pop();
                }
                else // Not in Dequeue order.
                {
                    ReverseStack(stack1, stack2);
                    popped = stack2.Pop();
                    inDequeueOrder = true;
                }
            }

            // Stack 2 is full (active).
            else
            {
                if (inDequeueOrder)
                {
                    popped = stack2.Pop();
                }
                else
                {
                    ReverseStack(stack2, stack1);
                    popped = stack1.Pop();
                    inDequeueOrder = true;
                }
            }
            return popped;
        }
        /// <summary>
        /// Helper to avoid repeating same while loop.
        /// </summary>
        /// <param name="full">Full stack.</param>
        /// <param name="empty">Empty stack.</param>
        private void ReverseStack(Stack<int> full, Stack<int> empty)
        {
            while (full.Count > 0)
            {
                empty.Push(full.Pop());
            }
        }
    }
}

/*
 * [Test Cases]
 * 
 * Dequeue on empty...exception thrown.
 * 
 * Alernating enqueue, dequeue with only one item: O(2n) -> O(n).
 * e, d, e, d, ... n
 * No reverse operations.
 * 
 * Sequential add, then remove: O(3n) - > O(n)
 * eee...n, ddd...n
 * One big reverse operation.
 * 
 * Sequential add, then alternate, then remove.
 * eee...n, d, e, d, e, ddd...n
 * This is expensive: O(n*a) where a is the number of alternating operations.
 * O(n) add + O(n*a) alternate + O(n) dequeue.
 * 
 * O(n) best/average, O(n*a) worst case.
 */
