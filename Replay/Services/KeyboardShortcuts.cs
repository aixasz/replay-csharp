﻿using System.Windows.Input;
using static Replay.Services.ReplCommand;
using static System.Windows.Input.Key;
using static System.Windows.Input.ModifierKeys;

namespace Replay.Services
{
    enum ReplCommand
    {
        EvaluateCurrentLine,
        ReevaluateCurrentLine,
        DuplicatePreviousLine,
        OpenIntellisense,
        GoToFirstLine,
        GoToLastLine,
        LineDown,
        LineUp
    }

    /// <summary>
    /// Maps keyboard keys to commands in the REPL
    /// </summary>
    static class KeyboardShortcuts
    {
        /// <summary>
        /// Given a key event, map it to a command in the REPL, or null if
        /// the key event does not correspond to a command in the REPL.
        /// </summary>
        public static ReplCommand? MapToCommand(KeyEventArgs key)
        {
            return
                key.Is(Enter) ? EvaluateCurrentLine :
                key.Is(Control, Enter) ? ReevaluateCurrentLine :
                key.Is(Alt, Up) ? DuplicatePreviousLine :
                key.Is(Control, Space) || key.Is(Tab) ? OpenIntellisense :
                key.Is(PageUp) || key.Is(Control, Up) ? GoToFirstLine :
                key.Is(PageDown) || key.Is(Control, Down) ? GoToLastLine :
                key.Is(Up) ? LineUp :
                key.Is(Down) ? LineDown :
                null as ReplCommand?;
        }

        private static bool Is(this KeyEventArgs args, Key test) =>
            args.Key == test && Keyboard.Modifiers == ModifierKeys.None;

        private static bool Is(this KeyEventArgs args, ModifierKeys modifier, Key test) =>
            Keyboard.Modifiers.HasFlag(modifier)
            && (
                args.Key == test
                || (args.SystemKey == test && args.Key == Key.System)
            );
    }

}
