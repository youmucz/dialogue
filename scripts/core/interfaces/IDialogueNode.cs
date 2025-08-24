using Godot;
using System;

namespace Story.Dialogue.Core.Interfaces
{
    public interface IDialogueNode
    {
        #region runtime functions

        public void NodeEntered();
        public void NodeExecute();
        public void NodeExited();

        public void Stop()
        {
            NodeExited();
        }

        #endregion

    }
}

