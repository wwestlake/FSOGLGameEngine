module GameState

    type GameActionMode =
        | Stopped
        | Running

    type GameMode<'T> =
        | Normal
        | Creative
        | Spectator
        | Admin
        | Custom of 'T

    type GameDifficulty<'T> =
        | Tutorial
        | Easy
        | Normal
        | Hard
        | Hardcore
        | Custom of 'T

    type GameType =
        | SinglePlayer
        | Multiplayer

    type InstanceType =
        | Client
        | Server


