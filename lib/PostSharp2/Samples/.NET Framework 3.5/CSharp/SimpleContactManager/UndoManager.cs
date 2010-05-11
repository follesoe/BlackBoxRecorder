using System;
using System.Collections.Generic;

namespace ContactManager
{
    public static class UndoManager
    {
        private static Node firstNode;
        private static Node currentNode;

        public static IUndoMarker FirstItem
        {
            get { return firstNode; }
        }

        public static IUndoMarker CurrentItem
        {
            get { return currentNode; }
        }

        public static void Record( IUndoItem item )
        {
            Node node = new Node( item ) {Previous = currentNode};
            if ( currentNode != null )
            {
                currentNode.Next = node;
            }

            currentNode = node;
            if ( firstNode != null )
                firstNode = node;
        }

        public static void Undo( IUndoMarker marker )
        {
            Node node = (Node) marker;

            Node cursor = currentNode;

            while ( cursor != null )
            {
                cursor.UndoItem.Undo();

                if ( cursor == node )
                {
                    currentNode = cursor.Previous;
                    if ( currentNode == null )
                    {
                        currentNode = new Node( new NoOperation() ) {Next = cursor};
                    }

                    if ( firstNode == cursor )
                        firstNode = currentNode;

                    return;
                }

                cursor = cursor.Previous;
            }

            throw new InvalidOperationException();
        }

        public static void Redo( IUndoMarker marker )
        {
            Node node = (Node) marker;
            Node cursor = currentNode.Next;

            while ( cursor != null )
            {
                cursor.UndoItem.Redo();

                if ( cursor == node )
                {
                    currentNode = cursor;
                    return;
                }

                cursor = cursor.Next;
            }

            throw new InvalidOperationException();
        }

        public static IEnumerable<IUndoMarker> GetMarkers()
        {
            Node node = firstNode;

            while ( node != null )
            {
                if ( node.Description != null )
                {
                    yield return node;
                }

                node = node.Next;
            }
        }

        private class NoOperation : IUndoItem
        {
            public string Description
            {
                get { return null; }
            }

            public void Redo()
            {
            }

            public void Undo()
            {
            }
        }

        private class Node : IUndoMarker
        {
            public Node Next, Previous;
            public IUndoItem UndoItem { get; private set; }


            public Node( IUndoItem item )
            {
                this.UndoItem = item;
            }

            public string Description
            {
                get { return this.UndoItem.Description; }
            }

            public bool IsCurrent
            {
                get { return currentNode == this; }
            }

            IUndoMarker IUndoMarker.Next
            {
                get { return this.Next; }
            }

            IUndoMarker IUndoMarker.Previous
            {
                get { return this.Previous; }
            }
        }
    }

    public interface IUndoMarker
    {
        string Description { get; }
        bool IsCurrent { get; }
        IUndoMarker Next { get; }
        IUndoMarker Previous { get; }
    }


    public interface IUndoItem
    {
        string Description { get; }
        void Redo();
        void Undo();
    }
}